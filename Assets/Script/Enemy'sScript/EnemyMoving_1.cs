using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving_1 : MonoBehaviour {

    public enum EnemyMoveType {
        Float,
        Straight,
        Jumping,
        Stay
    }

    public EnemyMoveType moveType;



    Rigidbody2D rigid2d;
    EnemyStatus.Status status;
    LayerMask Mask;
    int facing = 1;
    // Use this for initialization
    void Start()
    {
        Mask = LayerMask.GetMask("Terrain") + LayerMask.GetMask("Entity");
        rigid2d = GetComponent<Rigidbody2D>();
        status = GetComponent<EnemyStatus>().status;
        switch (moveType)
        {
            case EnemyMoveType.Straight:
                StartCoroutine("MoveStraight");
                break;
            case EnemyMoveType.Float:
                StartCoroutine("MoveFloat");
                break;
            case EnemyMoveType.Stay:
                StartCoroutine("MoveStay");
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        Flip();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator MoveStraight()
    {
        while (Application.isPlaying) {
            rigid2d.velocity = new Vector2
                (status.MoveSpeed * facing, rigid2d.velocity.y);
            BoxCollider2D HitBox = GetComponent<BoxCollider2D>();

            if (ObjectDetect
                (new Vector3(HitBox.size.x / 2, 0) * facing,
                Vector2.right * facing,
                Mask.value,0.1f) != gameObject ||
                !(TerrainNearby(new Vector3(HitBox.size.x * facing / 2, -HitBox.size.y / 2), Vector2.down, Mask.value))) {
                facing *= -1;
            }

            yield return null;
        }
    }

    IEnumerator MoveStay()
    {
        while (Application.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator MoveFloat()
    {
        float angle = 0;
        while (Application.isPlaying)
        {
            angle = (360 + angle + status.MoveSpeed * Random.Range(-30f, 30f)) % 360;
            rigid2d.AddForce(Quaternion.Euler(0, 0, angle * facing) * new Vector2(status.MoveSpeed, 0));
            BoxCollider2D HitBox = GetComponent<BoxCollider2D>();
            if (TerrainNearby
                (new Vector3(HitBox.size.x / 2, 0) * facing,
                Vector2.right * facing,
                Mask.value))
            {
                rigid2d.velocity = new Vector2(-1 * rigid2d.velocity.x, rigid2d.velocity.y);
                facing *= -1;
                new WaitForSeconds(0.1f);
            }
            Debug.DrawLine(transform.position, rigid2d.velocity, Color.green);
            yield return null;
            Flip();
        }
    }

    bool TerrainNearby(Vector2 ObjectGroundPos, Vector2 Direction, int layerMask)
    {
        Ray2D ray1 = new Ray2D();
        ray1.direction = Direction;
        ray1.origin = (Vector2)transform.position + ObjectGroundPos;
        RaycastHit2D Wall_1 = Physics2D.Raycast(ray1.origin, ray1.direction, 100f, layerMask);
        if ((Wall_1.distance > 0.1f || Wall_1.collider == null))
        {
            Debug.DrawLine(ray1.origin, Wall_1.point, new Color(1f, 0, 0));
            return false;
        }
        else
        {
            Debug.DrawLine(ray1.origin, Wall_1.point, new Color(1f, 0, 1f));
            return true;
        }
    }

    void Flip() {
        if (rigid2d.velocity.x > 0.1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rigid2d.velocity.x < -0.1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
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

    GameObject ObjectDetect(Vector2 pos1, Vector2 Direction, int layerMask, float CollideDist)
    {
        Ray2D ray1 = new Ray2D();
        ray1.direction = Direction;
        ray1.origin = (Vector2)transform.position + pos1;
        RaycastHit2D[] Wall_1 = Physics2D.RaycastAll(ray1.origin, ray1.direction, CollideDist, layerMask);
        foreach (RaycastHit2D Hit2D in Wall_1) {
            if (Hit2D.collider.gameObject != gameObject && Hit2D.collider.transform.root != gameObject)
            {
                Debug.DrawLine(transform.position, Hit2D.point , Color.green);
                return Hit2D.collider.gameObject;
            }
        }
        return gameObject;
    }
}
