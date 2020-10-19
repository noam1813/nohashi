using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneSelect : MonoBehaviour
{
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToEnemySampleScene()
    {
        string sceneName = "EnemySampleScene";
        gameManager.ChangeScene(sceneName);
    }


    public void ChangeToMainGameScene()
    {
        string sceneName = "MainGameScene";
        gameManager.ChangeScene(sceneName);
    }
}
