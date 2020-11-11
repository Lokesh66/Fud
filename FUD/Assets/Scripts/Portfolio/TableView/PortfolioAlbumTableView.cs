using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using frame8.ScrollRectItemsAdapter.Util.GridView;
using frame8.ScrollRectItemsAdapter.Util;
using frame8.Logic.Misc.Other.Extensions;
using UnityEngine.UI;
using UnityEngine;


namespace frame8.ScrollRectItemsAdapter.GridExample
{
	/// <summary>
	/// Implementation demonstrating the usage of a <see cref="GridAdapter{TParams, TCellVH}"/> for a simple gallery of remote images downloaded with a <see cref="SimpleImageDownloader"/>.
	/// It implements  <see cref="ILazyListSimpleDataManager{TItem}"/> to access the default interface implementations for common data manipulation functionality
	/// </summary>
	public class PortfolioAlbumTableView : GridAdapter<GridParams, PortfolioAlbumCellHolder>, ILazyListSimpleDataManager<MultimediaModel>
	{
		public UnityEngine.Events.UnityEvent OnItemsUpdated;

		private LazyList<MultimediaModel> _Data;

		public LazyList<MultimediaModel> Data { get { return _Data; } private set { _Data = value; } }

		public PortfolioAlbumView adataObject;


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
			Data = new LazyList<MultimediaModel> (CreateNewModel, adataObject.portfolioModels.Count);
		}

		/// <inheritdoc/>
		protected override void Start ()
		{
			base.Start ();
		}

		MultimediaModel CreateNewModel (int index)
		{
			return adataObject.portfolioModels [index];
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
		protected override void UpdateCellViewsHolder (PortfolioAlbumCellHolder viewsHolder)
		{
			var model = Data [viewsHolder.ItemIndex];

			viewsHolder.views.gameObject.transform.parent.GetComponent<PortfolioAlbumCell> ().SetView (model, adataObject.OnCellButtonAction);

			if ((viewsHolder.ItemIndex != 0 && viewsHolder.ItemIndex == Data.Count - 12 && _ScrollRect.velocity.y > 10) ||(Data.Count < 12 && viewsHolder.ItemIndex == Data.Count - 1))
			{
				if (adataObject != null)
					adataObject.OnAPICall();
			}
		}

		#endregion
	}

	/// <summary>All views holders used with GridAdapter should inherit from <see cref="CellViewsHolder"/></summary>
	public class PortfolioAlbumCellHolder : CellViewsHolder
	{
		public RemoteImageBehaviour remoteImageBehaviour;

		public override void CollectViews ()
		{
			base.CollectViews ();

			views.GetComponentAtPath("AlbumImage", out remoteImageBehaviour);
		}

		protected override RectTransform GetViews ()
		{
			return root.Find ("Views") as RectTransform;
		}
	}
}
