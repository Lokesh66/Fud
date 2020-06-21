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
        UnityWebRequest music = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.UNKNOWN);

        yield return music.SendWebRequest();

        if (music.isNetworkError)
        {
            Debug.Log(music.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(music);

            if (clip)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }
}
