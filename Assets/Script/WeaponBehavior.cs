using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(AudioSource))]
public class WeaponBehavior : MonoBehaviour
{
    public float BulletSpeed , RemainTime;
    public int FirePower;
    public GameObject HitEffect;
    public LayerMask PassThrough;
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
                status.status.hp -= FirePower;
            }
            else
            {
                status = collision.transform.parent.GetComponent<EnemyStatus>();
                status.status.hp -= FirePower;
            }
        }

            if (collision.transform.tag == "Player" && tag == "Enemy's Bullet")
        {
            PlayerMoving status = collision.transform.GetComponent<PlayerMoving>();
            status.status.Life -= FirePower;
        }
            if (isHit == true)
            {
                Destroy(gameObject);
                if (HitEffect != null)
                    Instantiate(HitEffect, transform.position, Quaternion.Euler(0, 0, 0));
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
            rigid2D.velocity = (Player.transform.position - transform.position).normalized * BulletSpeed;
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
