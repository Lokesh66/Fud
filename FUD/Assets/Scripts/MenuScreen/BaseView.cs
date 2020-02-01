using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public Stack<GameObject> subScreens = new Stack<GameObject>();

    public EScreenType screenType;

    public virtual void ShowScreen()
    {
        gameObject.SetActive(true);

        EnableView();
    }

    protected virtual void EnableView()
    { 
    
    }

    protected virtual void OnAddSubView(GameObject addedObject)
    {
        if (subScreens.Count > 0)
        {
            subScreens.Peek().SetActive(false);
        }

        subScreens.Push(addedObject);

        addedObject.SetActive(true);
    }

    public virtual void OnRemoveLastSubView()
    {
        if (subScreens.Count > 0)
        {
            GameObject currentObject = subScreens.Pop();

            Destroy(currentObject);

            GameObject nextScreen = subScreens.Count > 0 ? subScreens.Peek() : null;

            nextScreen?.SetActive(true);
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

public enum EScreenType 
{
    Home,
    MyStories,
    Lounge,
    Forum
}
