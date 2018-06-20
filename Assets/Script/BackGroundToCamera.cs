using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class BackGroundToCamera : MonoBehaviour {

    public Sprite BGsprite;
    public Vector2 MainPos , sizeMultiply = new Vector2(1, 1), Shift;
    public Vector2 TransMultiply = new Vector2(1f,1f);
    public bool isStableOnCamera;
    SpriteRenderer renderer;
    Camera mainCamera;
    CameraToScript cameraToScript;
    // Use this for initialization
    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = BGsprite;
        mainCamera = Camera.main;
        cameraToScript = mainCamera.GetComponent<CameraToScript>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = sizeMultiply;
        //移動先はカメラの位置がマップ座標のどこに位置しているかで決まる。
        //メインカメラのポジションを差し引いて中心からどのぐらい離れているかを調べる｡
        Vector2 BGPos =
            ((Vector2.one / 2 - (Vector2)mainCamera.transform.position) /
            (cameraToScript.Basesprite.bounds.size)) / cameraToScript.WorldSize * 2;
        Debug.Log(BGPos.ToString());

        //BGPosの動作範囲はワールド上のスプライトのサイズ - maincameraのサイズになる｡
        //もしカメラが背景からはみ出るときそれを止める。
        Vector2 ScreenWPos_LD =
            new Vector2(cameraToScript.MainPos.x - mainCamera.rect.size.x * 
            2 * mainCamera.aspect,
            cameraToScript.MainPos.y - mainCamera.rect.size.y * 4);
        Vector2 ScreenWPos_RU =
            new Vector2(cameraToScript.MainPos.x + mainCamera.rect.size.x *
            2 * mainCamera.aspect,
            cameraToScript.MainPos.y + mainCamera.rect.size.y * 4);

        Vector2 TransitSize = new Vector2
            (renderer.sprite.bounds.size.x * transform.localScale.x - mainCamera.rect.size.x * 4 * mainCamera.aspect,
            renderer.sprite.bounds.size.y * transform.localScale.y - mainCamera.rect.size.y * 4);

        MainPos = BGPos * TransitSize * TransMultiply / 2 + Shift;

        if (isStableOnCamera)
        {
            transform.position = mainCamera.transform.position;
        }
        else {
            transform.position = (Vector2)mainCamera.transform.position + MainPos;
        }
	}
}
