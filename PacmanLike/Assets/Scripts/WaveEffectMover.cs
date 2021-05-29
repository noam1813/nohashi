using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffectMover : MonoBehaviour
{
    public float waveSpeed = 1;
    private Vector3 position;
    private float x;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * waveSpeed;
        x = Mathf.Sin(time);
        transform.position = position + new Vector3(x, 0, 0);
    }
}
