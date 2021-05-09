using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum SceneTitle
    {
        Title,
        ChapterSelect,
        Stage1_1,
        Stage1_2,
        Stage1_3,
        Clear,
        GameOver
    }

    void Start()
    {
        Instance = GetComponent<SceneTransitionManager>();

        Debug.Log("Set Instance");
    }

    void Update()
    {

    }

    public IEnumerator SceneTransitionToPlay(string sceneTitle, float waitaTime)
    {
        Debug.Log("Pushed R Key 2");
        yield return new WaitForSeconds(waitaTime);
        Debug.Log("Pushed R Key 3");
        SceneManager.LoadScene(sceneTitle);
        Debug.Log("Pushed R Key 4");

    }
    public void OnButtonClick()
    {
        StartCoroutine(SceneTransitionManager.Instance.SceneTransitionToPlay("ChapterSelect", 0));
    }
}