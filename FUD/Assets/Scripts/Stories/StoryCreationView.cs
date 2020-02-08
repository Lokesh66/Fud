using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.IO;
using TMPro;


public class StoryCreationView : MonoBehaviour
{
    public RectTransform galleryPanel;

    public TMP_InputField storyTitleField;

    public TMP_InputField subTitleField;

    public TMP_Dropdown dropdown;

    public TMP_InputField descriptionField;

    public Image screenShotImage;

    public TextMeshProUGUI contentType;

    public TextMeshProUGUI filePath;

    public TextMeshProUGUI statusText;

    public TextMeshProUGUI canSupportMultipleText;


    MyStoriesController storiesController;

    List<Genre> genres;

    public void Load(MyStoriesController storiesController)
    {
        this.storiesController = storiesController;

        canSupportMultipleText.text = "Can Support Text " + NativeGallery.CanSelectMultipleFilesFromGallery().ToString();

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void OnBackButtonAction()
    {
        storiesController.OnRemoveLastSubView();
    }

    public void OnUploadAction()
    {
        PickImages(SystemInfo.maxTextureSize);

        GetAudioFromGallery();
    }

    public void OnSubmitAction()
    {
        string selectedGenreText = dropdown.options[dropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        Debug.Log("Genre Id = " + selectedGenre.id);

        GameManager.Instance.apiHandler.CreateStory(storyTitleField.text, subTitleField.text, descriptionField.text, selectedGenre.id, (status, response) => {

            if (status)
            {
                storiesController.OnRemoveLastSubView();
                Debug.Log("Story Uploaded Successfully");
            }
            else {
                Debug.LogError("Story Updation Failed");
            }
        });
    }

    public void OnScreenShotAction()
    {
        GetScreenShot();
    }

    public void OnPhotosGalleryAction()
    {
        PickImages(SystemInfo.maxTextureSize);
    }

    public void OnVideosAction()
    {
        GetGalleryVideos();
    }

    public void OnCancelAction()
    { 
        
    }     

    void Reset()
    {
        //storyTitleField.text = string.Empty;
        gameObject.SetActive(false);

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        descriptionField.text = string.Empty;
    }

    private void PickImages(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);

            canSupportMultipleText.text = path.Length.ToString();

            if (path != null)
            {
                // Create Texture from selected image

                Texture2D texture = NativeGallery.LoadImageAtPath(path[0], maxSize,true, true, false, UploadFile);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //NativeGallery.GetImageProperties(path);

                screenShotImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);

                screenShotImage.SetNativeSize();

                byte[] textureBytes = texture.EncodeToPNG();

                string filePath = APIConstants.IMAGES_PATH + "/GalleryPhotos";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.WriteAllBytes(filePath, textureBytes);

                // Assign texture to a temporary quad and destroy it after 5 seconds
                /*GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;*/

               // Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
               // Destroy(texture, 5f);
            }
        }, "Select a PNG image");

        Debug.Log("Permission result: " + permission);
    }

    void GetAudioFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetAudioFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image

                canSupportMultipleText.text = path;

                UploadFile(path);
                /*
                Texture2D texture = NativeGallery.LoadImageAtPath(path[0], maxSize, true, true, false, UploadFile);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                //NativeGallery.GetImageProperties(path);

                screenShotImage.sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.zero);

                screenShotImage.SetNativeSize();

                byte[] textureBytes = texture.EncodeToPNG();

                string filePath = APIConstants.IMAGES_PATH + "/GalleryPhotos";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.WriteAllBytes(filePath, textureBytes);

                // Assign texture to a temporary quad and destroy it after 5 seconds
                /*GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;*/

                // Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                // Destroy(texture, 5f);*/
            }
        }, "Select a PNG image");

        Debug.Log("Permission result: " + permission);
    }

    void UploadFile(string filePath)
    {
        this.filePath.text = "FilePath = "+ filePath;

        GameManager.Instance.apiHandler.UploadFile(filePath, (status, response) => { 
            
        });
    }

    void ShowGalleryPanel()
    { 
    
    }

    void GetScreenShot()
    { 
    
    }

    void GetGalleryVideos()
    { 
    
    }
}
