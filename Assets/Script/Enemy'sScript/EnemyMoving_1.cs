﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D),typeof(EnemyStatus))]
public class EnemyMoving_1 : MonoBehaviour {

    public bool isTracePlayer;

    public enum EnemyMoveType {
        Float,
        Straight,
        Jumping,
        Stay
    }

    public EnemyMoveType moveType;


    Animator anims;
    Rigidbody2D rigid2d;
    EnemyStatus.Status status;
    LayerMask Mask;
    int facing = 1;
    // Use this for initialization
    void Start()
    {
        anims = GetComponent<Animator>();
        Mask = LayerMask.GetMask("Terrain") + LayerMask.GetMask("HitLayer");
        rigid2d = GetComponent<Rigidbody2D>();
        status = GetComponent<EnemyStatus>().status;
        switch (moveType)
        {
            case EnemyMoveType.Straight:
                StartCoroutine(MoveStraight());
                break;
            case EnemyMoveType.Float:
                StartCoroutine(MoveFloat());
                break;
            case EnemyMoveType.Stay:
                StartCoroutine(MoveStay());
                break;
            case EnemyMoveType.Jumping:
                StartCoroutine(MoveJump());
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
            LayerMask NewMask = LayerMask.GetMask("Terrain");
            if (ObjectDetect
                (new Vector3(HitBox.size.x / 2, 0) * facing,
                Vector2.right * facing,NewMask
                ,0.5f) != gameObject)
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


    //ジャンプの動作。
    IEnumerator MoveJump()
    {
        LayerMask terrainlayer = LayerMask.GetMask("Terrain");
        float waittime = 0;
        while (Application.isPlaying)
        {
            BoxCollider2D HitBox = GetComponent<BoxCollider2D>();
            anims.SetBool("OnGround",TerrainNearby(new Vector2(0, -HitBox.size.y - 0.0001f), Vector2.down, Mask, 0.0001f));
            if (TerrainNearby(new Vector2(0,-HitBox.size.y - 0.0001f),Vector2.down,Mask,0.0001f))
            {
                if (waittime > status.AttackSpeed)
                {
                    Vector2 JumpVecs =  Quaternion.Euler(0,0,80f) * Vector2.right * status.MoveSpeed;
                    JumpVecs = new Vector2(JumpVecs.x * facing, JumpVecs.y);
                    rigid2d.velocity = new Vector2(JumpVecs.x,JumpVecs.y);
                    waittime = 0;
                }
                else
                {
                    waittime += Time.deltaTime;
                }
            }
            if (TerrainNearby(new Vector3(HitBox.size.x * facing / 2, -HitBox.size.y / 2), transform.right , terrainlayer.value))
            {
                facing *= -1;
            }
            rigid2d.velocity = new Vector2(Mathf.Abs(rigid2d.velocity.x) * facing, rigid2d.velocity.y);
            yield return null;
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
        if (Mathf.Abs(rigid2d.velocity.x) > 0.1)
        {
            Vector3 scale = transform.localScale;
            transform.localScale = 
                new Vector3(Mathf.Sign(rigid2d.velocity.x) * Mathf.Abs(scale.x), scale.y, scale.z);
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
