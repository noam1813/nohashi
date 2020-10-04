using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;
    [SerializeField] private List<FishMove> Fishes;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 取得した時間帯に合わせて、魚の動作モードを変更する
    /// </summary>
    /// <param name="data">時間帯</param>
    public void ChangeFishMode(TimeZoneData data)
    {
        foreach (var fish in Fishes)
        {
            switch (data)
            {
                case TimeZoneData.Noon:
                    fish.isNoon = true;
                    break;
                case TimeZoneData.Night:
                    fish.isNoon = false;
                    break;
            }
        }
    }
}
