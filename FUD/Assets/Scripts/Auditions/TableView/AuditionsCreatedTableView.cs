using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using frame8.Logic.Misc.Other.Extensions;
using frame8.ScrollRectItemsAdapter.Util.GridView;
using frame8.ScrollRectItemsAdapter.Util;
using frame8.ScrollRectItemsAdapter.Util.Drawer;
using frame8.Logic.Misc.Visual.UI.MonoBehaviours;
using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using frame8.Logic.Misc.Visual.UI;

namespace frame8.ScrollRectItemsAdapter.GridExample
{
	/// <summary>
	/// Implementation demonstrating the usage of a <see cref="GridAdapter{TParams, TCellVH}"/> for a simple gallery of remote images downloaded with a <see cref="SimpleImageDownloader"/>.
	/// It implements  <see cref="ILazyListSimpleDataManager{TItem}"/> to access the default interface implementations for common data manipulation functionality
	/// </summary>
	public class AuditionsCreatedTableView : GridAdapter<GridParams, AuditionCreatedCellHolder>, ILazyListSimpleDataManager<Audition>
	{

		public UnityEngine.Events.UnityEvent OnItemsUpdated;

		private LazyList<Audition> _Data;

		public LazyList<Audition> Data { get { return _Data; } private set { _Data = value; } }

		public AuditionCreatedView adataObject;


		#region GridAdapter implementation

		/// <inheritdoc/>
		protected override void Awake ()
		{

			base.Awake ();
			if (_ScrollRect == null)
			{
				_ScrollRect = GetComponent<ScrollRect>();
			}
		}

		public void OnEnable ()
		{
			Data = new LazyList<Audition> (CreateNewModel, adataObject.createdAuditions.Count);
		}

		/// <inheritdoc/>
		protected override void Start ()
		{
			base.Start ();
		}

		void OnReceivedNewModels (int newCount)
		{
			Data.Clear ();
			Data.InitWithNewCount (newCount);

			ResetItems (Data.Count, true);
			if (OnItemsUpdated != null)
				OnItemsUpdated.Invoke ();
		}

		IEnumerator FetchItemModelsFromServer (int count, Action onDone)
		{
			// Simulating server delay
			yield return new WaitForSeconds (DrawerCommandPanel.Instance.serverDelaySetting.InputFieldValueAsInt);

			onDone ();
		}

		Audition CreateNewModel (int index)
		{
			return adataObject.createdAuditions [index];
		}

		/// <inheritdoc/>

		/// <summary>
		/// <para><paramref name="contentPanelEndEdgeStationary"/> is overridden by the corresponding setting in the drawer. This is because the <see cref="ILazyListSimpleDataManager{TItem}"/> </para>
		/// <para>calls refresh after any data modification, but it can't know about the drawer panel settings, since it calls the parameterless version of Refresh(), which calls this version</para>
		/// </summary>
		/// <param name="contentPanelEndEdgeStationary">ignored for this demo</param>
		/// <seealso cref="GridAdapter{TParams, TCellVH}.Refresh(bool, bool)"/>
		public override void Refresh (bool contentPanelEndEdgeStationary /*ignored*/, bool keepVelocity = false)
		{
			_CellsCount = Data.Count;
			base.Refresh (false, keepVelocity);
		}

		ScrollRect _ScrollRect;

		/// <summary> Called when a cell becomes visible </summary>
		/// <param name="viewsHolder"> use viewsHolder.ItemIndexto find your corresponding model and feed data into its views</param>
		protected override void UpdateCellViewsHolder (AuditionCreatedCellHolder viewsHolder)
		{
			var model = Data [viewsHolder.ItemIndex];

			viewsHolder.views.gameObject.transform.parent.GetComponent<AuditionCreatedCell> ().SetView (adataObject, model);

			Debug.Log("image_url = " + model.image_url);
			var imageURLAtRequest = model.image_url;

			int itemIndexAtRequest = viewsHolder.ItemIndex;

			viewsHolder.remoteImageBehaviour.Load(imageURLAtRequest, true, (fromCache, success) => {
				if (success)
				{
					if (!IsRequestStillValid(viewsHolder.ItemIndex, itemIndexAtRequest, imageURLAtRequest))
						return;
				}
			});

			if ((viewsHolder.ItemIndex != 0 && viewsHolder.ItemIndex == Data.Count - 12 && _ScrollRect.velocity.y > 10) ||(Data.Count < 12 && viewsHolder.ItemIndex == Data.Count - 1))
			{
				Debug.LogError("It's Reaching to Last Index");

				if (adataObject != null)
					adataObject.OnAPICall();
			}
		}

		bool IsRequestStillValid(int itemIndex, int itemIdexAtRequest, string imageURLAtRequest)
		{
			return
				_CellsCount > itemIndex// be sure the index still points to a valid model
				&& itemIdexAtRequest == itemIndex// be sure the view's associated model index is the same (i.e. the viewsHolder wasn't re-used)
				&& imageURLAtRequest == Data[itemIndex].image_url; // be sure the model at that index is the same (could have changed if ChangeItemCountTo would've been called meanwhile)
		}

		#endregion
	}

	/// <summary>All views holders used with GridAdapter should inherit from <see cref="CellViewsHolder"/></summary>
	public class AuditionCreatedCellHolder : CellViewsHolder
	{
		public RemoteImageBehaviour remoteImageBehaviour;

		public override void CollectViews ()
		{
			base.CollectViews ();

			views.GetComponentAtPath("LeftImage", out remoteImageBehaviour);
		}

		protected override RectTransform GetViews ()
		{
			return root.Find ("Views") as RectTransform;
		}
	}
}
