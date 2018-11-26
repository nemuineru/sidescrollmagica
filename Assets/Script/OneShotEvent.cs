using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneShotEvent : MonoBehaviour {

    public enum type
    {
        NextScene
    }
    public string Scenename;
    public AudioClip Jingle;
    public float timeMax;

    float timestart = 0f;
    float timenow = 0f;
    // Use this for initialization
    void Start() {
        timestart = Time.realtimeSinceStartup;
        timenow = Time.realtimeSinceStartup;
        GetComponent<AudioSource>().PlayOneShot(Jingle);
    }

    // Update is called once per frame
    void Update () {
        if (timenow - timestart > timeMax || (Jingle && timenow - timestart > Jingle.length))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(Scenename);
        }
        else timenow = Time.realtimeSinceStartup;
		
	}
}
