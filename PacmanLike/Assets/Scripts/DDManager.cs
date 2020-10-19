using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDManager : MonoBehaviour
{

    public GameObject sceneTransitionManager;
    public GameObject sceneFadeManager;

    void Start()
    {
        DontDestroyOnLoad(sceneTransitionManager);
        DontDestroyOnLoad(sceneFadeManager);
    }

}