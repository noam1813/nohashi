using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public Grid grid;
    public Tilemap stageTilemap;

    //private FishSpritesManager FS;

    //昼かどうか
    public bool isNoon = true;

    enum Direction
    {
        Null = -1, Right = 0, Up = 1, Left = 2, Down = 3
    }

    //プレイヤー
    public GameObject playerObject;

    private Direction direction;
    private SpriteRenderer sr;
    
    //アニメーター
    private Animator animator;
    //右上左下のタイルマップの位置(補正)
    private Vector3[] AroundVector = new Vector3[4];

    //魚が実際に動く速度
    [SerializeField] float speed;
    //魚の通常速度
    public float normalSpeed = 0.8f;
    //魚の逃げる速度
    public float runAwaySpeed = 1.3f;
    //魚の発見時の追跡速度
    public float chaseSpeed = 1.0f;

    //魚の座標
    public Vector3 nowPosition;
    public Vector3 nextPosition;

    //昼の魚の可視距離
    public float visibleDistanceInNoon = 3.0f;
    //夜の魚の可視距離
    public float visibleDistanceInNight = 3.0f;
    //夜の魚の感知範囲
    public float sensibleDistanceInNight = 5.0f;

    //魚が逃走状態か
    bool isRunAwayState;
    //魚の逃走状態が継続する時間
    public float runAwayTime = 5.0f;
    //逃走タイマー
    public float runAwayTimer = 0.0f;

    //プレイヤーを感知しているかどうか
    bool isSensing;
    //プレイヤーを追跡しているかどうか
    bool isChasing;
    //魚の発見状態が継続する時間
    public float chaseTime = 3.0f;
    //追跡タイマー
    public float chaseTimer = 0.0f;
    //追跡クールタイム状態かどうか
    bool isChasingCoolTime;
    //追跡クールタイムが計測する時間
    public float chasingCoolTime = 3.0f;
    //追跡クールタイマー
    public float chasingCoolTimer = 0.0f;

    void Start()
    {
        stageTilemap = grid.transform.Find("Stage").GetComponent<Tilemap>();
        direction = Direction.Left;
        //FS = this.transform.Find("Sprites").GetComponent<FishSpritesManager>();
        animator = GetComponent<Animator>();
        SetAnimation();
        //if(isNoon)
        //{
        //    FS.SetSpriteToNoon();
        //}
        //else
        //{
        //    FS.SetSpriteToNight();
        //}

        sr = this.transform.Find("Sprites").GetComponent<SpriteRenderer>();
        AroundVector[(int)Direction.Right] = new Vector3(1, 0, 0);
        AroundVector[(int)Direction.Up] = new Vector3(0, 1, 0);
        AroundVector[(int)Direction.Left] = new Vector3(-1, 0, 0);
        AroundVector[(int)Direction.Down] = new Vector3(0, -1, 0);

        Vector3 myPos = this.transform.position;
        Vector3 worldPos = grid.WorldToCell(myPos);
        nowPosition = worldPos;
        //Debug.Log("初期myPos:" + myPos.ToString("F3"));
        //Debug.Log("worldPos:" + worldPos.ToString("F3"));
        //グリッド補正が入るかも
        myPos.x = worldPos.x;
        myPos.y = worldPos.y;
        this.transform.position = myPos;

        speed = normalSpeed;
        isRunAwayState = false;
        //Debug.Log("訂正後myPos:" + myPos.ToString("F3"));

        DecideNextPosition();

        isSensing = false;
        isChasing = false;
        
        sensibleDistanceInNight = Mathf.Pow(sensibleDistanceInNight, 2);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ChangeTime();
    }

    void FixedUpdate()
    {
        if (isNoon)
        {
            FishMoveInNoon();
            RunAwayFromPlayer();
        }
        else
        {
            FishMoveInNight();
            if(!isChasing &&!isChasingCoolTime)
                ChaseToPlayer();
        }

    }

    void FishMoveInNoon()
    {
        if (isRunAwayState == true && runAwayTimer < runAwayTime)
        {
            runAwayTimer += Time.deltaTime;
        }
        else if (isRunAwayState == true && runAwayTimer >= runAwayTime)
        {
            runAwayTimer = 0.0f;
            isRunAwayState = false;
            speed = normalSpeed;
        }

        Vector3 pos = transform.position;
        switch (direction)
        {

            case Direction.Right:

                pos.x += speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.x >= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    DecideNextPosition();
                }
                break;

            case Direction.Up:

                pos.y += speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.y >= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    DecideNextPosition();
                }
                break;

            case Direction.Left:

                pos.x -= speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.x <= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    DecideNextPosition();
                }
                break;

            case Direction.Down:

                pos.y -= speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.y <= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    DecideNextPosition();
                }
                break;

            case Direction.Null:
                DecideNextPosition();
                break;

            default:
                break;

        }

    }

    void FishMoveInNight()
    {
        if (isChasing && chaseTimer < chaseTime)
        {
            chaseTimer += Time.deltaTime;
        }
        else if (isChasing && chaseTimer >= chaseTime)
        {
            chaseTimer = 0.0f;
            isChasing = false;
            speed = normalSpeed;
            //追跡クールタイムに入る
            isChasingCoolTime = true;
            chasingCoolTimer = 0.0f;
        }
        else if(isChasingCoolTime && chasingCoolTimer < chasingCoolTime)
        {
            chasingCoolTimer += Time.deltaTime;
        }
        else if(isChasingCoolTime && chasingCoolTimer >= chasingCoolTime)
        {
            chasingCoolTimer = 0.0f;
            isChasingCoolTime = false;
        }

        isSensing = SensingPlayer();

        
        Vector3 pos = transform.position;
        switch (direction)
        {

            case Direction.Right:

                pos.x += speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.x >= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    nowPosition = nextPosition;
                    this.transform.position = pos;

                    if (!isSensing)
                        DecideNextPosition();
                    else
                        ChasingPlayerPosition();
                }
                break;

            case Direction.Up:

                pos.y += speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.y >= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    if (!isSensing)
                        DecideNextPosition();
                    else
                        ChasingPlayerPosition();
                }
                break;

            case Direction.Left:

                pos.x -= speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.x <= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    if (!isSensing)
                        DecideNextPosition();
                    else
                        ChasingPlayerPosition();
                }
                break;

            case Direction.Down:

                pos.y -= speed * Time.deltaTime;
                this.transform.position = pos;
                if (this.transform.position.y <= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    nowPosition = nextPosition;
                    this.transform.position = pos;
                    if (!isSensing)
                        DecideNextPosition();
                    else
                        ChasingPlayerPosition();
                }
                break;

            case Direction.Null:
                DecideNextPosition();
                break;

            default:
                break;

        }
        
        


        
    }

    //移動方向を決める
    void DecideNextPosition()
    {
        bool[] blockFlag = CheckAroundBlockToMove(this.transform.position);
        List<Direction> canMoveDir = new List<Direction>();

        if (!blockFlag[0] && direction != Direction.Left)
            canMoveDir.Add(Direction.Right);
        if (!blockFlag[1] && direction != Direction.Down)
            canMoveDir.Add(Direction.Up);
        if (!blockFlag[2] && direction != Direction.Right)
            canMoveDir.Add(Direction.Left);
        if (!blockFlag[3] && direction != Direction.Up)
            canMoveDir.Add(Direction.Down);

        if (canMoveDir.Count >= 1)
        {
            int rand = Random.Range(0, canMoveDir.Count);
            direction = canMoveDir[rand];
            SetAnimation();
        }

        Vector3 pos = new Vector3();
        switch (direction)
        {
            case Direction.Right:
                sr.flipX = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                pos = transform.position + AroundVector[(int)Direction.Right];
                nextPosition = pos;
                break;

            case Direction.Up:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                pos = transform.position + AroundVector[(int)Direction.Up];
                nextPosition = pos;
                break;

            case Direction.Left:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                pos = transform.position + AroundVector[(int)Direction.Left];
                nextPosition = pos;
                break;

            case Direction.Down:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                pos = transform.position + AroundVector[(int)Direction.Down];
                nextPosition = pos;
                break;

            case Direction.Null:
                break;

            default:
                break;

        }

    }

    //周囲が移動可能か調べる
    bool[] CheckAroundBlockToMove(Vector3 myPos)
    {
        Vector3 nowPosition = grid.WorldToCell(myPos);

        //右上左下の順
        bool[] AroundBlock = new bool[4];

        foreach (var v in AroundVector.Select((value, index) => new { value, index }))
        {
            Vector3Int pos = grid.WorldToCell(this.transform.position + v.value);
            if (stageTilemap.GetTile(pos) != null)
            {
                AroundBlock[v.index] = true;
            }
            else
            {
                AroundBlock[v.index] = false;
            }
        }

        return AroundBlock;
    }

    //プレイヤーを見つける関数
    bool DiscoverPlayer(float dist)
    {
        Vector2 rayDirection;
        switch (direction)
        {
            case Direction.Right:
                rayDirection = Vector2.right;
                break;

            case Direction.Up:
                rayDirection = Vector2.up;
                break;

            case Direction.Left:
                rayDirection = Vector2.left;
                break;

            case Direction.Down:
                rayDirection = Vector2.down;
                break;

            default:
                rayDirection = Vector2.zero;
                break;
        }
        
        SetAnimation();

        Ray2D ray = new Ray2D(transform.position, rayDirection);
        Debug.DrawRay(ray.origin + rayDirection, (Vector3)ray.direction * dist, Color.green, 0.1f, false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)rayDirection, rayDirection, dist);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("プレイヤー発見！");
                return true;
            }
        }

        return false;
    }

    //プレイヤーを発見したら逃げる(昼)
    void RunAwayFromPlayer()
    {
        if (DiscoverPlayer(visibleDistanceInNoon))
        {
            isRunAwayState = true;
            runAwayTimer = 0.0f;
            nextPosition = nowPosition;
            speed = runAwaySpeed;

            switch (direction)
            {
                case Direction.Right:
                    direction = Direction.Left;
                    break;

                case Direction.Up:
                    direction = Direction.Down;
                    break;

                case Direction.Left:
                    direction = Direction.Right;
                    break;

                case Direction.Down:
                    direction = Direction.Up;
                    break;

                default:
                    direction = Direction.Null;
                    break;
            }
            
            SetAnimation();
        }
    }

    //プレイヤーを発見したら追跡する(夜)
    void ChaseToPlayer()
    {
        if (DiscoverPlayer(visibleDistanceInNight))
        {
            isChasing = true;
            chaseTimer = 0.0f;
            speed = chaseSpeed;

        }
    }

    //周囲のブロックをお知らせ(デバッグ用)
    void AroundInfo()
    {
        bool[] blockflag = CheckAroundBlockToMove(this.transform.position);

        print("右のブロックフラグは" + blockflag[(int)Direction.Right] + "です");
        print("上のブロックフラグは" + blockflag[(int)Direction.Up] + "です");
        print("左のブロックフラグは" + blockflag[(int)Direction.Left] + "です");
        print("下のブロックフラグは" + blockflag[(int)Direction.Down] + "です");
    }

    //魚がプレイヤーを感知しているかどうか
    bool SensingPlayer()
    {
        Vector3 player = playerObject.transform.position;
        
        Vector3 fish = this.gameObject.transform.position;

        float distance = Mathf.Pow(player.x - fish.x, 2) + Mathf.Pow(player.y - fish.y, 2);

        if (distance < sensibleDistanceInNight)
            return true;

        return false;
    }

    //プレイヤーの座標を追跡して移動する
    void ChasingPlayerPosition()
    {
        bool[] blockFlag = CheckAroundBlockToMove(this.transform.position);

        Vector3 nowPosition = grid.WorldToCell(this.transform.position);

        //右上左下の順
        float[] AroundBlockDistanceFlomPlayer = new float[4];

        foreach (var v in AroundVector.Select((value, index) => new { value, index }))
        {
            Vector3Int position = grid.WorldToCell(this.transform.position + v.value);
            
            if(!blockFlag[v.index] && (Direction)v.index != ReverseDirection(direction))
            {
                Vector3 player = playerObject.transform.position;

                AroundBlockDistanceFlomPlayer[v.index] = Mathf.Pow(player.x - position.x, 2) + Mathf.Pow(player.y - position.y, 2);
            }
            else
            {
                AroundBlockDistanceFlomPlayer[v.index] = -1;
            }
        }

        for(int i = 0; i < 4; i++)
        {
            bool flag = true;
            for(int j = 0; j < 4; j++)
            {
                if (AroundBlockDistanceFlomPlayer[i] != -1)
                {
                    if(i != j)
                    {
                        if (AroundBlockDistanceFlomPlayer[i] > AroundBlockDistanceFlomPlayer[j] && AroundBlockDistanceFlomPlayer[j] != -1)
                        {
                            flag = false;
                            continue;
                        }
                    }
                }
                else
                {
                    flag = false;
                    continue;
                }

                if (flag && j == 3)
                {
                    direction = (Direction)i;
                    SetAnimation();
                    break;
                }
                    
            }
            if (flag)
                break;
            
        }

        Vector3 pos = new Vector3();
        switch (direction)
        {
            case Direction.Right:
                sr.flipX = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                pos = transform.position + AroundVector[(int)Direction.Right];
                nextPosition = pos;
                break;

            case Direction.Up:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                pos = transform.position + AroundVector[(int)Direction.Up];
                nextPosition = pos;
                break;

            case Direction.Left:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                pos = transform.position + AroundVector[(int)Direction.Left];
                nextPosition = pos;
                break;

            case Direction.Down:
                sr.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                pos = transform.position + AroundVector[(int)Direction.Down];
                nextPosition = pos;
                break;

            case Direction.Null:
                break;

            default:
                break;

        }

    }

    //反転させた向きを返す
    Direction ReverseDirection(Direction dir)
    {
        if (dir == Direction.Right)
            return Direction.Left;
        else if (dir == Direction.Up)
            return Direction.Down;
        else if (dir == Direction.Left)
            return Direction.Right;
        else if (dir == Direction.Down)
            return Direction.Up;
        else
            return Direction.Null;
    }

    //時間変更(デバッグ用)
    void ChangeTime()
    {

        if (isNoon)
        {
            InitializeFishState();
            isNoon = false;
            SetAnimation();
            //FS.SetSpriteToNight();
        }
        else
        {
            InitializeFishState();
            isNoon = true;
            SetAnimation();
            //FS.SetSpriteToNoon();
        }

    }

    //魚を初期化する
    void InitializeFishState()
    {
        speed = normalSpeed;

        //魚が逃走状態か
        isRunAwayState = false;
        //プレイヤーを感知しているかどうか
        isSensing = false;
        //プレイヤーを追跡しているかどうか
        isChasing = false;
        //追跡クールタイム状態かどうか
        isChasingCoolTime = false;

        //逃走タイマー
        runAwayTimer = 0.0f;
        //追跡タイマー
        chaseTimer = 0.0f;
        //追跡クールタイマー
        chasingCoolTimer = 0.0f;

}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && isNoon)
        {
            if(!DiscoverPlayer(visibleDistanceInNoon))
            {
                Debug.Log("食べられちゃった!!");
                FishManager.instance.RemoveFish(this);
                Destroy(this.gameObject);
            }
        }
    }
    
    
    public void SetAnimation()
    {
        string animName = "";
        if (isNoon)
        {
            switch (direction)
            {
                case Direction.Right:
                    animName = "RightNoon";
                    break;
                case Direction.Up:
                    animName = "UpNoon";
                    break;
                case Direction.Left:
                    animName = "LeftNoon";
                    break;
                case Direction.Down:
                    animName = "DownNoon";
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Direction.Right:
                    animName = "RightNight";
                    break;
                case Direction.Up:
                    animName = "UpNight";
                    break;
                case Direction.Left:
                    animName = "LeftNight";
                    break;
                case Direction.Down:
                    animName = "DownNight";
                    break;
                default:
                    break;
            }
        }
            

        animator.Play(animName);
    }

}