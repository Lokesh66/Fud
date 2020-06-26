using System.Collections.Generic;
using UnityEngine.UI;
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

    public static Texture2D ToTexture2D(this Texture texture)
    {
        return Texture2D.CreateExternalTexture(
            texture.width,
            texture.height,
            TextureFormat.RGB24,
            false, false,
            texture.GetNativeTexturePtr());
    }

    public static Color Alpha(this Color color, float alpha)
    {
        color.a = Mathf.Clamp01(alpha);

        return color;
    }

    public static Image SetAlpha(this Image color, float alpha)
    {
        color.color = new Color(color.color.r, color.color.g, color.color.b, alpha);

        return color;
    }

    public static bool IsNOTNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
