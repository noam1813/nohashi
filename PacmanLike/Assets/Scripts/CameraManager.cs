using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Stage;
    private Vector3 center;
    private Vector3 extent;
    private Vector2 NorthEast;
    private Vector2 SouthWest;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(5,5,-10);
        Vector3 center = Stage.GetComponent<TilemapCollider2D>().bounds.center;
        Vector3 extent = Stage.GetComponent<TilemapCollider2D>().bounds.extents;
        Debug.Log(center);
        Vector2 screenSize = GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width,Screen.height)); 
        NorthEast = new Vector2(center.x+extent.x-screenSize.x/2+2.3f,center.y+extent.y-screenSize.y/2+0.3f);
        SouthWest = new Vector2(center.x-extent.x+screenSize.x/2-2.3f,center.y-extent.y+screenSize.y/2-0.3f);
        Debug.Log(NorthEast);
        Debug.Log(SouthWest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 pos = new Vector3(Player.transform.position.x,Player.transform.position.y,-10);
        if (pos.x > NorthEast.x){
            pos.x = NorthEast.x;
        }
        if (pos.y > NorthEast.y){
            pos.y = NorthEast.y;
        }
        if (pos.x < SouthWest.x){
            pos.x = SouthWest.x;
        }
        if (pos.y < SouthWest.y){
            pos.y = SouthWest.y;
        }
        transform.position = pos;
    }
}
