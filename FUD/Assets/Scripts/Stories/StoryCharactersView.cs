using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StoryCharactersView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    List<StoryCharacterModel> characterModels;


    public void Load(List<StoryCharacterModel> characterModels)
    {
        this.characterModels = characterModels;

        SetView();
    }

    void SetView()
    {
        GameObject characterObject = null;

        for (int i = 0; i < characterModels.Count; i++)
        {
            characterObject = Instantiate(cellCache, content);
        }
    }


}
