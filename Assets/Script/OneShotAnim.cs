using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent((typeof(AudioSource)),(typeof(Animator)),(typeof(SpriteRenderer)))]
public class OneShotAnim : MonoBehaviour {
    public AudioClip Audio;
    AudioSource Source;
    Animator Anims;
    // Use this for initialization
    void Start () {
        Anims = GetComponent<Animator>();
        Source = GetComponent<AudioSource>();
        Source.clip = Audio;
        Source.Play();
	}
	
	// Update is called once per frame
	void Update ()
    {
        AnimatorStateInfo States = Anims.GetCurrentAnimatorStateInfo(0);
        if (States.normalizedTime >= 1f && !Source.isPlaying && !Source.isPlaying) {
            Destroy(gameObject);
        }
	}
}
