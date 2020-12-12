using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ResultManager : MonoBehaviour
{
    private int clearTime;
    private int defeatedEnemiesAmount;

    //リザルト画面でアニメーションを行うテキストオブジェクト
    [SerializeField] private GameObject cleatTimeText;
    [SerializeField] private GameObject cleatTimeScoreText;
    [SerializeField] private GameObject defeatedEnemiesAmountText;
    [SerializeField] private GameObject defeatedEnemiesAmountScoreText;

    [SerializeField] private GameObject enterImage;
    [SerializeField] private GameObject titleText;

    
    private float sin;
    private bool isSceneEnded;

    //シーン開始からの経過時間
    private float timeFromSceneStarted;

    //アニメーションが始まる時間
    [SerializeField] private float animationStartTime;

    //タイトルシーンに遷移できるようになるまでの時間
    [SerializeField] private float timeOfTranableTitleScene = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        clearTime = ResultDataManager.instance.data.clearTime;
        defeatedEnemiesAmount = ResultDataManager.instance.data.defeatedEnemiesAmount;

        TimeSpan clearTimeScore = new TimeSpan(0, 0, clearTime);

        cleatTimeScoreText.GetComponent<Text>().text = clearTimeScore.ToString(@"hh\:mm\:ss");
        defeatedEnemiesAmountScoreText.GetComponent<Text>().text = defeatedEnemiesAmount.ToString();

        timeFromSceneStarted = 0;

        isSceneEnded = false;

        enterImage.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        titleText.GetComponent<Text>().color = new Color(255, 255, 255, 0);
    }

    private void Update()
    {
        if(timeFromSceneStarted > timeOfTranableTitleScene)
        {
            
            sin = Mathf.Abs(Mathf.Sin(Time.time));

            enterImage.GetComponent<Image>().color = new Color(255, 255, 255, sin);
            titleText.GetComponent<Text>().color = new Color(255, 255, 255, sin);

            if(!isSceneEnded && Input.GetKeyDown(KeyCode.Return))
            {
                isSceneEnded = true;

                SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
                {
                    SceneManager.LoadScene("TitleScene");
                });
            }
        }

        timeFromSceneStarted += Time.deltaTime;
    }
}
