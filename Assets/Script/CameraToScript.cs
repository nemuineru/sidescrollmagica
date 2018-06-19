using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToScript : MonoBehaviour {

    GameObject Player;
    public Vector2 MainPos;
    public Vector2Int WorldSize;
    public Sprite Basesprite;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update () {
        //プレイヤーの速度と位置、スクリーン位置を確認
        Vector2 PlayerPos = Player.transform.position;
        Vector2 PlayerVector = Player.GetComponent<Rigidbody2D>().velocity/Camera.main.orthographicSize;
        Vector2 PlayerScreenPos = Camera.main.WorldToViewportPoint(
            new Vector2((PlayerPos.x - Camera.main.rect.size.x * 2 * Camera.main.aspect) , (PlayerPos.y - Camera.main.rect.size.y * 2)));


        //スクリーン位置に合わせメインポジションを変更
        MainPos += (PlayerVector / 25 + PlayerScreenPos / 20) * Time.timeScale;

        //もしステージからカメラがはみ出るときそれを止める。
        Vector2 ScreenWPos_LD = 
            new Vector2(MainPos.x - Camera.main.rect.size.x * 2 * Camera.main.aspect, 
            MainPos.y - Camera.main.rect.size.y * 2);
        Vector2 ScreenWPos_RU = 
            new Vector2(MainPos.x + Camera.main.rect.size.x * 2 * Camera.main.aspect,
            MainPos.y + Camera.main.rect.size.y * 2);
        if (ScreenWPos_LD.x + ((WorldSize.x - 1) / 2) * Basesprite.bounds.size.x < 0 ||
            ScreenWPos_RU.x - ((WorldSize.x - 1) / 2) * Basesprite.bounds.size.x > 0) {
            MainPos.x = ((WorldSize.x - 1) / 2 * Basesprite.bounds.size.x -
                Camera.main.rect.size.x * 2 * Camera.main.aspect) * Mathf.Sign(MainPos.x);
        }
        if (ScreenWPos_LD.y + ((WorldSize.y - 1) / 2) * Basesprite.bounds.size.y < 0 ||
            ScreenWPos_RU.y - ((WorldSize.y - 1) / 2) * Basesprite.bounds.size.y > 0)
        {
            MainPos.y = ((WorldSize.y - 1) / 2 * Basesprite.bounds.size.y -
                Camera.main.rect.size.y * 2) * Mathf.Sign(MainPos.y);
        }
        if (Camera.main.rect.size.x * 2 * Camera.main.aspect >
            (WorldSize.x - 1) / 2 * Basesprite.bounds.size.x) {
            MainPos.x = 0;
        }
        if (Camera.main.rect.size.y * 2 > (WorldSize.y - 1) / 2 * Basesprite.bounds.size.y)
        {
            MainPos.y = 0;
        }

        //実際に代入。
        transform.position = (Vector3)MainPos + new Vector3(0,0,-5);
    }
}
