using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_StartUP : MonoBehaviour {

    Fade fade;
    public float trasitiontime = 1f; 


	// Use this for initialization
	void Start ()
    {
        Debug.Log("Start");
        Time.timeScale = 0;
        fade = GetComponent<Fade>();
        fade.FadeIn(0f);
        fade.FadeOut(trasitiontime);
        Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
