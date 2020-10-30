using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
using UniRx;


[Serializable]
public class FazeData
{
    
}

[Serializable]
public enum TimeZoneData
{
    Noon,Night    
}
public class TimeManager : MonoBehaviour
{
    //「TimeManager.instance」で何処からでもこの情報にアクセスできるよ
    // Ex→TimeManager.instance.timeZone
    // TimeManager.instance.ChangeTimeZone()
    public static TimeManager instance;

    //これで音楽を変えれるよ
    private MainGameMusicManager musicManager;
    
    //現在の日付。１日目など
    [SerializeField] private int nowDay = 1;

    //昼か夜かを示す
    //[SerializeField] private TimeZoneData timeZone;
    [SerializeField] public TimeZoneData timeZone { get; private set; }

    //現在の時刻
    [SerializeField] private float nowTime;
    
    //合計経過時間
    private int totalTime;
    
    //最大時刻
    [SerializeField] private float maxTime;

    [SerializeField] private Image TimeZoneIcon;
    
    [SerializeField] private Image TimeZoneProgressCircle;

    [SerializeField] private Text nowDayText;

    private IDisposable countTimeObservable;




    void Start()
    {
        // instanceは俺だ
        instance = this;

        //見つけてくる
        musicManager = GameObject.Find("MusicManager").GetComponent<MainGameMusicManager>();

        countTimeObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            totalTime++;
        }).AddTo(this);
    }

    private void FixedUpdate()
    {
        UpdateTime();
    }
    
    
    /// <summary>
    /// 時刻の更新を行う
    /// </summary>
    void UpdateTime()
    {
        nowTime += Time.deltaTime;

        //現在時刻が最大時刻以上→昼と夜を入れ替える
        if (nowTime >= maxTime)
        {
            ChangeTimeZone();
        }
        
        TimeZoneProgressCircle.fillAmount = nowTime / maxTime;

    }

    
    /// <summary>
    /// 時間帯の更新を行う
    /// </summary>
    void ChangeTimeZone()
    {
        nowTime -= maxTime;
        switch (timeZone)
        {
            case TimeZoneData.Noon:
                timeZone = TimeZoneData.Night;
                TimeZoneIcon.sprite = Resources.Load<Sprite>("TimeZoneIcon/Night");
                StageEffectManager.instance.SetShadow(true);
                musicManager.StartCoroutine("ToNight");
                break;
            
            case TimeZoneData.Night:
                // 夜から昼に変えるとき、日付の加算を行う
                nowDay++;
                timeZone = TimeZoneData.Noon;
                TimeZoneIcon.sprite = Resources.Load<Sprite>("TimeZoneIcon/Noon");
                nowDayText.text = nowDay + "Day";
                StageEffectManager.instance.SetShadow(false);
                musicManager.StartCoroutine("ToNoon");
                break;
            
            default:
                Debug.LogError("時刻が正しく設定されていません");
                break;
        }

        FishManager.instance.Spawn(3);
        FishManager.instance.ChangeFishMode(timeZone);
    }


    public int GetTotalTime()
    {
        return totalTime;
    }
}
