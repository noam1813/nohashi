using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Mizukusa : MonoBehaviour
{
    [SerializeField] private float AvailableTime;

    [SerializeField] private float NowUsingTime;

    [SerializeField] private PlayerManager player;

    private bool isUsing;
    
    /// <summary>
    /// 水草スポーン時、プレイヤーの情報を代入する
    /// </summary>
    /// <param name="_player"></param>
    public void Initialize(PlayerManager _player)
    {
        this.player = _player;
    }
    
    void FixedUpdate()
    {
        if (isUsing)
        {
            Using();   
        }
    }
    
    /// <summary>
    /// 水草の有効化
    /// プレイヤーの秘匿化
    /// </summary>
    public void StartUse()
    {
        if (isUsing)
        {
            return;
        }

        isUsing = true;
        player.SetHide(true,this);
    }

    /// <summary>
    /// 自分の水草が使われている間、常に実行するコマンド
    /// 指定時間たったら強制的に削除処理が始まる
    /// </summary>
    void Using()
    {
        NowUsingTime += Time.deltaTime;
        if (NowUsingTime >= AvailableTime)
        {
            endUse();
        }
    }
    
    /// <summary>
    /// プレイヤー秘匿の解除、水草の削除を行う
    /// </summary>
    public void endUse()
    {
        player.SetHide(false);
        Destroy(gameObject);
    }
}
