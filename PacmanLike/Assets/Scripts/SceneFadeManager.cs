using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager Instance;

    public enum FADE_TYPE
    {
        FADE_IN = 0,
        FADE_OUT = 1,
        FADE_OUTIN = 2,
    };

    private Texture2D fadeTexture = null;
    private float fadeAlpha = 0;

    void Start()
    {
        Instance = GetComponent<SceneFadeManager>();

        // 黒テクスチャの作成。
        this.fadeTexture = new Texture2D(1, 1);
        this.fadeTexture.SetPixel(0, 0, Color.black);
        this.fadeTexture.Apply();
    }

    public void OnGUI()
    {
        //透明度を更新して黒テクスチャを描画
        if (Event.current.type == EventType.Repaint)
        {
            GUI.color = new Color(0, 0, 0, this.fadeAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.fadeTexture);
        }
    }

    // フェード開始
    // _type フェードの種類
    // _interval フェードにかける時間
    // _interval _onActionフェード完了後に呼ぶ関数
    public void StartFade(FADE_TYPE type, float interval, Action onAction)
    {
        this.gameObject.SetActive(true);

        switch (type)
        {
            case FADE_TYPE.FADE_OUT:
                StartCoroutine(FadeOut(interval, onAction));
                break;
            case FADE_TYPE.FADE_IN:
                StartCoroutine(FadeIn(interval, onAction));
                break;
            case FADE_TYPE.FADE_OUTIN:
                StartCoroutine(FadeOutIn(interval, onAction));
                break;
        };
    }

    private IEnumerator Fade(FADE_TYPE type, float interval)
    {
        float time = 0;
        float min = (type == FADE_TYPE.FADE_OUT) ? 0.0f : 1.0f;
        float max = (type == FADE_TYPE.FADE_OUT) ? 1.0f : 0.0f;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(min, max, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeOut(float interval, Action onAction)
    {
        yield return StartCoroutine(Fade(FADE_TYPE.FADE_OUT, interval));

        onAction();

        this.gameObject.SetActive(false);
    }

    private IEnumerator FadeIn(float interval, Action onAction)
    {
        yield return StartCoroutine(Fade(FADE_TYPE.FADE_IN, interval));

        onAction();

        this.gameObject.SetActive(false);
    }

    private IEnumerator FadeOutIn(float interval, Action onAction)
    {
        yield return StartCoroutine(Fade(FADE_TYPE.FADE_OUT, interval));

        onAction();

        yield return StartCoroutine(Fade(FADE_TYPE.FADE_IN, interval));

        this.gameObject.SetActive(false);
    }


    ////フェードアウト処理の開始、完了を管理するフラグ
    //private bool isFadeOut = false;
    ////フェードイン処理の開始、完了を管理するフラグ
    //private bool isFadeIn = true;
    ////透明度が変わるスピード
    //float fadeSpeed = 0.75f;
    ////画面をフェードさせるための画像をパブリックで取得
    //public Image fadeImage;
    //float red, green, blue, alfa;
    ////シーン遷移のための型
    //string afterScene;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    DontDestroyOnLoad(this);
    //    SetRGBA(0, 0, 0, 1);
    //    //シーン遷移が完了した際にフェードインを開始するように設定
    //    SceneManager.sceneLoaded += fadeInStart;
    //}
    ////シーン遷移が完了した際にフェードインを開始するように設定
    //void fadeInStart(Scene scene, LoadSceneMode mode)
    //{
    //    isFadeIn = true;
    //}
    ///// <summary>
    ///// フェードアウト開始時の画像のRGBA値と次のシーン名を指定
    ///// </summary>
    ///// <param name="red">画像の赤成分</param>
    ///// <param name="green">画像の緑成分</param>
    ///// <param name="blue">画像の青成分</param>
    ///// <param name="alfa">画像の透明度</param>
    ///// <param name="nextScene">遷移先のシーン名</param>
    //public void fadeOutStart(int red, int green, int blue, int alfa, string nextScene)
    //{
    //    SetRGBA(red, green, blue, alfa);
    //    SetColor();
    //    isFadeOut = true;
    //    afterScene = nextScene;
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    if (isFadeIn == true)
    //    {
    //        //不透明度を徐々に下げる
    //        alfa -= fadeSpeed * Time.deltaTime;
    //        //変更した透明度を画像に反映させる関数を呼ぶ
    //        SetColor();
    //        if (alfa <= 0)
    //            isFadeIn = false;
    //    }
    //    if (isFadeOut == true)
    //    {
    //        //不透明度を徐々に上げる
    //        alfa += fadeSpeed * Time.deltaTime;
    //        //変更した透明度を画像に反映させる関数を呼ぶ
    //        SetColor();
    //        if (alfa >= 1)
    //        {
    //            isFadeOut = false;
    //            SceneManager.LoadScene(afterScene);
    //        }
    //    }
    //}
    ////画像に色を代入する関数
    //void SetColor()
    //{
    //    fadeImage.color = new Color(red, green, blue, alfa);
    //}
    ////色の値を設定するための関数
    //public void SetRGBA(int r, int g, int b, int a)
    //{
    //    red = r;
    //    green = g;
    //    blue = b;
    //    alfa = a;
    //}
}
