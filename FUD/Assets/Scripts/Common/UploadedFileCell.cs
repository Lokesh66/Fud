using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class UploadedFileCell : MonoBehaviour
{
    public Image selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public void Load(string imagePath)
    {
        Texture2D texture = NativeGallery.LoadImageAtPath(imagePath);

        // Assign texture to a temporary quad and destroy it after 5 seconds
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
        quad.transform.forward = Camera.main.transform.forward;
        quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

        Material material = quad.GetComponent<Renderer>().material;
        if (!material.shader.isSupported) // happens when Standard shader is not included in the build
            material.shader = Shader.Find("Legacy Shaders/Diffuse");

        material.mainTexture = texture;

        size1Text.text = selectedImage.rectTransform.sizeDelta.x + ", " + selectedImage.rectTransform.sizeDelta.y;

        size2Text.text = texture.width + ", " + texture.height;

        selectedImage.sprite = Sprite.Create(texture, new Rect(0, 0, selectedImage.rectTransform.sizeDelta.x, selectedImage.rectTransform.sizeDelta.y), new Vector2(0.5f, 0.5f));

        selectedImage.SetNativeSize();



        // If a procedural texture is not destroyed manually, 
        // it will only be freed after a scene change
        //Destroy(texture, 5f);
    }
}
