using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public Stack<GameObject> subScreens = new Stack<GameObject>();

    public virtual void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    protected virtual void EnableView()
    { 
    
    }

    protected virtual void OnAddSubView(GameObject addedObject)
    {
        subScreens.Peek()?.SetActive(false);

        subScreens.Push(addedObject);

        addedObject.SetActive(true);
    }

    protected virtual void OnRemoveLastSubView()
    {
        if (subScreens.Count > 0)
        {
            subScreens.Pop();

            GameObject currentObject = subScreens.Count > 0 ? subScreens.Peek() : null;

            currentObject?.SetActive(true);
        }
    }

    public virtual void OnExitScreen()
    {
        int totalScreens = subScreens.Count;

        for (int i = totalScreens - 1; i >= 0; i++)
        {
            Destroy(subScreens.Pop());
        }

        subScreens.Clear();
        
        gameObject.SetActive(false);
    }
}
