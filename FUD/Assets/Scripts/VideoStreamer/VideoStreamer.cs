using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UMP;
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

    #region UniversalMediaPlayer

    public RectTransform musicDiscTrans;

    public DragHelper dragHelper;

    public CanvasGroup canvasGroup;

    private const string SHADER_BORDER_U_WIDTH = "_BorderUWidth";
    private const string SHADER_BORDER_V_WIDTH = "_BorderVWidth";

    public Camera topCamera;

    public GameObject pauseObject;

    public GameObject playObject;

    public GameObject audioObject;

    public RawImage updatedRawImage;


    public Slider positionSlider;

    public Slider volumeSlider;

    public TextMeshProUGUI volumeText;

    public TextMeshProUGUI mediaLengthText;


    UniversalMediaPlayer currentPlayer;

    EMediaType mediaType;

    private Material _objectMaterial;

    private Vector2 _objectSize;
    private Vector2 _videoSize;
    private Vector2 _calcSize;
    private Vector2 _borderUVSize;

    Vector3 rotationValue = new Vector3(0, 0, 360);

    RotateMode rotateMode = RotateMode.FastBeyond360;

    Sequence mySequence = null;

    bool isShowingUI = false;

    float currentPosValue = -1;


    public void Play(RawImage rawImage, UniversalMediaPlayer mediaPlayer, EMediaType mediaType)
    {
        updatedRawImage.texture = rawImage.texture;

        this.mediaType = mediaType;

        audioObject.SetActive(mediaType == EMediaType.Audio);

        volumeSlider.value = mediaPlayer.Volume;
        
        updatedRawImage.gameObject.SetActive(true);

        currentPosValue = -1;

        positionSlider.value = 0;

        mediaPlayer.Play();

        mediaPlayer.AddEndReachedEvent(() => {

            ResetView();
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
            UpdateAudioView();

            mediaPlayer.AddPreparedEvent((width, height) => {

                if (!mediaPlayer.IsPlaying)
                {
                    mediaPlayer.Play();
                }
            });
        }

        currentPlayer = mediaPlayer;

        StopCoroutine(UpdateMediaLength());

        StartCoroutine(UpdateMediaLength());
    }

    IEnumerator UpdateMediaLength()
    {
        while (currentPlayer.Length <= 0)
        {
            yield return new WaitForEndOfFrame();

            mediaLengthText.text = currentPlayer.GetFormattedLength(false);
        }
    }

    void OnBuffering(float percentage)
    {
        Loader.Instance.StartLoading();
    }

    private IEnumerator UpdateVideoCanvasRatio(RawImage rawImage)
    {
        _objectMaterial = rawImage.material;

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
        StopMusic();

        currentPlayer?.Pause();

        playObject.SetActive(false);

        pauseObject.SetActive(true);
    }

    public void OnPlayButtonAction()
    {
        if (mediaType == EMediaType.Audio)
        {
            UpdateAudioView();
        }

        currentPlayer?.Play();

        pauseObject.SetActive(false);

        playObject.SetActive(true);
    }

    public void OnCloseAction()
    {
        currentPlayer?.Stop();

        updatedRawImage.gameObject.SetActive(false);
    }

    public void OnPointerClick()
    {
        if (!isShowingUI)
        {
            canvasGroup.DOFade(1.0f, 0.2f);
        }

        isShowingUI = true;

        CancelInvoke("DisableUI");

        Invoke("DisableUI", 5.0f);
    }

    void DisableUI()
    {
        canvasGroup.DOFade(0.0f, 0.5f);

        isShowingUI = false;
    }

    void UpdateAudioView()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(musicDiscTrans.DOLocalRotate(rotationValue, 0.6f, rotateMode).SetEase(Ease.Linear));

        this.mySequence = mySequence.Play().SetLoops(-1);
    }

    void StopMusic()
    {
        if (mySequence.IsPlaying())
        {
            mySequence.Kill(true);

            musicDiscTrans.DOKill(true);
        }
    }

    void ResetView()
    {
        StopMusic();

        StopAllCoroutines();

        currentPlayer.Stop();

        updatedRawImage.gameObject.SetActive(false);
    }

    #endregion
}
