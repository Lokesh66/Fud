using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadedFilesHandler : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;

    public RectTransform mediaButtonTrans;

    public bool isSingleImage = false;


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

        mediaButtonTrans?.gameObject.SetActive(!isSingleImage);

        mediaButtonTrans?.SetAsLastSibling();
    }

    private void OnDisable()
    {
        if (mediaButtonTrans != null)
        {
            content.DestroyChildrens(mediaButtonTrans?.gameObject);
        }
    }
}
