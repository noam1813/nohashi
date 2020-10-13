using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KaiManager : MonoBehaviour
{
    public int MaxKai;

    public int NowKai;
    
    public List<Vector2> SpawnPoint;

    public GameObject KaiPrefab;

    public List<bool> IsSpawned;
    // Start is called before the first frame update
    void Start()
    {
        ResetSpawn();
        
        for (int i = 1; i <= MaxKai; i++)
        {
            Vector2 Pos = SendSpawnPoint();
            var Obj = Instantiate(KaiPrefab);
            Obj.transform.position = Pos;
        }
    }

    public Vector2 SendSpawnPoint()
    {
        SelectedNumber:

        int num = Random.Range(0, SpawnPoint.Count);

        if (IsSpawned[num])
        {
            goto SelectedNumber;
        }

        // else
        IsSpawned[num] = true;

        return SpawnPoint[num];

    }
    public void ResetSpawn()
    {
        IsSpawned = Enumerable.Repeat<bool>(false, SpawnPoint.Count).ToList();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
