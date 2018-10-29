using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToScript : MonoBehaviour {

    GameObject Player;
    public Vector2 TransPos, MainPos, CamShift;
    public Vector2 PLScrlDeadzone;
    public Vector2Int WorldSize;
    public float transitionVel , PlV_transitvel;
    public Sprite Basesprite;
    
    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
    }

    Vector2 PlayerPos, PlayerVector, ActualPos;
    Vector2 ScreenLD, ScreenRU, WorldBoun_LD, WorldBoun_RU, ScreenWPos_LD, ScreenWPos_RU;

    // Update is called once per frame
    void Update () {

        //プレイヤーの速度と位置、スクリーン位置を確認
        PlayerPos = Player.transform.position;
        PlayerVector = Player.GetComponent<Rigidbody2D>().velocity / Camera.main.orthographicSize;
        ActualPos = (Vector2)transform.position - CamShift;

        //スクリーン位置に合わせメインポジションを変更
        if (Mathf.Abs(PlayerPos.x - ActualPos.x) > PLScrlDeadzone.x * Camera.main.orthographicSize
            || Mathf.Abs(PlayerPos.y - ActualPos.y) > PLScrlDeadzone.y * Camera.main.orthographicSize)
        {
            TransPos += PlayerVector * Time.deltaTime * transitionVel * PlV_transitvel;
        }
        TransPos += (PlayerPos - (Vector2)transform.position + CamShift) * Time.deltaTime * transitionVel;
        MainPos = CamShift + TransPos;


        //もしステージからカメラがはみ出るときそれを止める。
        if (Camera.main.rect.size.x * 2 * Camera.main.aspect >
            (WorldSize.x) / 2 * Basesprite.bounds.size.x) {
            MainPos.x = CamShift.x;
        }
        if (Camera.main.rect.size.y * 2 > (WorldSize.y) / 2 * Basesprite.bounds.size.y)
        {
            MainPos.y = CamShift.y;
        }
        ScreenLD =
            new Vector2(-Camera.main.rect.size.x * Camera.main.aspect,
            -Camera.main.rect.size.y) * Camera.main.orthographicSize;
         ScreenRU =
            new Vector2(Camera.main.rect.size.x * Camera.main.aspect,
           Camera.main.rect.size.y) * Camera.main.orthographicSize;
         WorldBoun_LD =
            new Vector2(-(WorldSize.x + 0.8f) / 2 * Basesprite.bounds.size.x,
           -(WorldSize.y - 1.2f) / 2 * Basesprite.bounds.size.y);
         WorldBoun_RU =
            new Vector2((WorldSize.x - 1.2f) / 2 * Basesprite.bounds.size.x,
           (WorldSize.y + 0.8f) / 2 * Basesprite.bounds.size.y);

         ScreenWPos_LD =
            ScreenLD + MainPos;
         ScreenWPos_RU =
            ScreenRU + MainPos;

        //カメラの移動でステージから外れないようにする｡
        

        if (ScreenWPos_LD.x < WorldBoun_LD.x )
        {
            MainPos.x = (WorldBoun_LD.x - (ScreenLD.x - ScreenRU.x)/2);
            TransPos.x = (WorldBoun_LD.x - (ScreenLD.x - ScreenRU.x) / 2) - CamShift.x;
        }
        if (ScreenWPos_RU.x > WorldBoun_RU.x)
        {
            MainPos.x = (WorldBoun_RU.x + (ScreenLD.x - ScreenRU.x) / 2);
            TransPos.x = (WorldBoun_RU.x + (ScreenLD.x - ScreenRU.x) / 2) - CamShift.x;
        }
        if (ScreenWPos_LD.y < WorldBoun_LD.y)
        {
            MainPos.y = (WorldBoun_LD.y - (ScreenLD.y - ScreenRU.y) / 2);
            TransPos.y = (WorldBoun_LD.y - (ScreenLD.y - ScreenRU.y) / 2) - CamShift.y;
        }
        if (ScreenWPos_RU.y > WorldBoun_RU.y)
        {
            MainPos.y = (WorldBoun_RU.y + (ScreenLD.y - ScreenRU.y) / 2);
            TransPos.y = (WorldBoun_RU.y + (ScreenLD.y - ScreenRU.y) / 2) - CamShift.y;
        }

        Debug.DrawLine(ScreenWPos_LD,ScreenWPos_RU,new Color(1f,0.8f,0.5f)); //スクリーン移動位置
        Debug.DrawLine(WorldBoun_LD, WorldBoun_RU, new Color(0.8f, 1f, 0.5f)); //ワールド位置
        
        //実際に代入。
        transform.position = (Vector3)MainPos + new Vector3(0, 0, -10);
    }

    public void CameraInstaTransit(Vector2Int TransTo, Vector2 WorldSize)
    {
        MainPos = new Vector2((TransTo.x - WorldSize.x / 2f) * Basesprite.bounds.size.x,
                          (WorldSize.y / 2f - TransTo.y) * Basesprite.bounds.size.y);
        TransPos = new Vector2((TransTo.x - WorldSize.x / 2f) * Basesprite.bounds.size.x,
                          (WorldSize.y / 2f - TransTo.y) * Basesprite.bounds.size.y);
        transform.position = (Vector3)MainPos + new Vector3(0, 0, -10);
        Debug.Log(transform.position);
    }
}
