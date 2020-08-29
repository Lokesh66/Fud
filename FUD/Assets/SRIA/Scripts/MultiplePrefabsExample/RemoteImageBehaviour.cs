using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using frame8.ScrollRectItemsAdapter.Util;
using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;


namespace frame8.ScrollRectItemsAdapter.MultiplePrefabsExample
{
	/// <summary>Utility behavior to be attached to a GameObject containing a RawImage for loading remote images using <see cref="SimpleImageDownloader"/>, optionally displaying a specific image during loading and/or on error</summary>
	[RequireComponent(typeof(RawImage))]
	public class RemoteImageBehaviour : MonoBehaviour
	{
		[Tooltip("If not assigned, will try to find it in this game object")]
		[SerializeField] public RawImage _RawImage;
#pragma warning disable 0649
		[SerializeField] Texture2D _LoadingTexture;
		[SerializeField] Texture2D _ErrorTexture;
#pragma warning restore 0649

		string _CurrentRequestedURL;
		bool _DestroyPending;
		Texture2D _RecycledTexture;


		void Awake()
		{
			if (!_RawImage)
				_RawImage = GetComponent<RawImage>();

            string path = Path.Combine(Application.persistentDataPath + APIConstants.TEMP_IMAGES_PATH);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(path);
            }
        }

		Queue<CustomWorker> workerQueue = new Queue<CustomWorker>();
		Queue<CustomWorker> workerQueuetwo = new Queue<CustomWorker>();
		bool change = false;

		void loadINSeperateThread()
		{
		}

		void LoadSprite(string filepath, string fileName, Action<byte[]> ResponseData)
		{
			change = !change;
			if (change)
			{
				byte[] response = null;
				CustomWorker.QueueWorker(
					this.workerQueue,
					null,
					(x, e) => {
						//// some custom do work logic.
						response = LoadTexture(filepath, fileName);
					},
					(x, e) => {
						//// some custom completed logic.
						ResponseData(response);
					},
					(e) => {
						//// some custom display error logic.
					},
					(x, e) => {
						//// Progress change logic.
					});

			}
			else
			{
				byte[] response = null;
				CustomWorker.QueueWorker(
					this.workerQueuetwo,
					null,
					(x, e) => {
						//// some custom do work logic.
						response = LoadTexture(filepath, fileName);
					},
					(x, e) => {
						//// some custom completed logic.
						ResponseData(response);
					},
					(e) => {
						//// some custom display error logic.
					},
					(x, e) => {
						//// Progress change logic.
					});

			}

		}

		byte[] LoadTexture(string filePath, string fileName)
		{
			byte[] fileData = File.ReadAllBytes(filePath);
			return fileData;
		}

		public void udpateToDefaultTexture()
		{
			Destroy(_RawImage.material.mainTexture);
		}

		void loadFromLocal(string filePath, string imageURL, bool refreshResources, bool loadCachedIfAvailable = true, Action<bool, bool> onCompleted = null, Action onCanceled = null)
		{
			LoadSprite(filePath, Path.GetFileName(imageURL), (byte[] obj) => {
				Debug.Log("CallBack");
				//UnityMainThreadDispatcher.Instance().Enqueue(() => {
					if (this != null && _RawImage != null)
					{
						_RawImage.texture = _LoadingTexture;

						if (_RecycledTexture == null)
							_RecycledTexture = new Texture2D(1, 1);

						if (obj != null && obj.Length > 0)
							_RecycledTexture.LoadImage(obj); //..this will auto-resize the texture dimensions.
						else
						{
							File.Delete(filePath);
						}
						if (_RawImage == null)
							return;

						Destroy(_RawImage.material.mainTexture);

						_RawImage.texture = _RecycledTexture;

						if (refreshResources)
							Resources.UnloadUnusedAssets();

						if (onCompleted != null)
							onCompleted(true, true);
					}
				});
			//});

		}

