using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ProjectCreationView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField budgetField;

    public TextMeshProUGUI durationField;


    public void OnSubmitProject()
    {
        Debug.Log("apiHandler = " + GameManager.Instance.apiHandler);

        Debug.Log("titleField = " + titleField.text);

        Debug.Log("budgetField = " + budgetField.text);

        GameManager.Instance.apiHandler.UpdateProjectDetails(titleField.text, budgetField.text, 1000000.ToString(), (status, response) => {

            if (status)
            {
                Debug.Log("Project Created Successfully");

                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Project Failed To Save");
            }
        });
    }
}
