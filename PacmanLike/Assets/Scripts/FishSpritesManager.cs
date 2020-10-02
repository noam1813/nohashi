using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpritesManager : MonoBehaviour
{
    public Sprite noonSprite;
    public Sprite nightSprite;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSpriteToNoon()
    {
        sr.sprite = noonSprite;
    }

    public void SetSpriteToNight()
    {
        sr.sprite = nightSprite;
    }
}
