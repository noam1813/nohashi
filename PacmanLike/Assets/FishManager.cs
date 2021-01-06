using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager instance;
    [SerializeField] private List<FishMove> Fishes;

    [SerializeField] private List<Vector2> spawnPoints;

    [SerializeField] private GameObject FishPrefab;
    
    [SerializeField] private Grid grid;

    [SerializeField] private GameObject player;

    [SerializeField] private int defeatedFishes;

    
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

    private void Start()
    {
        Spawn(3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isChaseMode = IsChasingFish();
        if(isChaseMode != MainGameMusicManager.instance.isBattle)
        {
            StartCoroutine(MainGameMusicManager.instance.SwitchBattle(isChaseMode));
        }
    }

    /// <summary>
    /// 取得した時間帯に合わせて、魚の動作モードを変更する
    /// </summary>
    /// <param name="data">時間帯</param>
    public void ChangeFishMode(TimeZoneData data)
    {
        foreach (var fish in Fishes)
        {
            switch (data)
            {
                case TimeZoneData.Noon:
                    fish.isNoon = true;
                    break;
                case TimeZoneData.Night:
                    fish.isNoon = false;
                    break;
            }
        }
    }


    public void Spawn(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            Vector2 point = SpawnManager.instance.Spawn();

            var obj = Instantiate(FishPrefab);
            obj.transform.position = point;
            FishMove script = obj.GetComponent<FishMove>();
            script.grid = grid;
            script.playerObject = player;
            Fishes.Add(script);
        }
        
    }

    public void RemoveFish(FishMove target)
    {
        defeatedFishes++;
        Fishes.Remove(target);
    }

    public int GetDefeatedFishes()
    {
        return defeatedFishes;
    }


    public bool IsChasingFish()
    {
        bool answer = !Fishes.All(x => x.isChasing == false);
        return answer;
    }
}
