using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    //ポーズUIの画像
    private Image pauseImage;


    //ポーズ中の説明スプライト
    public List<Sprite> pictures;
    private int selectPicture = 0;

    // Start is called before the first frame update
    void Start()
    {
        pauseImage = GetComponent<Image>();
        ChangePicture(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePicture(-1);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePicture(1);
        }
    }


    private void ChangePicture(int mode)
    {
        if (selectPicture == 0 && mode == -1)
        {
            return;
        }

        if (selectPicture + mode == pictures.Count)
        {
            return;
        }
        
        selectPicture += mode;

        pauseImage.sprite = pictures[selectPicture];
    }

}
