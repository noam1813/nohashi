using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.CompilerServices;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    public Grid grid;
    public Tilemap stageTilemap;
    public int nowKai;

    [SerializeField] private Direction nowDirection;
    private Direction ReserveDirection;
    private Direction nowAnimatingDirection;
    private SpriteRenderer sr;
    private Vector3[] AroundVector = new Vector3[4];
    private Animator animator;
    
    // 水草関係
    public bool isHide;
    private Mizukusa useMizukusa;
    

    private bool isSceneEnded;

    //SerializeField : 変数の扱いをprivate扱いにしながらインスペクタに入力欄を表示することができる
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector3 nextPosition;

    
    [System.Serializable]
    enum Direction
    {
        Null, Right, Up, Left, Down
    }


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            DestroyImmediate(gameObject);
        }
        
        isSceneEnded = false;

        stageTilemap = grid.transform.Find("Stage").GetComponent<Tilemap>();
        ReserveDirection = Direction.Left;
        AroundVector[0] = new Vector3(1, 0, 0); //右
        AroundVector[1] = new Vector3(0, 1, 0); //上
        AroundVector[2] = new Vector3(-1, 0, 0); //左
        AroundVector[3] = new Vector3(0, -1, 0); // 下

        animator = gameObject.GetComponent<Animator>();
        
        DecideNextPosition();
    }


    private void Update()
    {
        GetReserveDirection();
        //秘匿有効時、Shiftキーが入力されているかチェックする
        if (isHide)
        {
            ManageHideMode();
        }
    }
    
    
    private void FixedUpdate()
    {
        //秘匿時はプレイヤーは動けない
        if (!isHide)
        {
            PlayerMove();   
        }
        SetAnimation();
    }


    private void PlayerMove()
    {
        Vector3 pos = transform.position;
        switch (nowDirection)
        {
            case Direction.Right:
                pos.x += speed * Time.deltaTime;
                if (pos.x >= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    transform.position = pos;
                    DecideNextPosition();
                }
                break;
            
            case Direction.Up:
                pos.y += speed * Time.deltaTime;
                if (pos.y>= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    transform.position = pos;
                    DecideNextPosition();
                }
                break;
            
            case Direction.Left:
                pos.x -= speed * Time.deltaTime;
                if (pos.x <= nextPosition.x)
                {
                    pos.x = nextPosition.x;
                    transform.position = pos;
                    DecideNextPosition();
                }
                break;
            
            case Direction.Down:
                pos.y -= speed * Time.deltaTime;
                if (pos.y <= nextPosition.y)
                {
                    pos.y = nextPosition.y;
                    transform.position = pos;
                    DecideNextPosition();
                }
                break;
            
            case Direction.Null:
                DecideNextPosition();
                break;
            
            default:
                Debug.Log("Now Direction Error! : "+nowDirection);
                break;
            
        }
        this.transform.position = pos;
    }

    private void ManageHideMode()
    {
        if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            useMizukusa.endUse();
            Debug.Log("終わり");
        }
    }


    private void GetReserveDirection()
    {
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ReserveDirection = Direction.Right;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            ReserveDirection = Direction.Up;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            ReserveDirection = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ReserveDirection = Direction.Down;
        }
        else
        {
            return;
        }

        if (nowDirection == Direction.Null)
        {
            DecideNextPosition();
        }
        
    }


    private void DecideNextPosition()
    {
        if (ReserveDirection == Direction.Null)
        {
            DecideNextPositionFromNow();
        }
        else
        {
            DecideNextPositionFromReserve();
        }

    }


    private void DecideNextPositionFromNow()
    {
        bool[] blockflag = CheckAroundBlockForMovingDirection();
        if (!blockflag[0] && nowDirection == Direction.Right)
        {
            SetDirection(Direction.Right);
        }
        else if (!blockflag[1] && nowDirection == Direction.Up)
        {
            SetDirection(Direction.Up);
        }
        else if (!blockflag[2] && nowDirection == Direction.Left)
        {
            SetDirection(Direction.Left);
        }
        else if (!blockflag[3] && nowDirection == Direction.Down)
        {
            SetDirection(Direction.Down);
        }
        else
        {
            SetDirection(Direction.Null);
        }
    }


    private void DecideNextPositionFromReserve()
    {
        bool[] blockflag = CheckAroundBlockForMovingDirection();
        
        if (!blockflag[0] && ReserveDirection == Direction.Right)
        {
            SetDirection(Direction.Right);
            ReserveDirection = Direction.Null;
        }
                
        else if (!blockflag[1] && ReserveDirection == Direction.Up)
        {
            SetDirection(Direction.Up);
            ReserveDirection = Direction.Null;
        }
                
        else if (!blockflag[2] && ReserveDirection == Direction.Left)
        {
            SetDirection(Direction.Left);
            ReserveDirection = Direction.Null;
        }
                
        else if (!blockflag[3] && ReserveDirection == Direction.Down)
        {
            SetDirection(Direction.Down);
            ReserveDirection = Direction.Null;
        }

        else
        {
            DecideNextPositionFromNow();
        }
    }
    


    private void SetDirection(Direction direc)
    {
        nowDirection = direc;
        Vector3 pos = transform.position;
        TileMapManager.instance.SetThroughedMap(pos);

        switch (direc)
        {
            case Direction.Right:
                pos += AroundVector[0];
                break;
            case Direction.Up:
                pos += AroundVector[1];
                break;
            case Direction.Left:
                pos += AroundVector[2];
                break;
            case Direction.Down:
                pos += AroundVector[3];
                break;

        }

        nextPosition = pos;
    }
    
    
    bool[] CheckAroundBlockForMovingDirection()
    {
        Vector3 nowPosition = grid.WorldToCell(this.transform.position);

        //右上左下の順
        bool[] AroundBlock = new bool[4];

        foreach(var v in AroundVector.Select((value, index) => new { value, index }))
        {
            if (stageTilemap.GetTile(grid.WorldToCell(this.transform.position + v.value)) != null)
            {
                AroundBlock[v.index] = false;
            }
            else
            {
                AroundBlock[v.index] = true;
            }
        }
        
        return AroundBlock;
    }
    
    
    //周囲のブロックをお知らせ(デバッグ用)
    void AroundInfo()
    {
        bool[] blockflag = CheckAroundBlockForMovingDirection();

        print("右のブロックフラグは" + blockflag[0] + "です");
        print("上のブロックフラグは" + blockflag[1] + "です");
        print("左のブロックフラグは" + blockflag[2] + "です");
        print("下のブロックフラグは" + blockflag[3] + "です");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy" && TimeManager.instance.timeZone == TimeZoneData.Night && !isSceneEnded)
        {
            isSceneEnded = true;

            SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
            {
                SceneManager.LoadScene("GameOverScene");
            });

            //Debug.Log("ゲームオーバー");

        }

        if (other.gameObject.tag == "Kai")
        {
            nowKai += 1;
            Debug.Log("ゲット");
            Destroy(other.gameObject);
            if (nowKai == 1)
            {
                KaiManager.instance.kai1.color = new Color(255, 255, 255, 255);
            }
            else if (nowKai == 2)
            {
                KaiManager.instance.kai2.color = new Color(255, 255, 255, 255);
            }
            else if (nowKai == 3)
            {
                KaiManager.instance.kai3.color = new Color(255, 255, 255, 255); 
            }
            else if (nowKai == 4)
            {
                KaiManager.instance.kai4.color = new Color(255, 255, 255, 255); 
            }

            if (nowKai >= KaiManager.instance.MaxKai)
            {
                ResultDataManager.instance.SetResultData();

                SceneFadeManager.Instance.StartFade(SceneFadeManager.FADE_TYPE.FADE_OUTIN, 0.4f, () =>
                {
                    SceneManager.LoadScene("ResultScene");
                });
                //Debug.Log("貝全部ゲット");
            }
        }
    }

    
    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            //Shiftキーの入力&秘匿設定がされてない際実行
            case "Mizukusa":
                if (Input.GetKey(KeyCode.LeftShift) && !isHide)
                {
                    Debug.Log("はじまり");
                    other.gameObject.GetComponent<Mizukusa>().StartUse();
                }

                break;
        }
        
    }


    /// <summary>
    /// 移動方向に合わせてアニメーションを設定
    /// </summary>
    public void SetAnimation()
    {
        String animName = "";
        if (nowDirection != Direction.Null && nowDirection != nowAnimatingDirection)
        {
            nowAnimatingDirection = nowDirection;
        }

        if (nowDirection == Direction.Null)
        {
            switch (nowAnimatingDirection)
            {
                case Direction.Right:
                    animName = "RightIdle";
                    break;
                case Direction.Up:
                    animName = "UpIdle";
                    break;
                case Direction.Left:
                    animName = "LeftIdle";
                    break;
                case Direction.Down:
                    animName = "DownIdle";
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (nowAnimatingDirection)
            {
                case Direction.Right:
                    animName = "Right";
                    break;
                case Direction.Up:
                    animName = "Up";
                    break;
                case Direction.Left:
                    animName = "Left";
                    break;
                case Direction.Down:
                    animName = "Down";
                    break;
                default:
                    break;
            }
        }

            animator.Play(animName);
    }

    
    /// <summary>
    /// 水草を用いたプレイヤーの秘匿設定
    /// 有効、無効、使用している水草の設定
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="mizukusa"></param>
    public void SetHide(bool mode,Mizukusa mizukusa = null)
    {
        isHide = mode;
        if (mode)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
            useMizukusa = mizukusa;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
            useMizukusa = null;
        }
        
        //秘匿時、暗くなるエフェクトをかける
        StageEffectManager.instance.SetShadow(mode);
    }


    public Vector2 SetPlayerPosition()
    {
        return transform.position;
    }

}

