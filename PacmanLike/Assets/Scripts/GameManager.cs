using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ManageObject;
    public SceneFadeManager fadeManager;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
        //SceneFadeManagerがアタッチされているオブジェクトを取得
        ManageObject = GameObject.Find("SceneFader");
        //オブジェクトの中のSceneFadeManagerを取得
        fadeManager = ManageObject.GetComponent<SceneFadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            //SceneFadeManagerの中のフェードアウト開始関数を呼び出し
            //fadeManager.fadeOutStart(0, 0, 0, 0, "StageSelectScene");
            //fadeManager.fadeOutStart(0, 0, 0, 0, "MainGameScene");
        }
    }

    public void ChangeScene(string sceneName)
    {
        //fadeManager.fadeOutStart(0, 0, 0, 0, sceneName);
    }
}
