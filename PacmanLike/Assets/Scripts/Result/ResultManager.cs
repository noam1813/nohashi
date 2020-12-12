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
    [SerializeField] private GameObject totalScoreText;

    [SerializeField] private GameObject enterImage;
    [SerializeField] private GameObject titleText;

    //ランクごとのテキストオブジェクト
    [SerializeField] private GameObject rankS;
    [SerializeField] private GameObject rankA;
    [SerializeField] private GameObject rankB;
    [SerializeField] private GameObject rankC;

    private float sin;
    private bool isSceneEnded;

    //シーン開始からの経過時間
    private float timeFromSceneStarted;

    //アニメーションが始まる時間
    [SerializeField] private float animationStartTime;

    //タイトルシーンに遷移できるようになるまでの時間
    [SerializeField] private float timeOfTranableTitleScene = 3.0f;

    //スコアの評価基準
    [SerializeField] private int[] listsOfTimeEvaluation = new int[3];
    [SerializeField] private int[] listsOfDefeatedEnemiesEvaluation = new int[2];
    [SerializeField] private int[] listsOfTotalScoreEvaluation = new int[3];


    //評価ごとに得られるスコア
    [SerializeField] private int[] listsOfTimeEvaluationScore = new int[4];
    [SerializeField] private int[] listsOfDefeatedEnemiesEvaluationScore = new int[3];

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

        Text text = JadgeRank(clearTime, defeatedEnemiesAmount);
        totalScoreText.GetComponent<Text>().text = text.text;
        totalScoreText.GetComponent<Text>().color = text.color;


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

    private Text JadgeRank(int clearTimeScore, int defeatedEnemiesAmountScore)
    {
        int totalScore = 0;

        if (clearTimeScore <= listsOfTimeEvaluation[0])
        {
            totalScore += listsOfTimeEvaluationScore[0];
        }
        else if (clearTimeScore <= listsOfTimeEvaluation[1])
        {
            totalScore += listsOfTimeEvaluationScore[1];
        }
        else if (clearTimeScore <= listsOfTimeEvaluation[2])
        {
            totalScore += listsOfTimeEvaluationScore[2];
        }
        else
        {
            totalScore += listsOfTimeEvaluationScore[3];
        }

        if (defeatedEnemiesAmountScore >= listsOfDefeatedEnemiesEvaluation[0])
        {
            totalScore += listsOfDefeatedEnemiesEvaluationScore[0];
        }
        else if (defeatedEnemiesAmountScore >= listsOfDefeatedEnemiesEvaluation[1])
        {
            totalScore += listsOfDefeatedEnemiesEvaluationScore[1];
        }
        else
        {
            totalScore += listsOfDefeatedEnemiesEvaluationScore[2];
        }

        if(totalScore >= listsOfTotalScoreEvaluation[0])
        {
            return rankS.GetComponent<Text>();
        }
        else if (totalScore >= listsOfTotalScoreEvaluation[1])
        {
            return rankA.GetComponent<Text>();
        }
        else if (totalScore >= listsOfTotalScoreEvaluation[2])
        {
            return rankB.GetComponent<Text>();
        }
        else
        {
            return rankC.GetComponent<Text>();
        }

    }
}
