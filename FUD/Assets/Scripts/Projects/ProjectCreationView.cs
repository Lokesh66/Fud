using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ProjectCreationView : MonoBehaviour
{
    #region Singleton

    private static ProjectCreationView instance = null;

    private ProjectCreationView()
    {

    }

    public static ProjectCreationView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ProjectCreationView>();
            }
            return instance;
        }
    }
    #endregion

    public Transform parentPanel;

    public TMP_InputField titleField;

    public TMP_InputField budgetField;

    public TextMeshProUGUI durationField;

    public TMP_Text errorText;

    System.Action backAction;
    public void SetView(System.Action action)
    {
        parentPanel.gameObject.SetActive(true);
        backAction = action;
    }

    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke();
        backAction = null;
    }

    public void OnSubmitProject()
    {
        Debug.Log("apiHandler = " + GameManager.Instance.apiHandler);

        Debug.Log("titleField = " + titleField.text);

        Debug.Log("budgetField = " + budgetField.text);

        GameManager.Instance.apiHandler.UpdateProjectDetails(titleField.text, budgetField.text, 1000000.ToString(), (status, response) => {

            if (status)
            {
                Debug.Log("Project Created Successfully");

                BackButtonAction();
            }
            else
            {
                Debug.LogError("Project Failed To Save");
            }
        });
    }

    void ShowErrorMessage(string message)
    {
        errorText.text = message;
        if (IsInvoking("DisableErrorMessage"))
            CancelInvoke("DisableErrorMessage");
        Invoke("DisableErrorMessage", 2.0f);
    }

    void DisableErrorMessage()
    {
        errorText.DOFade(0f, 0.5f).OnComplete(() => {
            errorText.text = string.Empty;
            errorText.color = Color.red;
        });
    }
}
