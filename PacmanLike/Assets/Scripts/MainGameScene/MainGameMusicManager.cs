using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMusicManager : MonoBehaviour
{
    public AudioSource noonBGM;
    public AudioSource nightBGM;

    public bool isFade = false;
    public double fadeOutSeconds = 1.0f;
    public double fadeInSeconds = 1.0f;
    public float noonVolume = 0.1f;
    public float nightVolume = 0.1f;
    bool isFadeOut = false;
    bool isFadeIn = false;
    double fadeDeltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        isFade = false;
        //noonBGM.Play();
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame()
    {
        if (!isFade && !isFadeOut && !isFadeIn)
        {
            isFade = true;
            isFadeIn = true;
            fadeDeltaTime = 0;
            noonBGM.Play();
            noonBGM.volume = 0;
        }

        while(isFadeIn)
        {
            fadeDeltaTime += Time.deltaTime;

            if (fadeDeltaTime <= fadeInSeconds && isFadeIn)
            {
                noonBGM.volume = (float)((fadeDeltaTime / fadeInSeconds) * noonVolume);
                yield return null;
            }
            else
            {
                noonBGM.volume = noonVolume;
                isFade = false;
                isFadeIn = false;
            }
        }
    }

    public IEnumerator ToNight()
    {
        if(!isFade && !isFadeOut && !isFadeIn)
        {
            isFade = true;
            isFadeOut = true;
            isFadeIn = false;
            fadeDeltaTime = 0;
        }

        while(isFadeOut)
        {
            fadeDeltaTime += Time.deltaTime;

            if (fadeDeltaTime <= fadeOutSeconds && isFadeOut)
            {
                noonBGM.volume = ((float)(1 - (fadeDeltaTime / fadeOutSeconds)) * noonVolume);
                yield return null;
            }
            else
            {
                isFadeOut = false;
                isFadeIn = true;
                fadeDeltaTime = 0;
                noonBGM.Stop();
                nightBGM.Play();
                nightBGM.volume = 0;
                yield return null;
            }
        }

        while(isFadeIn)
        {
            fadeDeltaTime += Time.deltaTime;

            if (fadeDeltaTime <= fadeInSeconds && isFadeIn)
            {
                nightBGM.volume = ((float)(fadeDeltaTime / fadeOutSeconds) * nightVolume);
                yield return null;
            }
            else
            {
                nightBGM.volume = nightVolume;
                isFade = false;
                isFadeIn = false;
            }
        }
        
    }

    public IEnumerator ToNoon()
    {
        if (!isFade && !isFadeOut && !isFadeIn)
        {
            isFade = true;
            isFadeOut = true;
            isFadeIn = false;
            fadeDeltaTime = 0;
        }

        while (isFadeOut)
        {
            fadeDeltaTime += Time.deltaTime;

            if (fadeDeltaTime <= fadeOutSeconds && isFadeOut)
            {
                nightBGM.volume = ((float)(1 - (fadeDeltaTime / fadeOutSeconds)) * nightVolume);
                yield return null;
            }
            else
            {
                isFadeOut = false;
                isFadeIn = true;
                fadeDeltaTime = 0;
                nightBGM.Stop();
                noonBGM.Play();
                noonBGM.volume = 0;
                yield return null;
            }
        }

        while (isFadeIn)
        {
            fadeDeltaTime += Time.deltaTime;

            if (fadeDeltaTime <= fadeInSeconds && isFadeIn)
            {
                noonBGM.volume = (float)((fadeDeltaTime / fadeInSeconds) * noonVolume);
                yield return null;
            }
            else
            {
                noonBGM.volume = noonVolume;
                isFade = false;
                isFadeIn = false;
            }
        }
            
    }
}
