using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Tilemaps;

public class TitleSceneManager : MonoBehaviour
{
    public GameObject ddManager;

    // Start is called before the first frame update
    void Start()
    {
        //SceneFadeManagerがアタッチされているオブジェクトを取得
        //ManageObject = GameObject.Find("SceneFader");
        //オブジェクトの中のSceneFadeManagerを取得
        //fadeManager = ManageObject.GetComponent<SceneFadeManager>();

        ddManager = GameObject.Find("DDManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            //SceneFadeManagerの中のフェードアウト開始関数を呼び出し
            //fadeManager.fadeOutStart(0, 0, 0, 0, "StageSelectScene");
            //fadeManager.fadeOutStart(0, 0, 0, 0, "MainGameScene");

            SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
             {
                 SceneManager.LoadScene("MainGameScene");
             });
            //SceneManager.LoadScene("MainGameScene");

        }
    }
}
