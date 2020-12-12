using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTenmetu : MonoBehaviour
{

    [Header("点滅間隔")] public float span = 0;
    private Text text;
    private Color color;
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
        color.r = 255f;
        color.g = 255f;
        color.b = 255f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * span;
        color.a = Mathf.Abs(Mathf.Sin(time));
        text.color = color;
    }
}
