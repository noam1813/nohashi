using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager instance;
    
    [SerializeField] private Tilemap myTileMap;
    [SerializeField] private Tilemap parentTileMap;

    [SerializeField] private TileBase wallTileBase;
    [SerializeField] private TileBase roadTileBase;
    [SerializeField] private TileBase throughedTileBase;

    [SerializeField] private List<TileBase> newMaps;
    [SerializeField] private List<TileBase> newMiniMaps;
    [SerializeField] private List<TileBase> newThroughMiniMaps;

    private TilemapCollider2D parentTileMapCollider;

    private Vector2 parentTileMapSize;

    private bool[,] throughedMap;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        parentTileMapCollider = parentTileMap.GetComponent<TilemapCollider2D>();
        parentTileMapSize = parentTileMapCollider.bounds.size;
        throughedMap = new bool[(int) parentTileMapSize.x,(int) parentTileMapSize.y];
        Debug.Log(parentTileMapSize);
        for (var i = 0; i < parentTileMapSize.y; i++)
        {
            for (var j = 0; j < parentTileMapSize.x; j++)
            {
                Vector3Int pos = new Vector3Int(j,i,0);
                pos-=new Vector3Int((int) (5),(int) (37),0);
                Debug.Log(pos);
                var data = parentTileMap.GetTile(pos);
                Debug.Log("NorHum TileMap");
                Debug.Log(newMaps.IndexOf(data));
                if (data != null)
                {
                    Debug.Log(data.name);
                    myTileMap.SetTile(pos,newMiniMaps[newMaps.IndexOf(data)]);
                }
                else
                {
                    Debug.Log("null");
                    myTileMap.SetTile(pos,roadTileBase);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetThroughedMap(Vector3 pos)
    {
        Vector3Int converedPos = myTileMap.GetComponent<GridLayout>().WorldToCell(pos);
        Debug.Log(converedPos);
        if (!throughedMap[(int) (5), (int) (37)])
        {
            throughedMap[(int) (converedPos.x+5), (int) (converedPos.y+37)] = true;
            var data = parentTileMap.GetTile(converedPos);
            myTileMap.SetTile(converedPos, newThroughMiniMaps[newMaps.IndexOf(data)]);
        }
    }
}
