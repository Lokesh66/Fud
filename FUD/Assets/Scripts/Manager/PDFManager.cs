using Paroxe.PdfRenderer;
using UnityEngine;


public class PDFManager : MonoBehaviour
{
    #region Singleton

    private static PDFManager instance = null;

    private PDFManager()
    {

    }

    public static PDFManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PDFManager>();
            }
            return instance;
        }
    }

    #endregion

    public PDFViewer pdfViewer;


    public void LoadDocument(string documentURL)
    {
        if (!string.IsNullOrEmpty(documentURL))
        {
            pdfViewer.gameObject.SetActive(true);

            pdfViewer.LoadDocumentFromWeb(documentURL);
        }
    }
}
