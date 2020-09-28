using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine;

public class FishMoveInNoon : MonoBehaviour
{
    public Grid grid;
    public Tilemap stageTilemap;

    enum Direction
    {
        Null=-1, Right=0, Up=1, Left=2, Down=3
    }

    private Direction direction;
    private SpriteRenderer sr;
    //右上左下のタイルマップの位置(補正)
    private Vector3[] AroundVector = new Vector3[4];

    [SerializeField] float speed;
    //魚の通常速度
    public float normalSpeed = 0.8f;
    //魚の逃げる速度
    public float runAwaySpeed = 1.3f;

    //魚の座標
    public Vector3 nowPosition;
    public Vector3 nextPosition;

    //魚の可視距離
    public float visibleDistance = 3.0f;

    //魚が逃走状態か
    bool isRunAwayState;
    //魚の逃走状態が継続する時間
    public float runAwayTime = 5.0f;
    //タイマー
    public float runAwayTimer = 0.0f;
    //プレイヤーを発見した方向
    [SerializeField] Direction discoveredPlayerDirection = Direction.Null;

    void Start()
    {
        stageTilemap = grid.transform.Find("Stage").GetComponent<Tilemap>();
        direction = Direction.Left;
        sr = this.transform.Find("Sprites/Noon").GetComponent<SpriteRenderer>();
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
        
    }

    void FixedUpdate()
    {
        FishMove();
        RunAwayFromPlayer();
    }

    void FishMove()
    {
        if(isRunAwayState == true && runAwayTimer < runAwayTime)
        {
            runAwayTimer += Time.deltaTime;
        }
        else if(isRunAwayState == true && runAwayTimer >= runAwayTime)
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

    //移動方向を決める
    void DecideNextPosition()
    {
        bool[] blockflag = CheckAroundBlockToMove();
        List<Direction> canMoveDir = new List<Direction>();

        if (!blockflag[0] && direction != Direction.Left)
            canMoveDir.Add(Direction.Right);
        if (!blockflag[1] && direction != Direction.Down)
            canMoveDir.Add(Direction.Up);
        if (!blockflag[2] && direction != Direction.Right)
            canMoveDir.Add(Direction.Left);
        if (!blockflag[3] && direction != Direction.Up)
            canMoveDir.Add(Direction.Down);

        if(canMoveDir.Count >= 1)
        {
            int rand = Random.Range(0, canMoveDir.Count);
            direction = canMoveDir[rand];
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
    bool[] CheckAroundBlockToMove()
    {
        Vector3 nowPosition = grid.WorldToCell(this.transform.position);

        //右上左下の順
        bool[] AroundBlock = new bool[4];

        foreach(var v in AroundVector.Select((value, index) => new { value, index }))
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
    bool DiscoverPlayer()
    {
        Vector2 rayDirection;
        switch(direction)
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

        Ray2D ray = new Ray2D(transform.position, rayDirection);
        Debug.DrawRay(ray.origin + rayDirection, (Vector3)ray.direction * visibleDistance, Color.green, 0.1f, false);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)rayDirection, rayDirection, visibleDistance);

        if(hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("プレイヤー発見！");
                return true;
            }
        }
        
        return false;
    }

    //プレイヤーを発見したら逃げる
    void RunAwayFromPlayer()
    {
        if(DiscoverPlayer())
        {
            isRunAwayState = true;
            runAwayTimer = 0.0f;
            nextPosition = nowPosition;
            speed = runAwaySpeed;

            switch(direction)
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
        }
    }


    //周囲のブロックをお知らせ(デバッグ用)
    void AroundInfo()
    {
        bool[] blockflag = CheckAroundBlockToMove();

        print("右のブロックフラグは" + blockflag[(int)Direction.Right] + "です");
        print("上のブロックフラグは" + blockflag[(int)Direction.Up] + "です");
        print("左のブロックフラグは" + blockflag[(int)Direction.Left] + "です");
        print("下のブロックフラグは" + blockflag[(int)Direction.Down] + "です");
    }

}