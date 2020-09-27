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
        Null, Right, Up, Left, Down
    }

    private Direction direction;
    private SpriteRenderer sr;
    //右上左下のタイルマップの位置(補正)
    private Vector3[] AroundVector = new Vector3[4];

    public float speed = 1.0f;
    public Vector3 nextPosition;

    void Start()
    {
        stageTilemap = grid.transform.Find("Stage").GetComponent<Tilemap>();
        direction = Direction.Left;
        sr = this.transform.Find("Sprites/Noon").GetComponent<SpriteRenderer>();
        AroundVector[0] = new Vector3(1, 0, 0);
        AroundVector[1] = new Vector3(0, 1, 0);
        AroundVector[2] = new Vector3(-1, 0, 0);
        AroundVector[3] = new Vector3(0, -1, 0);

        DecideNextPosition();
        
    }

    void FixedUpdate()
    {
        //Debug.Log("実際のポジションは" + this.transform.position + "です");
        //Debug.Log("グリッドされたポジションは" + grid.WorldToCell(this.transform.position) + "です");
        //var position = new Vector3Int(-6, 4, 0);
        //print(stageTilemap.GetTile(position));

        FishMove();
    }

    

    void FishMove()
    {
        if(direction == Direction.Right)
        {
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            this.transform.position = pos;
            if (this.transform.position.x >= nextPosition.x)
            {
                pos.x = nextPosition.x;
                this.transform.position = pos;
                DecideNextPosition();
            }
        }
        else if (direction == Direction.Up)
        {
            Vector3 pos = transform.position;
            pos.y += speed * Time.deltaTime;
            this.transform.position = pos;
            if (this.transform.position.y >= nextPosition.y)
            {
                pos.y = nextPosition.y;
                this.transform.position = pos;
                DecideNextPosition();
            }
        }
        else if (direction == Direction.Left)
        {
            Vector3 pos = transform.position;
            pos.x -= speed * Time.deltaTime;
            this.transform.position = pos;
            if (this.transform.position.x <= nextPosition.x)
            {
                pos.x = nextPosition.x;
                this.transform.position = pos;
                DecideNextPosition();
            }
        }
        else if (direction == Direction.Down)
        {
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            this.transform.position = pos;
            if (this.transform.position.y <= nextPosition.y)
            {
                pos.y = nextPosition.y;
                this.transform.position = pos;
                DecideNextPosition();
            }
        }
    }

    //移動方向を決める
    void DecideNextPosition()
    {
        bool[] blockflag = CheckAroundBlockForMovingDirection();
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

        if (direction == Direction.Right)
        {
            sr.flipX = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 pos = transform.position + AroundVector[0];
            nextPosition = pos;
        }
        else if (direction == Direction.Up)
        {
            sr.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, -90);
            Vector3 pos = transform.position + AroundVector[1];
            nextPosition = pos;
        }
        else if (direction == Direction.Left)
        {
            sr.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 pos = transform.position + AroundVector[2];
            nextPosition = pos;
        }
        else if (direction == Direction.Down)
        {
            sr.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 90);
            Vector3 pos = transform.position + AroundVector[3];
            nextPosition = pos;
        }

    }

    //周囲が移動可能か調べる
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

}
