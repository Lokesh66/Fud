using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public RectTransform content;

    public GameObject projectCell;


    public void Load()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject projectObject = Instantiate(projectCell, content);

            projectObject.GetComponent<ProjectCell>().SetView();
        }
    }
}
