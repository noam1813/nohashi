using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    //シーン開始からの経過時間
    private float TimeFromSceneStarted;

    //アニメーションが始まる時間
    [SerializeField] private float AnimationStartTime;

    //テキストの初期・終了座標

    

    // Start is called before the first frame update
    void Start()
    {
        clearTime = ResultDataManager.instance.data.clearTime;
        defeatedEnemiesAmount = ResultDataManager.instance.data.defeatedEnemiesAmount;

        TimeSpan clearTimeScore = new TimeSpan(0, 0, clearTime);

        cleatTimeScoreText.GetComponent<Text>().text = clearTimeScore.ToString(@"hh\:mm\:ss");

    }
}
