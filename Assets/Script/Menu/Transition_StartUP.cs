using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_StartUP : MonoBehaviour {

    Fade fade;
    public float trasitiontime = 1f;
    

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator Fadingin() {
        Debug.Log("Start");
        Time.timeScale = 0;
        fade = GetComponent<Fade>();
        fade.FadeIn(0f, false);
        fade.FadeOut(trasitiontime, false);
        Time.timeScale = 1f;
        yield return null;
    }

    IEnumerator FadingOut()
    {
        Debug.Log("Start");
        Time.timeScale = 0;
        fade = GetComponent<Fade>();
        fade.FadeOut(0f,false);
        fade.FadeIn(trasitiontime,false);
        Time.timeScale = 1f;
        yield return null;
    }
}
