using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTenmetu : MonoBehaviour
{

    [Header("点滅間隔")] public float span = 0;
    private Image image;
    private Color color;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * span;
        color.a = Mathf.Abs(Mathf.Sin(time));
        image.color = color;
    }
}
