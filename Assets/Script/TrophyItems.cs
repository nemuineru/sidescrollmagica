using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyItems : MonoBehaviour {

    [SerializeField]
    GameObject ShowClear;

    public AudioClip GetSound;

    AudioSource sourceAudio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sourceAudio = gameObject.AddComponent<AudioSource>();
        StartCoroutine(Clear());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   private IEnumerator Clear()
    {
        bool EventEnd = false;
        while (!EventEnd)
        {
            sourceAudio.volume = 0.3f;
            sourceAudio.PlayOneShot(GetSound);
            Time.timeScale = 0.01f;
            Debug.Log("OK");
            GameObject Shows = Instantiate(ShowClear);
            Shows.name = "ShowClear";
            EventEnd = true;
            yield return null;
        }
    }
}
