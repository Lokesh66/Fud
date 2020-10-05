using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;


public class AudioStreamer : MonoBehaviour
{
    #region Singleton
    private static AudioStreamer instance = null;

    private AudioStreamer()
    {

    }

    public static AudioStreamer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioStreamer>();
            }

            return instance;
        }
    }

    #endregion

    public AudioSource audioSource;

    public Image seekImage;



    void Update()
    {
        if (audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                float value = (float)(audioSource.time / audioSource.clip.length);

                seekImage.fillAmount = value;
            }
        }
    }

    public void AudioStream(string audioURL, Action OnComplete)
    {
        StartCoroutine(StartStreaming(audioURL));
    }

    IEnumerator StartStreaming(string audioURL)
    {
        //string url = APIConstants.MEDIA_UPLOAD_BASE_URL + "adam/v1/downloadFile?" + "key_name=" + audioURL;

        UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.UNKNOWN);

        //webRequest.SetRequestHeader("Authorization", GetToken());

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(webRequest);

            if (clip)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    string GetToken()
    {
        return GameManager.Instance.apiHandler.GetToken();
    }
}
