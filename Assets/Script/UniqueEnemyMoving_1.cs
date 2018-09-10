using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEnemyMoving_1 : MonoBehaviour {

    public enum MoveType
    {
        Golems
    }
    public MoveType Type;
    public GameObject[] Things;
    EnemyStatus status;
    Animator animator;
    float ThinkTime = 0;


    bool isAIChanged = false;
    Rigidbody2D rigidbody2D;
    BoxCollider2D HitBox;
    GameObject Player;
    // Use this for initialization
    void Start () {
        HitBox = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
        switch (Type)
        {
            case MoveType.Golems:
                StartCoroutine("MovType_Golem");
                break;
            default:
                break;
        }
        StartCoroutine("MovingScript");
    }
	
	// Update is called once per frame
	void Update () {

    }

    enum AI {
        Attack,
        MoveHorizontal,
        MoveJump,
        Nothing
    }

    IEnumerator MovType_Golem()
    {
        AI ai = AI.Nothing, prev_ai; //AIは最初は何もしない｡
        Vector2 Colbox_LD, Colbox_RD;
        Colbox_LD = -GetComponent<BoxCollider2D>().size / 2 + GetComponent<BoxCollider2D>().offset;
        Colbox_RD = new Vector2(-Colbox_LD.x, Colbox_LD.y) + GetComponent<BoxCollider2D>().offset; //コリドルボックスの右下左下
        while (Application.isPlaying)
        {
            LayerMask Mask = LayerMask.GetMask("Terrain");
            bool terrainNear = TerrainNearby(Colbox_LD, Vector2.down, Mask , 0.1f) 
                || TerrainNearby(Colbox_RD, Vector2.down, Mask, 0.1f);
            bool PlayerFar = Mathf.Abs(Vector2.Distance
                    (transform.position, Player.transform.position)) > 1.0;
            bool PlayerHigh = transform.position.y - Player.transform.position.y < -0.1;
            
       
                if (ai == AI.MoveJump)
                {
                    ai = AI.Nothing;
                }
            if (ThinkTime > 0.5f)
            {
                if (PlayerFar)
                {
                    if (terrainNear && Random.Range(0, 100) < 3)
                    {
                        ai = AI.MoveHorizontal;
                    }
                    else if (terrainNear && ((PlayerHigh && Mathf.Abs(rigidbody2D.velocity.x) > 0.3
                        && Random.Range(0, 50) < 20)||(Random.Range(0,100) < 5)))
                    {
                        ai = AI.MoveJump;
                    }
                    else {
                        ai = AI.Nothing;
                    }
                }
                if (ai != AI.Nothing)//AIがなにかしている時は思考時間を0にリセットする｡
                {
                    ThinkTime = 0;                    
                }
                prev_ai = ai;
            }

            switch (ai){ //AIの挙動
                case AI.MoveHorizontal:
                    rigidbody2D.velocity = new Vector2(1.5f * -Mathf.Sign
                        (transform.position.x - Player.transform.position.x), 
                        rigidbody2D.velocity.y);
                    break;
                case AI.MoveJump:
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 4);
                    break;
                default:
                    break;
            }
            ThinkTime += Time.deltaTime;
            
            yield return null;
        }
    }

    IEnumerator MovingScript() {
        while (Application.isPlaying)
        {
            if(Mathf.Abs(rigidbody2D.velocity.x) > 0.05)
            transform.localScale = new Vector2(1 * Mathf.Sign(rigidbody2D.velocity.x),1);
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0.05) {
                animator.SetBool("IsMoving", true);
            }
            else
                animator.SetBool("IsMoving", false);
            yield return null;
        }
    }

    bool TerrainNearby(Vector2 pos1, Vector2 Direction, int layerMask, float CollideDist)
    {
        Ray2D ray1 = new Ray2D();
        ray1.direction = Direction;
        ray1.origin = (Vector2)transform.position + pos1;
        RaycastHit2D Wall_1 = Physics2D.Raycast(ray1.origin, ray1.direction, 100f, layerMask);
        if ((Wall_1.distance > CollideDist || Wall_1.collider == null))
        {
            Debug.DrawLine(ray1.origin, Wall_1.point, new Color(1f, 0, 0));
            return false;
        }
        else
        {
            Debug.DrawLine(ray1.origin, Wall_1.point, new Color(0, 1f, 1f));
            return true;
        }
    }
}
