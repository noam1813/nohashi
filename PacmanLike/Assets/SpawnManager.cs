using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO: SpawnManagerとMizukusaManagerは共通点が多い
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] private List<Vector2> spawnPoints;
    [SerializeField] private List<int> spawnAmount;
    [SerializeField] private int spawnCancelRange;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            DestroyImmediate(gameObject);
        }
        
    }

    /// <summary>
    /// 他のキャラと重複しない様スポーン地点を転送
    /// </summary>
    /// <returns></returns>
    public Vector2 Spawn()
    {
        SelectedNumber:
        
        int num = Random.Range(0, spawnPoints.Count);

        if (IsNearToPlayerPosition(num))
        {
            goto SelectedNumber;
        }
        
        return spawnPoints[num];
    }


    public int GetSpawnAmount(int date)
    {
        return spawnAmount[date-1];
    }


    public bool IsNearToPlayerPosition(int number)
    {
        Vector2 spawnPos = spawnPoints[number];
        Vector2 playerPos = PlayerManager.instance.SetPlayerPosition();

        if (Vector2.Distance(spawnPos, playerPos) <= spawnCancelRange)
        {
            return true;
        }

        return false;
    }

}
