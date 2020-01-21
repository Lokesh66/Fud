using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void DestroyChildrens(this Transform trans, GameObject skip = null)
    {
        foreach (Transform t in trans)
        {
            if (skip != null && skip == t.gameObject)
            {
                continue;
            }
            GameObject.Destroy(t.gameObject);
        }
    }

    public static T Remove<T>(this Stack<T> stack, T element)
    {
        T obj = stack.Pop();
        if (obj.Equals(element))
        {
            return obj;
        }
        else
        {
            T toReturn = stack.Remove(element);
            stack.Push(obj);
            return toReturn;
        }
    }
    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {

        // exit if possitions are equal or outside array
        if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
            (newIndex >= list.Count)) return;
        // local variables
        var i = 0;
        T tmp = list[oldIndex];
        // move element down and shift other elements up
        if (oldIndex < newIndex)
        {
            for (i = oldIndex; i < newIndex; i++)
            {
                list[i] = list[i + 1];
            }
        }
        // move element up and shift other elements down
        else
        {
            for (i = oldIndex; i > newIndex; i--)
            {
                list[i] = list[i - 1];
            }
        }
        // put element from position 1 to destination
        list[newIndex] = tmp;
    }

}
