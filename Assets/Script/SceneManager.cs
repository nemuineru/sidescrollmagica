using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D),typeof(AudioSource))]
public class SceneManager : MonoBehaviour {

    public GameObject FadingObject;
    public string NextStageName;
    public Vector2Int ConnectsTo;
    public AudioClip EnteringSound;

    [SerializeField]
    Fade fade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            fade = GameObject.Find("ViewTransit").GetComponent<Fade>();
            StartCoroutine("ExitAndEnter");
        }
    }

    AudioSource Audio;

    // Use this for initialization
    void Start () {
        Audio =GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ExitAndEnter() {
        if (EnteringSound != null)
        {
            Audio.clip = EnteringSound;
            Audio.Play();
        }
        Time.timeScale = 0;
        fade.FadeIn(2f);
        yield return new WaitForSecondsRealtime(2f);
        if (NextStageName != null && SceneUtility.GetBuildIndexByScenePath(NextStageName) != -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextStageName);
            fade.FadeOut(2f);
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1;
        }
        else
        {
            fade.FadeOut(2f);
           yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1;
        }
        yield return null;
    }
}
