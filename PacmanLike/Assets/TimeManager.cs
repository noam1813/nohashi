using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [SerializeField] private int day;

    [SerializeField] private TimeZoneData timeZone;

    [SerializeField] private int time;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
