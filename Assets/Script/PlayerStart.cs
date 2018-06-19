using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour {

    Camera MainCamera;
	// Use this for initialization
	void Start () {
        GameObject Player = GameObject.Find("Player");
        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        MainCamera.GetComponent<CameraToScript>().MainPos = transform.position;
        Player.transform.position = transform.position;
        MainCamera.transform.position = transform.position;
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
