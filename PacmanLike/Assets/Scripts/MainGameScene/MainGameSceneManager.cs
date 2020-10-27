using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneManager : MonoBehaviour
{
    public GameObject ddManager;

    // Start is called before the first frame update
    void Start()
    {
        ddManager = GameObject.Find("DDManager");
    }

    void Update()
    {
        
    }
}
