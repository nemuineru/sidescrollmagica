﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackGroundToCamera : MonoBehaviour
{

    Vector2 MainPos;
    [System.Serializable]
    public class backGroundProperty
    {
        public Sprite BGsprite;
        public Vector2 sizeMultiply = new Vector2(1, 1);
        public Vector2 TransMultiply = new Vector2(1f, 1f), Shift;
        public Vector2 MovesTo;
        public bool isBGloops;
        public bool isStableOnCamera;
        public bool isStableOnStage;
    }
    public bool isLoopCreated = false;
    public backGroundProperty BGproperty;
    SpriteRenderer SprRend;
    Camera mainCamera;
    public CameraToScript cameraToScript;

    GameObject[] OtherLoopingBG;
    Coroutine MainCoroutine;
    // Use this for initialization
    void Start()
    {
        OtherLoopingBG = new GameObject[9];
        SprRend = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        Transform parent = transform.parent;
        if (BGproperty.BGsprite != null)
            SprRend.sprite = BGproperty.BGsprite;
        MainCoroutine = StartCoroutine(MainCameraPositions());
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = BGproperty.sizeMultiply;
        if (BGproperty.isBGloops == true && isLoopCreated == false)
        {
                StartCoroutine(CreateLoop());
                isLoopCreated = true;
        }

    }
    
    IEnumerator MainCameraPositions()
    {
        while(Application.isPlaying)
        {
            cameraToScript = mainCamera.GetComponent<CameraToScript>();
            //移動先はカメラの位置がマップ座標のどこに位置しているかで決まる。
            //メインカメラのポジションを差し引いて中心からどのぐらい離れているかを調べる｡
            Vector2 BGPos =
                ((Vector2.one / 2 - (Vector2)mainCamera.transform.position) /
                (cameraToScript.Basesprite.bounds.size)) / cameraToScript.WorldSize * 2;
            //Debug.Log(BGPos.ToString());
            //BGPosの動作範囲はワールド上のスプライトのサイズ - maincameraのサイズになる｡
            //もしカメラが背景からはみ出るときそれを止める。
            Vector2 ScreenWPos_LD =
                new Vector2(cameraToScript.TransPos.x - mainCamera.rect.size.x *
                2 * mainCamera.aspect,
                cameraToScript.TransPos.y - mainCamera.rect.size.y * 2);
            Vector2 ScreenWPos_RU =
                new Vector2(cameraToScript.TransPos.x + mainCamera.rect.size.x *
                2 * mainCamera.aspect,
                cameraToScript.TransPos.y + mainCamera.rect.size.y * 2);
            Debug.DrawLine(ScreenWPos_LD, ScreenWPos_RU,new Color(0.1f, 1f,0.5f));

            Vector2 TransitSize = new Vector2
                (SprRend.sprite.bounds.size.x * transform.localScale.x - mainCamera.rect.size.x * 4 * mainCamera.aspect,
                SprRend.sprite.bounds.size.y * transform.localScale.y - mainCamera.rect.size.y * 4);

            MainPos = BGPos * TransitSize * BGproperty.TransMultiply / 2 + BGproperty.Shift;
            
            //カメラ自体に追従するとき
            if (BGproperty.isStableOnCamera)
            {
                transform.position = mainCamera.transform.position;
            }
            //ステージ上に固定しないとき
            else if(!BGproperty.isStableOnStage)
            {
                transform.position = (Vector2)mainCamera.transform.position + MainPos;
            }
            //ループするとき
            if (BGproperty.isBGloops)
            { 
                transform.position = 
                    new Vector2(-Mathf.Repeat(((Vector2)mainCamera.transform.position + MainPos).x + SprRend.bounds.size.x, SprRend.bounds.size.x ),
                    Mathf.Repeat(((Vector2)mainCamera.transform.position + MainPos).y + SprRend.bounds.size.y, SprRend.bounds.size.y ) - SprRend.bounds.size.y);
            }
            yield return null;
        }
    }

    //ループ用に作る
    IEnumerator CreateLoop()
    {
        if (BGproperty.isBGloops)
        {
            for (int i = 0; i < 9; i++)
            {
                OtherLoopingBG[i] = new GameObject();
                SpriteRenderer LoopSprite = OtherLoopingBG[i].AddComponent<SpriteRenderer>();
                LoopSprite.sprite = SprRend.sprite;
                LoopSprite.sortingOrder = SprRend.sortingOrder;

                OtherLoopingBG[i].transform.parent = transform;
                OtherLoopingBG[i].transform.localScale = Vector3.one;
                OtherLoopingBG[i].transform.position =
                    (Vector2)transform.position + new Vector2
                    (SprRend.bounds.size.x * ((i % 3) - 1),
                     SprRend.bounds.size.y * (Mathf.CeilToInt((i) / 3) - 1));
            }
            Destroy(OtherLoopingBG[4]);
            yield return true;
        }
        else
        {
            Destroy(this);
            yield return false;
        }
    }
}
