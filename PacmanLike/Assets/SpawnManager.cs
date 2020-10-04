using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] private List<Vector2> spawnPoints;
    private List<bool> spawnedPoints;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ResetSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /// <summary>
    /// 他のキャラと重複しない様スポーン地点を転送
    /// </summary>
    /// <returns></returns>
    public Vector2 Spawn()
    {
        SelectedNumber:
        int num = Random.Range(0, spawnPoints.Count);

        if (spawnedPoints[num])
        {
            goto SelectedNumber;
        }
        // else
        spawnedPoints[num] = true;

        if (spawnedPoints.Distinct().Count() == 1)
        {
            ResetSpawn();
        }

        return spawnPoints[num];
    }


    public void ResetSpawn()
    {
        spawnedPoints = Enumerable.Repeat<bool>(false, spawnPoints.Count).ToList();
    }
}
