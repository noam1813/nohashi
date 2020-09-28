using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Runtime.CompilerServices;

public class PlayerManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap stageTilemap;

    private Direction nowDirection;
    private Direction ReserveDirection;
    private SpriteRenderer sr;
    private Vector3[] AroundVector = new Vector3[4];
    private Animator animator;
    
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
    }
    
    
    private void FixedUpdate()
    {
        PlayerMove();
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

        switch (direc)
        {
            case Direction.Right:
                pos += AroundVector[0];
                animator.Play("Right");
                break;
            case Direction.Up:
                pos += AroundVector[1];
                animator.Play("Up");
                break;
            case Direction.Left:
                pos += AroundVector[2];
                animator.Play("Left");
                break;
            case Direction.Down:
                pos += AroundVector[3];
                animator.Play("Down");
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
                AroundBlock[v.index] = true;
            }
            else
            {
                AroundBlock[v.index] = false;
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
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("ゲームオーバー");
        }
    }

}
