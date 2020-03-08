using UnityEngine;
using DG.Tweening;

public class GalleryButtonsPanel : MonoBehaviour
{
    #region Singleton

    private static GalleryButtonsPanel instance = null;

    private GalleryButtonsPanel()
    {

    }

    public static GalleryButtonsPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GalleryButtonsPanel>();
            }
            return instance;
        }
    }
    #endregion
    public GameObject parentPanel;

    public RectTransform buttonsRectTransform;

    System.Action<int> OnButtonAction;

    float height;
    private void Awake()
    {
        height = buttonsRectTransform.rect.height;
    }
    private void OnEnable()
    {
        buttonsRectTransform.DOAnchorPosY(0f, 0.5f).OnStart(() => {
            buttonsRectTransform.anchoredPosition = new Vector2(0, -height);
        });
    }
    public void Load(System.Action<int> action)
    {
        parentPanel.SetActive(true);
        OnButtonAction = action;
    }

    public void ButtonClick(int index)
    { 
        parentPanel.SetActive(false);
        OnButtonAction?.Invoke(index);
    }
}
