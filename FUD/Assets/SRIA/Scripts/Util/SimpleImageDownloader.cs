using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;

namespace frame8.ScrollRectItemsAdapter.Util
{
	/// <summary>
	/// <para>A utility singleton class for downloading images using a LIFO queue for the requests. <see cref="MaxConcurrentRequests"/> can be used to limit the number of concurrent requests. </para> 
	/// <para>Default is <see cref="DEFAULT_MAX_CONCURRENT_REQUESTS"/>. Each request is executed immediately if there's room for it. When the queue is full, the downloder starts checking each second if a slot is freed, after which re-enters the loop.</para> 
	/// </summary>
	public class SimpleImageDownloader : MonoBehaviour
	{
		public static SimpleImageDownloader Instance {
			get {
				if (_Instance == null)
					_Instance = new GameObject (typeof(SimpleImageDownloader).Name).AddComponent<SimpleImageDownloader> ();

				return _Instance;
			}
		}

		static SimpleImageDownloader _Instance;


		public int MaxConcurrentRequests { get; set; }

		const int DEFAULT_MAX_CONCURRENT_REQUESTS = 15;

		List<Request> _QueuedRequests = new List<Request> ();
		List<Request> _ExecutingRequests = new List<Request> ();
		WaitForSeconds _Wait1Sec = new WaitForSeconds (1f);


		IEnumerator Start ()
		{
			if (MaxConcurrentRequests == 0)
				MaxConcurrentRequests = DEFAULT_MAX_CONCURRENT_REQUESTS;

			while (true) {
				while (_ExecutingRequests.Count >= MaxConcurrentRequests) {
					System.GC.Collect ();
					yield return _Wait1Sec;
				}

				int lastIndex = _QueuedRequests.Count - 1;
				if (lastIndex >= 0) {
					var lastRequest = _QueuedRequests [lastIndex];
					_QueuedRequests.RemoveAt (lastIndex);

					StartCoroutine (DownloadCoroutine (lastRequest));
				}

				yield return null;
			}
		}

		void OnDestroy ()
		{
			_Instance = null;
		}

		public void Enqueue (Request request)
		{
			_QueuedRequests.Add (request);
		}

		IEnumerator DownloadCoroutine (Request request)
		{
			_ExecutingRequests.Add (request);
			var www = new WWW (request.url);

			yield return www;

			if (string.IsNullOrEmpty (www.error)) {
				if (request.onDone != null) {
					var result = new Result (www);
					if (www.progress >= 1.0f && www.isDone) {
						request.onDone (result);
					} else {
						StartCoroutine(DownloadCoroutine (request));
					}
				}
			} else {
				if (request.onError != null)
					request.onError ();
			}
			www.Dispose ();
			_ExecutingRequests.Remove (request);
		}

		public class Request
		{
			public string url;
			public Action<Result> onDone;
			public Action onError;
			public byte bytees;
		}

		public class Result
		{
			WWW _UsedWWW;


			internal Result (WWW www)
			{
				_UsedWWW = www;
			}

			public byte[] getByteData ()
			{
				return _UsedWWW.bytes;
			}

			public void removeRequest ()
			{
				_UsedWWW.Dispose ();
			}

			public Texture2D CreateTextureFromReceivedData ()
			{
				return _UsedWWW.texture;
			}

			public void LoadTextureInto (Texture2D existingTexture)
			{


				_UsedWWW.LoadImageIntoTexture (existingTexture);
			}
		}


		public enum ImageFilterMode : int
		{
			Nearest = 0,
			Biliner = 1,
			Average = 2
		}