		/// <summary>Starts the loading, setting the current image to <see cref="_LoadingTexture"/>, if available. If the image is already in cache, and <paramref name="loadCachedIfAvailable"/>==true, will load that instead</summary>
		public void Load(string imageURL, bool loadCachedIfAvailable = true, Action<bool, bool> onCompleted = null, Action onCanceled = null)
		{
			if (_RawImage == null)
			{
				return;
			}

			if (imageURL == null || imageURL.Length <= 0)
			{
				Destroy(_RawImage.material.mainTexture);

				_RawImage.texture = _LoadingTexture;

				return;
			}

			string filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, Path.GetFileName(imageURL));

			if (File.Exists(filePath))
			{
				Destroy(_RawImage.material.mainTexture);

				_RawImage.texture = _LoadingTexture;

				string filesPath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, Path.GetFileName(imageURL));

				loadFromLocal(filesPath, imageURL, false, loadCachedIfAvailable, onCompleted, onCanceled);
			}
			else
			{
				if (_RawImage.texture)
				{
					_RecycledTexture = _RawImage.texture as Texture2D;
					if (_RecycledTexture == _LoadingTexture || _RecycledTexture == _ErrorTexture)
						_RecycledTexture = null;
				}
				else
					_RecycledTexture = null;

				_CurrentRequestedURL = imageURL;

				Destroy(_RawImage.material.mainTexture);

				_RawImage.texture = _LoadingTexture;

				var request = new SimpleImageDownloader.Request()
				{
					url = imageURL,
					onDone = result => {
						if (!_DestroyPending && imageURL == _CurrentRequestedURL)
						{ // this will be false if a new request was done during downloading, case in which the result will be ignored
							string path = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, Path.GetFileName(_CurrentRequestedURL));

							BackgroundWorker worker = new BackgroundWorker();

							byte[] response = result.getByteData();
							worker.DoWork += (sender, e) => {
								if (response.Length > 0)
								{   
									File.WriteAllBytes(path, response);
									//loadFromLocal(path, imageURL, true, loadCachedIfAvailable, onCompleted, onCanceled);
									Debug.Log("Calling Load");
									Load(_CurrentRequestedURL, loadCachedIfAvailable, onCompleted, onCanceled);
								}
								result.removeRequest();
							};
							worker.RunWorkerAsync(response);


							if (onCompleted != null)
								onCompleted(false, true);
						}
						else if (onCanceled != null)
							onCanceled();
					},
					onError = () => {
						if (!_DestroyPending && imageURL == _CurrentRequestedURL)
						{ // this will be false if a new request was done during downloading, case in which the result will be ignored
							if (_LoadingTexture != null)
							{
								Destroy(_RawImage.material.mainTexture);

								_RawImage.texture = _LoadingTexture;
							}
							if (onCompleted != null)
								onCompleted(false, false);
						}
						else if (onCanceled != null)
							onCanceled();
					}
				};
				SimpleImageDownloader.Instance.Enqueue(request);
			}
		}


		public enum ImageFilterMode : int
		{
			Nearest = 0,
			Biliner = 1,
			Average = 2
		}

		public static Texture2D ResizeTexture(Texture2D pSource, ImageFilterMode pFilterMode, float pScale)
		{

			//			byte[] response = null;


			//*** Variables
			int i;

			//*** Get All the source pixels
			Color[] aSourceColor = pSource.GetPixels(0);
			Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);

			//*** Calculate New Size
			float xWidth = pSource.width / 2;//Mathf.RoundToInt ((float)pSource.width / 2 * pScale);                     
			float xHeight = pSource.height / 2;//Mathf.RoundToInt ((float)pSource.height / 2 * pScale);

			//*** Make New
			Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);

			//*** Make destination array
			int xLength = (int)xWidth * (int)xHeight;
			Color[] aColor = new Color[xLength];

			Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);

			//*** Loop through destination pixels and process
			Vector2 vCenter = new Vector2();
			for (i = 0; i < xLength; i++)
			{

				//*** Figure out x&y
				float xX = (float)i % xWidth;
				float xY = Mathf.Floor((float)i / xWidth);

				//*** Calculate Center
				vCenter.x = (xX / xWidth) * vSourceSize.x;
				vCenter.y = (xY / xHeight) * vSourceSize.y;

				//*** Do Based on mode
				//*** Nearest neighbour (testing)
				if (pFilterMode == ImageFilterMode.Nearest)
				{

					//*** Nearest neighbour (testing)
					vCenter.x = Mathf.Round(vCenter.x);
					vCenter.y = Mathf.Round(vCenter.y);

					//*** Calculate source index
					int xSourceIndex = (int)((vCenter.y * vSourceSize.x) + vCenter.x);

					//*** Copy Pixel
					aColor[i] = aSourceColor[xSourceIndex];
				}

				//*** Bilinear
				else if (pFilterMode == ImageFilterMode.Biliner)
				{

					//*** Get Ratios
					float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
					float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);

					//*** Get Pixel index's
					int xIndexTL = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
					int xIndexTR = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));
					int xIndexBL = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
					int xIndexBR = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));

					//*** Calculate Color
					aColor[i] = Color.Lerp(
						Color.Lerp(aSourceColor[xIndexTL], aSourceColor[xIndexTR], xRatioX),
						Color.Lerp(aSourceColor[xIndexBL], aSourceColor[xIndexBR], xRatioX),
						xRatioY
					);
				}

				//*** Average
				else if (pFilterMode == ImageFilterMode.Average)
				{

					//*** Calculate grid around point
					int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
					int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
					int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
					int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);

					//*** Loop and accumulate
					//					Vector4 oColorTotal = new Vector4 ();
					Color oColorTemp = new Color();
					float xGridCount = 0;
					for (int iy = xYFrom; iy < xYTo; iy++)
					{
						for (int ix = xXFrom; ix < xXTo; ix++)
						{

							//*** Get Color
							oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];

							//*** Sum
							xGridCount++;
						}
					}

					//*** Average Color
					aColor[i] = oColorTemp / (float)xGridCount;
				}
			}

			//*** Set Pixels
			oNewTex.SetPixels(aColor);
			oNewTex.Apply();
			return oNewTex;


			//*** Return
		}

		void OnDestroy()
		{
			_DestroyPending = true;
		}
	}
}
public class CustomWorker : BackgroundWorker
{
	public CustomWorker(object sender)
	{
		this.Sender = sender;
	}

