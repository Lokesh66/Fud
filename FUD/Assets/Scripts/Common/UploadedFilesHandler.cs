using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadedFilesHandler : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    public void Load(List<string> urls)
    {
        content.DestroyChildrens();

        for (int i = 0; i < urls.Count; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            cellObject.GetComponent<UploadedFileCell>().Load(urls[i]);
        }
    }
}
