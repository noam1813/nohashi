using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainGameMusicManager : MonoBehaviour
{
    public static MainGameMusicManager instance;

    public AudioSource noonBGM;
    public AudioSource nightBGM;
    public AudioSource battleBGM;

    public bool isFade = false;
    public bool isBattle = false;
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
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }

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

    
    public IEnumerator SwitchBattle(bool mode)
    {
        isBattle = mode;

        if(isBattle)
        {
            noonBGM.volume = 0f;
            nightBGM.volume = 0f;
            yield return new WaitForSeconds(1f);
            battleBGM.Play();
        }
        else
        {
            battleBGM.Stop();
            Sequence seq = DOTween.Sequence();
            seq.Append(
                DOTween.To(
                    () => noonBGM.volume,
                    num => noonBGM.volume = num,
                    1f,
                    1f
                    )
                );
            seq.Join(
                DOTween.To(
                    () => nightBGM.volume,
                    num => nightBGM.volume = num,
                    1f,
                    1f
                    )
                );
        }
    }
}
