using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_StartUP : MonoBehaviour {

    Fade fade;
    public float trasitiontime = 1f;

    void OnActiveSceneChanged(Scene scenePrev, Scene sceneNext)
    {
        StartCoroutine("FadingStart");
    }

	// Use this for initialization
	void Start ()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += 
            OnActiveSceneChanged;
        StartCoroutine("FadingStart");
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator FadingStart() {
        Debug.Log("Start");
        Time.timeScale = 0;
        fade = GetComponent<Fade>();
        fade.FadeIn(0f);
        fade.FadeOut(trasitiontime);
        Time.timeScale = 1f;
        yield return null;
    }
}
