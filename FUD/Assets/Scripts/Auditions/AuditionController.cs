using UnityEngine;


public class AuditionController : MonoBehaviour
{
    public RectTransform content;

    public GameObject auditionCell;


    public void Load()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject auditionObject = Instantiate(auditionCell, content);

            auditionObject.GetComponent<AuditionCell>().SetView(i);
        }
    }
}
