using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadedFilesHandler : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    EMediaType mediaType;


    public void Load(string[] urls, bool isDownloadedFile, EMediaType mediaType = EMediaType.Image)
    {
        //content.DestroyChildrens();

        this.mediaType = mediaType;

        for (int i = 0; i < urls.Length; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            cellObject.GetComponent<UploadedFileCell>().Load(urls[i], isDownloadedFile, mediaType);
        }
    }

    private void OnDisable()
    {
        content.DestroyChildrens();
    }
}
