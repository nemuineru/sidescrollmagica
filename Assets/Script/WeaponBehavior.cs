/*
Copyright (c) 2014 vexe
Released under the MIT license
http://opensource.org/licenses/mit-license.php 
*/

using System.Collections;
using System;
using UnityEngine;
using Vexe.Runtime.Types;
using Vexe.FastSave;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(AudioSource))]
public class WeaponBehavior : BaseBehaviour
{
    public float BulletSpeed , RemainTime;
    public int FirePower;
    public GameObject HitEffect;
    public LayerMask PassThrough;
    public OneShotEvent eventstart;
    public Type type;
    public enum Type
    {
        Straight,
        Straight_AutoRotate,
        Gravity,
        Chase
    }

    Rigidbody2D rigid2D;

    float remaining = 0;
    // Use this for initialization

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isHit = true;
        Debug.Log(((int)Mathf.Pow(2, collision.gameObject.layer) & PassThrough.value));
            if ( ((int)Mathf.Pow(2,collision.gameObject.layer) & PassThrough.value) 
            == (int)Mathf.Pow(2, collision.gameObject.layer)
            || (collision.transform.tag == "Enemy" && tag == "Enemy's Bullet")
            || (collision.transform.tag == "Player" && tag == "Player's Bullet")) {
                isHit = false;
            }
        if (collision.transform.tag == "Enemy" && tag == "Player's Bullet")
        {
            EnemyStatus status;
            if (collision.GetComponent<EnemyHitComp_RefTo>() != null)
            {
                status = collision.transform.GetComponent<EnemyHitComp_RefTo>().Status;
                GameObject.Find("Status").GetComponent<StatusComp>().TargetedEnemy = status.gameObject;
                status.status.hp -= FirePower;
            }
        }

            if (collision.transform.tag == "Player" && tag == "Enemy's Bullet")
            {
            PlayerMoving status = collision.transform.parent.GetComponent<PlayerMoving>();
            PlayerMoving Player = collision.transform.parent.GetComponent<PlayerMoving>();
            if (status.status.InvisTime == 0)
            {
                Player.Damaged = true;
                status.status.Life -= FirePower;
                Player.gameObject.GetComponent<Rigidbody2D>().velocity =
                    Mathf.Sign(Player.transform.localScale.x) * Vector2.left * FirePower * 4
                    +
                    Vector2.up * FirePower * 2;
               isHit = true;
            }
        }
            if (isHit == true)
            {
                Destroy(gameObject);
                if (HitEffect != null)
                    Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, 0));
            }
        if (eventstart != null) {
            string output = eventstart.gameObject.SaveHierarchyToMemory().GetString();
            Load.HierarchyFromMemory(output.GetBytes(),new GameObject(eventstart.name));
        }
    }

    void Start()
    {

        rigid2D = GetComponent<Rigidbody2D>();

        switch (type)
        {
            case Type.Straight:
                StartCoroutine("S_Straight");
                break;
            case Type.Straight_AutoRotate:
                StartCoroutine("S_AutoRotate");
                break;
            case Type.Gravity:
                StartCoroutine("S_Gravity");
                break;
            case Type.Chase:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        remaining += Time.deltaTime;
        if(remaining > RemainTime)
        {
            Destroy(gameObject);
            if (HitEffect != null)
                Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    IEnumerator S_Straight()
    {
        rigid2D.velocity = new Vector2(BulletSpeed,0);
        yield return null;
    }

    IEnumerator S_AutoRotate()
    {
        if (tag == "Enemy's Bullet")
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            Vector2 ToPos = (Player.transform.position - transform.position);
            rigid2D.velocity = ToPos.normalized * BulletSpeed;
        }
        yield return null;
    }

    IEnumerator S_Gravity()
    {
        rigid2D.velocity = new Vector2(BulletSpeed, Mathf.Abs(BulletSpeed / 2));
        while (Application.isPlaying){
            transform.rotation = Quaternion.Euler(0,0,
                Mathf.Rad2Deg * Mathf.Asin(rigid2D.velocity.normalized.y / rigid2D.velocity.normalized.x));
            yield return null;
        }
    }
}

