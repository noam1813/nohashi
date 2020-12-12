using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.Tilemaps;
using UnityEngine.UI;
//UI使うときに必要
using UnityEngine.EventSystems;
//選択の解除のため
public class TitleSceneManager : MonoBehaviour
{
    public GameObject ddManager;
    public GameObject buttonSummary;
    public GameObject checkStart;
    public GameObject checkExit;
    public GameObject pressEnter;
    Button button;

    private AudioSource audioSource;
    public AudioClip[] audioClip = new AudioClip[10];

    // Start is called before the first frame update
    void Start()
    {
        //SceneFadeManagerがアタッチされているオブジェクトを取得
        //ManageObject = GameObject.Find("SceneFader");
        //オブジェクトの中のSceneFadeManagerを取得
        //fadeManager = ManageObject.GetComponent<SceneFadeManager>();
        ddManager = GameObject.Find("DDManager");
        buttonSummary = GameObject.Find("ButtonSummary");
        pressEnter = GameObject.Find("PressStartTextEffect");
        checkStart = GameObject.Find("CheckStartPanel");
        checkExit = GameObject.Find("CheckExitPanel");
        buttonSummary.SetActive(false);
        checkStart.SetActive(false);
        checkExit.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return) && pressEnter.activeSelf)
        {
            BackToMenu();
        }
    }


    //　STARTを押したら実行する
    public void CheckStart() {
        EventSystem.current.SetSelectedGameObject(null);//選択の解除
        buttonSummary.SetActive(false);
        checkStart.SetActive(true);
        button = GameObject.Find("Canvas/CheckStartPanel/CheckStartButtonN").GetComponent<Button>();
        SEPlay(1);
        //ボタンが選択された状態になる
        button.Select();
    }
    // START -> はい を押したら実行する
    public void StartGame() {
        //SceneFadeManagerの中のフェードアウト開始関数を呼び出し
        //fadeManager.fadeOutStart(0, 0, 0, 0, "StageSelectScene");
        //fadeManager.fadeOutStart(0, 0, 0, 0, "MainGameScene");

        SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
        {
            SceneManager.LoadScene("EnemySampleScene");
        });
        //SceneManager.LoadScene("MainGameScene");
    }

    //　SCORE BOARDを押したら実行する
        public void ShowScoreBoard() {

        }

    //　EXITを押したら実行する
    public void CheckExit() {
        EventSystem.current.SetSelectedGameObject(null);//選択の解除
        buttonSummary.SetActive(false);
        checkExit.SetActive(true);
        button = GameObject.Find("Canvas/CheckExitPanel/CheckExitButtonN").GetComponent<Button>();
        //ボタンが選択された状態になる
        button.Select();
    }
    //　EXIT -> はい を押したら実行する
    public void EndGame() {
	    #if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
	    #elif UNITY_WEBPLAYER
		    Application.OpenURL("http://www.yahoo.co.jp/");
	    #else
		    Application.Quit();
	    #endif
    }

    // START or Exit -> いいえ を押したら実行する
    public void BackToMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);//選択の解除
        pressEnter.SetActive(false);
        buttonSummary.SetActive(true);
        checkStart.SetActive(false);
        checkExit.SetActive(false);
        StartCoroutine(Appearance());
        button = GameObject.Find("Canvas/ButtonSummary/StartButton").GetComponent<Button>();
        SEPlay(2);
        //ボタンが選択された状態になる
        button.Select();
    }
    private IEnumerator Appearance()
   {
        for (int i = 40; i > 0; i--) {
            float p = i * 50f / 60;
            RectTransform rt = buttonSummary.GetComponent<RectTransform>();
            //Vector3 nowPos = buttonSummary.GetComponent<RectTransform>().position;
            //Vector3 nowPos = GameObject.Find("Canvas/ButtonSummary").transform.position;
            if(i == 40){
                rt.localPosition = new Vector3 (-1300f, 120f, rt.localPosition.z);
                //nowPos.x = -1000f;
            //   GameObject.Find("Canvas/ButtonSummary").transform.position = new Vector3 (-10f, nowPos.y, nowPos.z);
            }else{
                rt.localPosition = new Vector3 (rt.localPosition.x + p, rt.localPosition.y, rt.localPosition.z);
                //nowPos.x += p;
            //  GameObject.Find("Canvas/ButtonSummary").transform.position = new Vector3 (nowPos.x + p, nowPos.y, nowPos.z);
            }
            yield return null;
         }
     }

    public void SEPlay(int num)
    {
        audioSource.clip = audioClip[num];
        audioSource.Play();
    }
}
