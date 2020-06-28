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
	public class ProjectOfferedTableView : GridAdapter<GridParams, ProjectOfferedCellViewHolder>, ILazyListSimpleDataManager<ProjectOfferedModel>
	{

		public UnityEngine.Events.UnityEvent OnItemsUpdated;

		private LazyList<ProjectOfferedModel> _Data;

		public LazyList<ProjectOfferedModel> Data { get { return _Data; } private set { _Data = value; } }

		public ProjectHandler adataObject;


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
			Data = new LazyList<ProjectOfferedModel> (CreateNewModel, adataObject.offeredModels.Count);
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

		ProjectOfferedModel CreateNewModel (int index)
		{
			return adataObject.offeredModels [index];
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
		protected override void UpdateCellViewsHolder (ProjectOfferedCellViewHolder viewsHolder)
		{
			var model = Data [viewsHolder.ItemIndex];

			viewsHolder.views.gameObject.transform.parent.GetComponent<ProjectOfferedCell> ().SetView (model, adataObject.OnOfferedProjectAction);
			
			if ((viewsHolder.ItemIndex != 0 && viewsHolder.ItemIndex == Data.Count - 12 && _ScrollRect.velocity.y > 10) ||(Data.Count < 12 && viewsHolder.ItemIndex == Data.Count - 1))
			{
				Debug.LogError("It's Reaching to Last Index");

				if (adataObject != null)
					adataObject.OnAPICall();
			}
		}

		#endregion
	}

	/// <summary>All views holders used with GridAdapter should inherit from <see cref="CellViewsHolder"/></summary>
	public class ProjectOfferedCellViewHolder : CellViewsHolder
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
