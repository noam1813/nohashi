using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDataManager : MonoBehaviour
{
    public static ResultDataManager instance;
    public ResultData data;   
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    /// <summary>
    /// ゲームオーバー時など、数値を保存したいときにこのコマンドを外部から実行してください。
    /// 逆に取得したいときは
    /// 「ResultDataManager.instance.~~」で取得できるはず
    /// </summary>
    public void SetResultData()
    {
        SetTotalTime();
        SetDefeatedEnemiesAmount();
        //タイルパレットはまだできてない
        SetPassedTilePercent();
    }

    void SetTotalTime()
    {
        TimeManager.instance.GetTotalTime();
    }

    void SetDefeatedEnemiesAmount()
    {
        data.defeatedEnemiesAmount =  FishManager.instance.GetDefeatedFishes();
    }

    void SetPassedTilePercent()
    {
        
    }
}


[System.Serializable]
public class ResultData
{
    //クリアタイム、秒単位で整数で代入
    public int clearTime;
    //敵を倒した数
    public int defeatedEnemiesAmount;
    //タイルマップ塗りつぶし率
    public float passedTilePercent;
}
