using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private List<AudioSource> audioSources;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            DestroyImmediate(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        Debug.Log("AudioPlay");
        AudioSource source = null;
        source = audioSources.First(x => x.gameObject.name == name);
        if (source != null)
        {
            source.Play();
        }
    }
}
