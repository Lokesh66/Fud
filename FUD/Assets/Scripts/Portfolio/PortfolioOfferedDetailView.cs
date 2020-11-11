using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PortfolioOfferedDetailView : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI phoneText;

    public TextMeshProUGUI roleText;

    public TextMeshProUGUI roleCategoryText;

    public TextMeshProUGUI mailText;


    public GameObject mediaCell;

    public RectTransform mediaContent;


    PortfolioActivityModel activityModel;

    List<MultimediaModel> mediaList;


    public void Load(PortfolioActivityModel activityModel)
    {
        GameManager.Instance.apiHandler.GetPortfolioProfileInfo(activityModel.portfolio_id, (status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            if (responseModel != null)
            {
                this.activityModel = responseModel.data[0];

                gameObject.SetActive(true);

                mediaList = this.activityModel.PortfolioMedia;

                SetView();

                LoadMedia();
            }
        });
    }

    void SetView()
    {
        nameText.text = activityModel.Users.name;

        phoneText.text = activityModel.Users.phone.ToString();

        roleText.text = activityModel.Users.Craftroles.name;

        roleCategoryText.text = activityModel.Users.RoleCategories.name;

        mailText.text = activityModel.Users.email_id;
    }

    void LoadMedia()
    {
        mediaContent.DestroyChildrens();

        CreatedPortfolioMediaCell _mediaCell = null;

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            _mediaCell = mediaObject.GetComponent<CreatedPortfolioMediaCell>();

            _mediaCell.SetView(mediaList[i], null);
        }
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}
