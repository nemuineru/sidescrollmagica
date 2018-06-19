using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class BackGroundToCamera : MonoBehaviour {

    public Sprite BGsprite;
    public Vector2 sizeMultiply;
    public Vector2 Shift;
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
        Vector2 BGPos =
            new Vector2(-(mainCamera.transform.position.x) / ((cameraToScript.WorldSize.x) / 2) * cameraToScript.Basesprite.bounds.size.x,
            -(mainCamera.transform.position.y) / (cameraToScript.WorldSize.y) / 2 * cameraToScript.Basesprite.bounds.size.y);
        BGPos = BGPos * renderer.bounds.size * 4 * transform.localScale;
        //もしカメラが背景からはみ出るときそれを止める。
        Vector2 ScreenWPos_LD =
            new Vector2(cameraToScript.MainPos.x - Camera.main.rect.size.x * 
            2 * Camera.main.aspect,
            cameraToScript.MainPos.y - Camera.main.rect.size.y * 2);
        Vector2 ScreenWPos_RU =
            new Vector2(cameraToScript.MainPos.x + Camera.main.rect.size.x *
            2 * Camera.main.aspect,
            cameraToScript.MainPos.y + Camera.main.rect.size.y * 2);



        if (isStableOnCamera)
        {
            transform.position = mainCamera.transform.position;
        }
        else {
            transform.position = (Vector2)mainCamera.transform.position + BGPos + Shift;
        }
	}
}