	public object Sender { get; private set; }

	public static void QueueWorker(
		Queue<CustomWorker> queue,
		object item,
		Action<object, DoWorkEventArgs> action,
		Action<object, RunWorkerCompletedEventArgs> actionComplete,
		Action<RunWorkerCompletedEventArgs> displayError,
		Action<object, ProgressChangedEventArgs> progressChange)
	{
		if (queue == null)
			throw new ArgumentNullException("queue");
		var worker = new CustomWorker(item);

		try
		{
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;

			worker.ProgressChanged += (sender, args) => {
				progressChange.Invoke(sender, args);
			};

			worker.DoWork += (sender, args) => {
				action.Invoke(sender, args);
			};

			worker.RunWorkerCompleted += (sender, args) => {
				actionComplete.Invoke(sender, args);
				queue.Dequeue();
				if (queue.Count > 0)
				{
					var next = queue.Peek();
					next.ReportProgress(0, "Performing operation...");
					next.RunWorkerAsync(next.Sender);
				}
				else
					displayError.Invoke(args);
			};

			queue.Enqueue(worker);
			if (queue.Count == 1)
			{
				var next = queue.Peek();
				next.ReportProgress(0, "Performing operation...");
				next.RunWorkerAsync(next.Sender);
			}
		}
		finally
		{
		}
	}
}
