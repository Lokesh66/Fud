using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadedFilesHandler : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    EMediaType mediaType;


    public void Load(string[] paths, bool isDownloadedFile, EMediaType mediaType = EMediaType.Image)
    {
        //content.DestroyChildrens();

        this.mediaType = mediaType;

        UploadedFileCell fileCell = null;

        for (int i = 0; i < paths.Length; i++)
        {
            GameObject cellObject = Instantiate(cellCache, content);

            fileCell = cellObject.GetComponent<UploadedFileCell>();

            fileCell.Load(paths[i], isDownloadedFile, mediaType);
        }
    }

    private void OnDisable()
    {
        content.DestroyChildrens();
    }
}
