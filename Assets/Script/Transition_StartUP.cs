using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_StartUP : MonoBehaviour {

    Fade fade;
    public float trasitiontime = 1f;

    void OnActiveSceneChanged(Scene scenePrev, Scene sceneNext)
    {
        StartCoroutine("Fadingin");
    }
    void Awake() {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged +=
            OnActiveSceneChanged;
        StartCoroutine("FadingOut");
    }

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
        fade.FadeIn(0f);
        fade.FadeOut(trasitiontime);
        Time.timeScale = 1f;
        yield return null;
    }

    IEnumerator FadingOut()
    {
        Debug.Log("Start");
        Time.timeScale = 0;
        fade = GetComponent<Fade>();
        fade.FadeOut(0f);
        fade.FadeIn(trasitiontime);
        Time.timeScale = 1f;
        yield return null;
    }
}
