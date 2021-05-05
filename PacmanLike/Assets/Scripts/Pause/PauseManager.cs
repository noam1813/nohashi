using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PauseImageType
{
    HowToPlay,
    HowToControll,
    Story,
    Other,
}

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    //ポーズ中の説明スプライト
    public Sprite howToPlay;
    public Sprite howToControll;
    public Sprite story;
    public Sprite other;

    //現在座標
    [SerializeField] float x = 0;
    [SerializeField] float y = 0;

    //初期座標
    public float startX = 960;
    public float startY = 1000;

    //最終座標
    public float endX = 960;
    public float endY = 540;

    //時間
    [SerializeField] float time = 0.0f;
    public float easingTime = 1.5f;

    //フラグ
    private bool isEasing = false;
    private bool isPause = false;

    //ポーズした時に表示するUIのプレハブ
    public GameObject pauseUIPrefab;

    //ポーズUIのインスタンス
    private GameObject pauseUIInstance;

    private RectTransform instanceRectTransform;

    //ポーズUIの画像
    private Image pauseImage;

    public GameObject BlackFilter;

    //現在表示されている画像が何か
    private PauseImageType nowImage;

    // Start is called before the first frame update
    void Start()
    {
        isEasing = false;
        isPause = false;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        BlackFilter.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKey(KeyCode.Q) && isEasing == false)
        //{
        //    StartCoroutine("StartPause");
        //}

        PauseControll();

    }

    void PauseControll()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isPause == false)
        {
            isPause = true;
            ShowBlackFilter(true);
            pauseUIInstance = GameObject.Instantiate(pauseUIPrefab);
            instanceRectTransform = pauseUIInstance.GetComponent<RectTransform>();
            instanceRectTransform.Translate(0, 0, 0);
            instanceRectTransform.sizeDelta = new Vector2(1920, 1080);
            pauseUIInstance.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
            pauseImage = pauseUIInstance.transform.Find("Image").GetComponent<Image>();
            pauseImage.sprite = howToPlay;
            nowImage = PauseImageType.HowToPlay;
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isPause == true)
        {
            isPause = false;
            ShowBlackFilter(false);
            Destroy(pauseUIInstance);
            Time.timeScale = 1.0f;
        }
        else if (isPause == true && Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (nowImage)
            {
                case PauseImageType.HowToPlay:
                    pauseImage.sprite = howToControll;
                    nowImage = PauseImageType.HowToControll;
                    break;
                case PauseImageType.HowToControll:
                    pauseImage.sprite = other;
                    nowImage = PauseImageType.Other;
                    break;
                case PauseImageType.Other:
                    pauseImage.sprite = story;
                    nowImage = PauseImageType.Story;
                    break;

            }
        }
        else if (isPause == true && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (nowImage)
            {
                case PauseImageType.HowToControll:
                    pauseImage.sprite = howToPlay;
                    nowImage = PauseImageType.HowToPlay;
                    break;
                case PauseImageType.Story:
                    pauseImage.sprite = other;
                    nowImage = PauseImageType.Other;
                    break;
                case PauseImageType.Other:
                    pauseImage.sprite = howToControll;
                    nowImage = PauseImageType.HowToControll;
                    break;

            }
        }


    }


    public void ShowBlackFilter(bool mode)
    {
        BlackFilter.SetActive(mode);   
    }


//イージングつきでポーズUIを表示(未実装)
    //public IEnumerator StartPause()
    //{
    //    if(isEasing == false)
    //    {
    //        isEasing = true;

    //        pauseUIInstance = GameObject.Instantiate(pauseUIPrefab);
    //        instanceRectTransform = pauseUIInstance.GetComponent<RectTransform>();
    //        x = startX;
    //        y = startY;
    //        instanceRectTransform.Translate(x, y, 0);
    //        instanceRectTransform.sizeDelta = new Vector2(1920, 1080);

    //        pauseUIInstance.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);

    //        pauseImage = pauseUIInstance.transform.Find("Image").GetComponent<Image>();
    //        pauseImage.sprite = howToPlay;

    //        time = 0.0f;

    //        Time.timeScale = 0f;

    //        yield return null;
    //    }

    //    while (isEasing == true)
    //    {
    //        time += Time.unscaledDeltaTime;

    //        if (time < easingTime)
    //        {
    //            y = Easing.BackOut(time, easingTime, startY, endY, 1.7f);
    //            yield return null;
    //        }
    //        else
    //        {
    //            y = endY;
    //            isEasing = false;
    //        }

    //        instanceRectTransform.Translate(x, y, 0);
    //    }
    //}
}
