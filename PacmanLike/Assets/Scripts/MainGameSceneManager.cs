using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneManager : MonoBehaviour
{
    GameObject ManageObject;
    SceneFadeManager fadeManager;
    // Start is called before the first frame update
    void Start()
    {
        //SceneFadeManagerがアタッチされているオブジェクトを取得
        ManageObject = GameObject.Find("ManageObject");
        //オブジェクトの中のSceneFadeManagerを取得
        fadeManager = ManageObject.GetComponent<SceneFadeManager>();
    }
}
