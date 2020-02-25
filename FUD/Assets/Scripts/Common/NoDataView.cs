using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class NoDataView : MonoBehaviour
{
    public TextMeshProUGUI subTitleText;

    public TextMeshProUGUI buttonText;

    public Button createButton;


    public void SetView(NoDataModel noDataModel)
    {
        subTitleText.text = noDataModel.subTitle;

        buttonText.text = noDataModel.buttonName;

        createButton.onClick.RemoveAllListeners();

        if (noDataModel.buttonAction == null)
        {
            createButton.gameObject.SetActive(false);
        }
        else
        {
            createButton.onClick.AddListener(delegate { noDataModel.buttonAction?.Invoke(); });
        }
    }
}

[Serializable]
public class NoDataModel
{
    public string subTitle;
    public string buttonName;
    public Action buttonAction;
}
