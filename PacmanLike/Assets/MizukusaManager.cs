using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MizukusaManager : MonoBehaviour
{
    public static MizukusaManager instance;
    
    [SerializeField] private PlayerManager player;
    
    [SerializeField] private GameObject MizukusaPrefab;
    [SerializeField] private Transform MizukusaRoot;
    
    //スポーンさせる水草の数
    [SerializeField] private int MizukusaAmount;
    
    //水草スポーン位置候補
    [SerializeField] private List<Vector2> MizukusaSpawns;
    
    //水草設置リスト、指定の位置に既に水草を置いているか保存する
    private List<bool> spawnedPoints;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        ResetSpawn();
        Spawn();
    }

    /// <summary>
    /// ステージ生成時の水草設置処理
    /// </summary>
    void Spawn()
    {
        for (var i = 1; i <= MizukusaAmount; i++)
        {
            Vector2 pos = SendSpawnPoint();

            var obj = Instantiate(MizukusaPrefab, MizukusaRoot);
            obj.transform.position = pos;
            var script = obj.GetComponent<Mizukusa>();
            script.Initialize(player);
        }
    }

    /// <summary>
    /// 他の水草とかぶらないように位置を指定する処理
    /// </summary>
    /// <returns></returns>
    public Vector2 SendSpawnPoint()
    {
        SelectedNumber:
        
        int num = Random.Range(0, MizukusaSpawns.Count);

        if (spawnedPoints[num])
        {
            goto SelectedNumber;
        }
        // else
        spawnedPoints[num] = true;

        return MizukusaSpawns[num];
        
    }

    /// <summary>
    /// 水草設置リストの初期化
    /// </summary>
    public void ResetSpawn()
    {
        spawnedPoints = Enumerable.Repeat<bool>(false, MizukusaSpawns.Count).ToList();
    }
}
