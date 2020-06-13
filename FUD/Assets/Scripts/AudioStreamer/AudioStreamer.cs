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
        WWW _www = new WWW("https://geekanddummy.com/wp-content/uploads/2014/02/ambient-noise-server-room.mp3");

        yield return _www;

        
        audioSource.clip = _www.GetAudioClip(false, true, AudioType.MPEG);

        //_m1SongTime = musicOne.clip.length + Time.time;

        audioSource.Play();
    }
}
