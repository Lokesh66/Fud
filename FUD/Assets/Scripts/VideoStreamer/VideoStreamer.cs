using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public TextMeshProUGUI volumeText;

    public TextMeshProUGUI mediaLengthText;


    UniversalMediaPlayer currentPlayer;

    private Material _objectMaterial;

    private RawImage prevImage;

    private Vector2 _objectSize;
    private Vector2 _videoSize;
    private Vector2 _calcSize;
    private Vector2 _borderUVSize;

    float currentPosValue = -1;


    public void Play(RawImage rawImage, UniversalMediaPlayer mediaPlayer, EMediaType mediaType)
    {
        updatedRawImage.texture = rawImage.texture;

        updatedRawImage.gameObject.SetActive(true);

        currentPosValue = -1;

        positionSlider.value = 0;

        mediaPlayer.Play();

        mediaPlayer.AddEndReachedEvent(() => {

            updatedRawImage.gameObject.SetActive(false);
        });

        mediaPlayer.AddPositionChangedEvent((positon) =>
        {
            if (!dragHelper.IsDragged())
            {
                currentPosValue = positionSlider.value = positon;
            }

            Loader.Instance.StopLoading();
        });

        mediaPlayer.AddBufferingEvent(OnBuffering);

        StopAllCoroutines();

        updatedRawImage.color = mediaType == EMediaType.Video ? Color.white : Color.black;

        if (mediaType == EMediaType.Video)
        {
            StartCoroutine(UpdateVideoCanvasRatio(rawImage));
        }
        else
        {
            mediaPlayer.AddPreparedEvent((width, height) => {

                if (!mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Play();
                }
            });
        }

        int minutes;

        int seconds;

        int hours = (int)TimeSpan.FromMilliseconds(mediaPlayer.Length).TotalHours;

        if (hours > 0)
        {
            minutes = (int)TimeSpan.FromMilliseconds(mediaPlayer.Length).TotalHours % 60;
        }
        else
        {
            minutes = (int)TimeSpan.FromMilliseconds(mediaPlayer.Length).TotalMinutes;
        }

        if (minutes > 0)
        {
            seconds = (int)TimeSpan.FromMilliseconds(mediaPlayer.Length).TotalMinutes % 60;
        }
        else {
            seconds = (int)TimeSpan.FromMilliseconds(mediaPlayer.Length).TotalSeconds;
        }

        Debug.Log("Milliseconds = " + mediaPlayer.Length);
            
        Debug.Log("seconds = " + seconds);

        Debug.Log("minutes = " + minutes);

        Debug.Log("hours = " + hours);

        mediaLengthText.text = mediaPlayer.GetFormattedLength(false);

        mediaLengthText.text = hours > 0 ? string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds) : string.Format("{0:00}:{1:00}", minutes, seconds);

        currentPlayer = mediaPlayer;
    }

    void OnBuffering(float percentage)
    {
        Loader.Instance.StartLoading();
    }

    private IEnumerator UpdateVideoCanvasRatio(RawImage rawImage)
    {
        _objectMaterial = rawImage.material;

        Debug.Log("UpdateVideoCanvasRatio Called = " + rawImage.material);

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
        currentPlayer.Volume = (int)volumeSlider.value;

        volumeText.text = currentPlayer.Volume + "%";
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
}
