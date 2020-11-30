using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverAnimation : MonoBehaviour
{

    [SerializeField] private GameObject gameOverTextImage;
    [SerializeField] private GameObject enterImage;
    [SerializeField] private GameObject titleText;

    [SerializeField] private AudioSource gameOverSE;

    //アニメーションの初期・終了座標
    [SerializeField] private Vector3 firstPosOfgameOverTextImage;
    [SerializeField] private Vector3 endPosOfgameOverTextImage;

    //アニメーションの時間制御
    //シーンが始まってからの時間
    private float timeFromSceneStart;
    //アニメーションが始まる時間
    [SerializeField] private float timeOfStartAnimation;
    //アニメーションの継続時間
    [SerializeField] private float animationDuration;

    //アニメーション管理フラグ
    private bool isAnimationStarted;
    private bool isSceneEnded;

    //sin関数格納変数
    private float sin;

    // Start is called before the first frame update
    void Start()
    {
        gameOverTextImage.GetComponent<RectTransform>().localPosition = firstPosOfgameOverTextImage;

        timeFromSceneStart = 0;
        isAnimationStarted = false;
        isSceneEnded = false;

        enterImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        titleText.GetComponent<Text>().color = new Color(255, 255, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeFromSceneStart >= timeOfStartAnimation + animationDuration && !isSceneEnded && Input.GetKeyDown(KeyCode.Return))
        {
            isSceneEnded = true;

            SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
            {
                SceneManager.LoadScene("TitleScene");
            });
        }
    }

    private void FixedUpdate()
    {
        timeFromSceneStart += Time.deltaTime;

        if (timeFromSceneStart >= timeOfStartAnimation && !isAnimationStarted)
        {
            isAnimationStarted = true;
            gameOverTextImage.GetComponent<RectTransform>().DOLocalMove(endPosOfgameOverTextImage, animationDuration).SetEase(Ease.OutBounce);
            gameOverSE.Play();
        }

        if(timeFromSceneStart >= timeOfStartAnimation + animationDuration)
        {
            sin = Mathf.Abs(Mathf.Sin(Time.time));

            enterImage.GetComponent<Image>().color = new Color(255, 255, 255, sin);
            titleText.GetComponent<Text>().color = new Color(255, 255, 255, sin);
        }
    }
}
