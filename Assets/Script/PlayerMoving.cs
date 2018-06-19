﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour {

    public sound Sound;
    [System.Serializable]
    public class sound
    {
        public AudioClip Jump, Run, Hurt;
        public GameObject RunDust;
    }
    public Status status;
    [System.Serializable]
    public class Status
    {
        public int Life, LifeMax;
        public float InvisTime, InvisMaxTime;
        public float speed = 0, jumppower = 0, CastTime = 0;
        public int AirjumpNum = 1;
    }
    public bool Damaged;
    public Vector2 PlayerLDEdge, PlayerRDEdge;
    public Vector2 PlayerCastPoint;
    int RestAirJump;



    Rigidbody2D rigid2d;
    Animator Animations;
    AudioSource audio;

    WeaponStates weaponStates;

    bool WallNearby = false, OnGround = false, OnBothGround = false;
    LayerMask layermask;
    Vector2 PlayerFacing;

    Material SpriteMat;
    // コルーチンを使用してキャラの動作を行う
    void Start() {
        SpriteMat = GetComponent<SpriteRenderer>().material;
        rigid2d = GetComponent<Rigidbody2D>();
        Animations = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        weaponStates = GetComponent<WeaponStates>();

        PlayerFacing = Vector2.right * Mathf.Sign(transform.localScale.x);

        layermask = LayerMask.GetMask("LivingThings") + LayerMask.GetMask("Terrain");
        WallNearby = (TerrainNearby(PlayerLDEdge, PlayerFacing, layermask))
            || (TerrainNearby(PlayerRDEdge, PlayerFacing, layermask));
        OnGround = (TerrainNearby(PlayerLDEdge, Vector2.down, layermask))
            || (TerrainNearby(PlayerRDEdge, Vector2.down, layermask));
        OnBothGround = (TerrainNearby(PlayerLDEdge, Vector2.down, layermask))
            && (TerrainNearby(PlayerRDEdge, Vector2.down, layermask));

        StartCoroutine("Moving");
        StartCoroutine("AnimChange");
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        PlayerFacing = Vector2.right * Mathf.Sign(transform.localScale.x);

        WallNearby = (TerrainNearby(PlayerLDEdge, PlayerFacing, layermask))
            || (TerrainNearby(PlayerRDEdge, PlayerFacing, layermask));
        OnGround = (TerrainNearby(PlayerLDEdge, Vector2.down, layermask))
            || (TerrainNearby(PlayerRDEdge, Vector2.down, layermask));
        OnBothGround = (TerrainNearby(PlayerLDEdge, Vector2.down, layermask))
            && (TerrainNearby(PlayerRDEdge, Vector2.down, layermask));
    }


    GameObject SelectedMagic;
    // キャラの動き。
    IEnumerator Moving() {
        float AttackPressed = 0;
        string MagType = "Short";
        while (Application.isPlaying)
        {

            if (Input.GetButtonUp("Magic"))
            {
                Vector2 PlayerFacing = new Vector3(1f, 0f) * transform.localScale.x;
                switch(MagType){
                    case "Short":
                        SelectedMagic = weaponStates.weapon.Short;
                        break;
                    case "Long":
                        SelectedMagic = weaponStates.weapon.Long;
                        break;
                    case "Super":
                        SelectedMagic = weaponStates.weapon.Super;
                        break;
                    default:
                        break;
                }
                GameObject Magic;
                Magic = Instantiate(SelectedMagic,
                    transform.position + (Vector3)PlayerCastPoint * PlayerFacing.x
                    , Quaternion.Euler(0f, 0f, 0f));
                Magic.GetComponent<WeaponBehavior>().BulletSpeed *= PlayerFacing.x;
            }

            if (Input.GetButton("Magic"))
            {
                AttackPressed += Time.deltaTime;
            }
            else
            {
                AttackPressed = 0;
            }

            //AttackPressedの値で出せる魔法が決まる。
            if (AttackPressed > status.CastTime * 3)
            {
                SpriteMat.SetFloat("_Val", AttackPressed * 50 % 5 + 1);
                MagType = "Super";
                Debug.Log("Super");
            }
            else if (AttackPressed > status.CastTime / 2)
            {
                SpriteMat.SetFloat("_Val", AttackPressed * 25 % 5 + 1);
                MagType = "Long";
                Debug.Log("Long");
            }
            else
            {
                SpriteMat.SetFloat("_Val",1f);
                MagType = "Short";
                Debug.Log("Short");
            }

            // キャラクターのX方向の動作。
            if (Mathf.Abs(rigid2d.velocity.x) < 0.05 && !WallNearby && OnGround)
            {
                rigid2d.velocity = new Vector2(Input.GetAxis("Horizontal") * status.speed / 1.5f,
                  rigid2d.velocity.y);
            }

            rigid2d.velocity = new Vector2(Input.GetAxis("Horizontal") * status.speed / 10 + rigid2d.velocity.x,
              rigid2d.velocity.y);

            if (Mathf.Abs(rigid2d.velocity.x) > status.speed && !WallNearby)
            {
                rigid2d.velocity = new Vector2(Mathf.Sign(rigid2d.velocity.x) * status.speed,
                rigid2d.velocity.y);
            }

            //ジャンプ時。
            if (Input.GetButtonDown("Jump") && (OnGround || RestAirJump > 0))
            {
                if (WallNearby && !OnBothGround)
                {
                    rigid2d.velocity = new Vector2(rigid2d.velocity.x + status.jumppower * transform.localScale.x * -1,
                        status.jumppower);
                }
                else if (!OnGround) {
                    rigid2d.velocity = new Vector2(rigid2d.velocity.x,
                            status.jumppower / 1.5f);
                }
                else
                {
                    rigid2d.velocity = new Vector2(rigid2d.velocity.x,
                           status.jumppower);
                }

                audio.clip = Sound.Jump;
                audio.Play();
                Instantiate(Sound.RunDust, transform.position + (Vector3)
                (PlayerLDEdge + PlayerRDEdge) + new Vector3(0f, 0.01f), Quaternion.Euler(Vector3.zero));


                if (OnGround == false) {
                    RestAirJump--;
                }
            }
            if (OnGround)
            {
                RestAirJump = status.AirjumpNum;
            }


            yield return null;
        }
    }

    //アニメーションの変更。
    IEnumerator AnimChange()
    {
        while (Application.isPlaying)
        {
            //キャラがいずれの方向にも動いている時。
            if (Mathf.Abs(rigid2d.velocity.normalized.magnitude) > 0.2)
            {
                Animations.SetBool("IsMoving", true);
            }
            else
            {
                Animations.SetBool("IsMoving", false);
            }

            if (rigid2d.velocity.x > 0.1)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (rigid2d.velocity.x < -0.1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            AnimatorStateInfo States = Animations.GetCurrentAnimatorStateInfo(0);

            //アニメスピードはキャラのスピードにより変化。
            if (States.fullPathHash == Animator.StringToHash("Base Layer.Walking"))
                Animations.speed = 3f * (Mathf.Abs(rigid2d.velocity.x) / status.speed);
            else {
                Animations.speed = 1f;
            }

            if (WallNearby && !OnBothGround)
            {
                Animations.SetBool("IsWallNearby", true);
            }
            else
            {
                Animations.SetBool("IsWallNearby", false);
            }

            //キャラが地面に居てX方向にある一定のスピード以上で走っていると砂埃を出す。
            if (Mathf.Abs(rigid2d.velocity.x) > status.speed * 0.6
                && States.fullPathHash == Animator.StringToHash("Base Layer.Walking")
                && States.normalizedTime >= 1
                && Mathf.RoundToInt(States.normalizedTime * 10) % 10 == 0
                && OnGround
                )
            {
                Instantiate(Sound.RunDust, transform.position + (Vector3)
                (PlayerLDEdge + PlayerRDEdge) + new Vector3(0f, 0.01f), Quaternion.Euler(Vector3.zero));
            }

            //攻撃時プレイヤーは攻撃モーションを出す。
            if (Input.GetButtonUp("Magic"))
            {
                Animations.SetBool("IsAttack", true);
            }
            else {
                Animations.SetBool("IsAttack", false);
            }

            if ((Damaged == true) || (status.InvisTime > 0))
            {
                Damaged = false;
                status.InvisTime += Time.deltaTime;
                SpriteMat.SetColor("_Color", new Color
                    (1
                    ,1 - Mathf.CeilToInt(status.InvisTime * 100) % 2
                    ,1 - Mathf.CeilToInt(status.InvisTime * 100) % 2));
                SpriteMat.SetFloat("_Val", 3f);
            }
            if (status.InvisTime >= status.InvisMaxTime)
            {
                status.InvisTime = 0;
                SpriteMat.SetColor("_Color", new Color(1,1,1));
                SpriteMat.SetFloat("_Val", 1f);
            }

            Debug.DrawLine(transform.position, transform.position + (Vector3)rigid2d.velocity, Color.cyan);
            yield return null;
        }
    }
    

    bool TerrainNearby(Vector2 pos1, Vector2 Direction, int layerMask)
    {
        Ray2D ray1 = new Ray2D();
        ray1.direction = Direction;
        ray1.origin = (Vector2)transform.position + pos1;
        RaycastHit2D Wall_1 = Physics2D.Raycast(ray1.origin, ray1.direction, 100f, layerMask);
        if ((Wall_1.distance > 0.08f || Wall_1.collider == null))
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
