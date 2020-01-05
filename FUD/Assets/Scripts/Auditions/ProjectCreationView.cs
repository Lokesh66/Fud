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
        GameManager.Instance.apiHandler.UpdateProjectDetails(titleField.text, budgetField.text, 1000000.ToString(), (status, response) => {

            if (status)
            {
                Debug.Log("Project Created Successfully");

                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Project Failed To Save");
            }
        });
    }
}
