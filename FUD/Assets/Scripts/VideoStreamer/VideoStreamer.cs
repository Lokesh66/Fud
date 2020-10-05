using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using UMP;


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

    public GameObject closeObject;

    private CanvasGroup canvasGroup = null;

    private RenderTexture renderTexture = null;

    private RawImage rawImage = null;

    private VideoPlayer videoPlayer = null;

    #endregion

    #region Events
    private System.Action OnComplete = null;
    #endregion


    #region UniversalMediaPlayer

    public DragHelper dragHelper;

    private const string SHADER_BORDER_U_WIDTH = "_BorderUWidth";
    private const string SHADER_BORDER_V_WIDTH = "_BorderVWidth";

    public Camera topCamera;

    public GameObject pauseObject;

    public GameObject playObject;

    public RawImage updatedRawImage;


    public Slider positionSlider;

    public Slider volumeSlider;


    UniversalMediaPlayer currentPlayer;

    private Material _objectMaterial;

    private RawImage prevImage;

    private Vector2 _objectSize;
    private Vector2 _videoSize;
    private Vector2 _calcSize;
    private Vector2 _borderUVSize;

    float currentPosValue = -1;


    public void Play(RawImage rawImage, UniversalMediaPlayer mediaPlayer)
    {
        updatedRawImage.texture = rawImage.texture;

        updatedRawImage.gameObject.SetActive(true);

        currentPosValue = -1;

        positionSlider.value = 0;

        //if (prevImage != null)
        //{
        //    StopCoroutine(UpdateVideoCanvasRatio(prevImage));
        //}

        mediaPlayer.Play();

        mediaPlayer.AddPositionChangedEvent((positon) =>
        {
            if (!dragHelper.IsDragged())
            {
                currentPosValue = positionSlider.value = positon;
            }
        });

        StopAllCoroutines();

        StartCoroutine(UpdateVideoCanvasRatio(rawImage));

        //prevImage = rawImage;

        currentPlayer = mediaPlayer;
    }

    private IEnumerator UpdateVideoCanvasRatio(RawImage rawImage)
    {
        _objectMaterial = rawImage.material;

        Debug.Log("UpdateVideoCanvasRatio Called");

        while (true)
        {
            if (_objectSize == Vector2.zero)
                _objectSize = GetPixelSizeOfRawImage(rawImage);

            updatedRawImage.texture = rawImage.texture;

            _calcSize = Vector2.zero;
            _calcSize.x = (_objectSize.y / _videoSize.y) * _videoSize.x;
            if (_calcSize.x < _objectSize.x)
                _calcSize.y = _objectSize.y;
            else
                _calcSize = new Vector2(_objectSize.x, (_objectSize.x / _videoSize.x) * _videoSize.y);

            _borderUVSize = new Vector2((1 - (_calcSize.x / _objectSize.x)) * 0.5f, (1 - (_calcSize.y / _objectSize.y)) * 0.5f);
            _objectMaterial.SetFloat(SHADER_BORDER_U_WIDTH, _borderUVSize.x);
            _objectMaterial.SetFloat(SHADER_BORDER_V_WIDTH, _borderUVSize.y);
            yield return null;
        }
    }

    public static Vector2 GetPixelSizeOfRawImage(RawImage rawImage)
    {
        if (rawImage == null)
            return Vector2.zero;

        return new Vector2(instance.updatedRawImage.rectTransform.rect.width, instance.updatedRawImage.rectTransform.rect.height);
    }

    public void OnPositionValueChange()
    {
        if (Mathf.Abs(currentPosValue - positionSlider.value) > 0.05f)
        {
            dragHelper.OnBeginDrag(null);

            currentPosValue = currentPlayer.Position = positionSlider.value;

            dragHelper.ResetIsDragged();
        }
    }

    public void OnVolumeValueChange()
    {
        //currentPlayer.
    }

    public void OnPauseButtonAction()
    {
        currentPlayer?.Pause();

        playObject.SetActive(false);

        pauseObject.SetActive(true);
    }

    public void OnPlayButtonAction()
    {
        currentPlayer?.Play();

        pauseObject.SetActive(false);

        playObject.SetActive(true);
    }

    public void OnCloseAction()
    {
        currentPlayer?.Stop();

        updatedRawImage.gameObject.SetActive(false);
    }

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

    public void StreamVideo(string url, Action onComplete)
    {
        if(videoPlayer == null)
        {
            Init();
        }

        Loader.Instance.StartLoading();

        videoPlayer.url = url;

        videoPlayer.Prepare();

        EnableBackground(true);

        OnComplete = onComplete;
    }

    public void OnCloseButtonAction()
    {
        StopVideo();
    }

    private void PrepareCompleted(VideoPlayer source)
    {
        source.controlledAudioTrackCount = source.audioTrackCount;

        for(int i=0; i < source.audioTrackCount; i++)
        {
            source.EnableAudioTrack((ushort)i, true);
        }

        Loader.Instance.StopLoading();

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

        closeObject.SetActive(enable);
    }

    private void StopVideo()
    {
        videoPlayer.sendFrameReadyEvents = false;

        videoPlayer.frameReady -= OnFirstFrame;

        videoPlayer.Stop();

        Debug.Log("Stop Video Called");

        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0.0f, TRANSITION_TIME).OnStart(() => 
        {
            OnComplete?.Invoke();
            OnComplete = null;

            EnableBackground(false);

            closeObject.SetActive(false);
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
