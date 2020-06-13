using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class VideoStreamer : MonoBehaviour
{
    #region Singleton
    private static VideoStreamer instance = null;

    private VideoStreamer()
    {

    }

    public static VideoStreamer Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<VideoStreamer>();
            }

            return instance;
        }
    }

    #endregion

    #region Constants
    private const float TRANSITION_TIME = 0.5f;
    #endregion

    #region Variables
    public Image seekImage;

    public GameObject backGroundGameObject;

    private CanvasGroup canvasGroup = null;

    private RenderTexture renderTexture = null;

    private RawImage rawImage = null;

    private VideoPlayer videoPlayer = null;

    #endregion

    #region Events
    private System.Action OnComplete = null;
    #endregion

    private void Init()
    {
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        Resolution resolution = Screen.currentResolution;

        renderTexture = new RenderTexture((int) resolution.width, (int) resolution.height, 0);
        renderTexture.format = RenderTextureFormat.RGB111110Float;

        rawImage = gameObject.AddComponent<RawImage>();
        rawImage.texture = renderTexture;
        rawImage.enabled = false;

        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.playOnAwake = false;
        videoPlayer.waitForFirstFrame = false;
        videoPlayer.isLooping = false;
        videoPlayer.skipOnDrop = true;
        videoPlayer.playbackSpeed = 1.0f;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.aspectRatio = VideoAspectRatio.FitOutside;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

        videoPlayer.prepareCompleted += PrepareCompleted;
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.errorReceived += ErrorReceived;
    }

    private void Start()
    {
        //StreamVideo("https://www.radiantmediaplayer.com/media/bbb-360p.mp4",null);
    }

    private void Update()
    {
        if(videoPlayer != null)
        {
            if (videoPlayer.isPlaying)
            {
                float value = (float)(videoPlayer.time / videoPlayer.length);

                seekImage.fillAmount = value;
            }
        }
    }

    public void StreamVideo(string url, System.Action onComplete)
    {
        if(videoPlayer == null)
        {
            Init();
        }

        videoPlayer.url = url;

        videoPlayer.Prepare();

        EnableBackground(true);

        OnComplete = onComplete;
    }

    private void PrepareCompleted(VideoPlayer source)
    {
        source.controlledAudioTrackCount = source.audioTrackCount;
        for(int i=0; i < source.audioTrackCount; i++)
        {
            source.EnableAudioTrack((ushort)i, true);
        }

        source.Play();

        canvasGroup.DOFade(1.0f, TRANSITION_TIME).OnStart(() => 
        {
            rawImage.enabled = true;
        }).OnComplete(() =>
        {
        });
    }

    private void LoopPointReached(VideoPlayer source)
    {
        StopVideo();
    }

    private void ErrorReceived(VideoPlayer source, string message)
    {
        StopVideo();
    }

    private void EnableBackground(bool enable)
    {
        backGroundGameObject.SetActive(enable);
    }

    private void StopVideo()
    {
        videoPlayer.Stop();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0.0f, TRANSITION_TIME).OnStart(() => 
        {
            OnComplete?.Invoke();
            OnComplete = null;

            EnableBackground(false);
        }).OnComplete(() =>
        {
            rawImage.enabled = false;
        }));

        sequence.Play();
    }

    Action<Texture> OnFirstFrameReady;

    public void GetThumbnailImage(string videoURL, Action<Texture> action)
    {
        if (videoPlayer == null)
        {
            Init();
        }

        videoPlayer.sendFrameReadyEvents = true;

        videoPlayer.frameReady += OnFirstFrame;

        this.OnFirstFrameReady = action;

        videoPlayer.url = videoURL;

        videoPlayer.time = 0;

        videoPlayer.Play();

        videoPlayer.frame = 0;
    }

    Texture2D videoFrame;

    void OnFirstFrame(VideoPlayer source, long frameIdx)
    {
        Debug.Log("OnFirstFrame Called");

        videoFrame = new Texture2D(2, 2);

        RenderTexture renderTexture = source.texture as RenderTexture;

        if (videoFrame.width != renderTexture.width || videoFrame.height != renderTexture.height)
        {
            videoFrame.Resize(renderTexture.width, renderTexture.height);
        }

        RenderTexture.active = renderTexture;

        videoFrame.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        videoFrame.Apply();

        source.frameReady -= OnFirstFrame;

        source.sendFrameReadyEvents = false;

        StopVideo();

        RenderTexture.active = null;

        OnFirstFrameReady?.Invoke(videoFrame);
    }
}