		public static Texture2D ResizeTexture (Texture2D pSource, ImageFilterMode pFilterMode, float pScale)
		{


			//*** Variables
			int i;
 
			//*** Get All the source pixels
			Color[] aSourceColor = pSource.GetPixels (0);
			Vector2 vSourceSize = new Vector2 (pSource.width, pSource.height);
 
			//*** Calculate New Size
			float xWidth = pSource.width / 2;//Mathf.RoundToInt ((float)pSource.width / 2 * pScale);                     
			float xHeight = pSource.height / 2;//Mathf.RoundToInt ((float)pSource.height / 2 * pScale);
 
			//*** Make New
			Texture2D oNewTex = new Texture2D ((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);
 
			//*** Make destination array
			int xLength = (int)xWidth * (int)xHeight;
			Color[] aColor = new Color[xLength];
 
			Vector2 vPixelSize = new Vector2 (vSourceSize.x / xWidth, vSourceSize.y / xHeight);
 
			//*** Loop through destination pixels and process
			Vector2 vCenter = new Vector2 ();
			for (i = 0; i < xLength; i++) {
 
				//*** Figure out x&y
				float xX = (float)i % xWidth;
				float xY = Mathf.Floor ((float)i / xWidth);
 
				//*** Calculate Center
				vCenter.x = (xX / xWidth) * vSourceSize.x;
				vCenter.y = (xY / xHeight) * vSourceSize.y;
 
				//*** Do Based on mode
				//*** Nearest neighbour (testing)
				if (pFilterMode == ImageFilterMode.Nearest) {
 
					//*** Nearest neighbour (testing)
					vCenter.x = Mathf.Round (vCenter.x);
					vCenter.y = Mathf.Round (vCenter.y);
 
					//*** Calculate source index
					int xSourceIndex = (int)((vCenter.y * vSourceSize.x) + vCenter.x);
 
					//*** Copy Pixel
					aColor [i] = aSourceColor [xSourceIndex];
				}
 
        //*** Bilinear
        else if (pFilterMode == ImageFilterMode.Biliner) {
 
					//*** Get Ratios
					float xRatioX = vCenter.x - Mathf.Floor (vCenter.x);
					float xRatioY = vCenter.y - Mathf.Floor (vCenter.y);
 
					//*** Get Pixel index's
					int xIndexTL = (int)((Mathf.Floor (vCenter.y) * vSourceSize.x) + Mathf.Floor (vCenter.x));
					int xIndexTR = (int)((Mathf.Floor (vCenter.y) * vSourceSize.x) + Mathf.Ceil (vCenter.x));
					int xIndexBL = (int)((Mathf.Ceil (vCenter.y) * vSourceSize.x) + Mathf.Floor (vCenter.x));
					int xIndexBR = (int)((Mathf.Ceil (vCenter.y) * vSourceSize.x) + Mathf.Ceil (vCenter.x));
 
					//*** Calculate Color
					aColor [i] = Color.Lerp (
						Color.Lerp (aSourceColor [xIndexTL], aSourceColor [xIndexTR], xRatioX),
						Color.Lerp (aSourceColor [xIndexBL], aSourceColor [xIndexBR], xRatioX),
						xRatioY
					);
				}
 
        //*** Average
        else if (pFilterMode == ImageFilterMode.Average) {
 
					//*** Calculate grid around point
					int xXFrom = (int)Mathf.Max (Mathf.Floor (vCenter.x - (vPixelSize.x * 0.5f)), 0);
					int xXTo = (int)Mathf.Min (Mathf.Ceil (vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
					int xYFrom = (int)Mathf.Max (Mathf.Floor (vCenter.y - (vPixelSize.y * 0.5f)), 0);
					int xYTo = (int)Mathf.Min (Mathf.Ceil (vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);
 
					//*** Loop and accumulate
//					Vector4 oColorTotal = new Vector4 ();
					Color oColorTemp = new Color ();
					float xGridCount = 0;
					for (int iy = xYFrom; iy < xYTo; iy++) {
						for (int ix = xXFrom; ix < xXTo; ix++) {
 
							//*** Get Color
							oColorTemp += aSourceColor [(int)(((float)iy * vSourceSize.x) + ix)];
 
							//*** Sum
							xGridCount++;
						}
					}
 
					//*** Average Color
					aColor [i] = oColorTemp / (float)xGridCount;
				}
			}
 
			//*** Set Pixels
			oNewTex.SetPixels (aColor);
			oNewTex.Apply ();
 
			//*** Return
			return oNewTex;
		}
	}


}

