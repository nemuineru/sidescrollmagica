using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateManager : MonoBehaviour {

    
    public StageComp NextUp;
    public Vector2Int TransitTo;
    public AudioClip EnteringSound;
    public bool isContract = false;

    GameObject Systems;
    StageScript stageScript;

    [SerializeField]
    Fade fade;
    
    AudioSource Audio;

    public IEnumerator ExitAndEnter()
    {
        Systems = GameObject.Find("Systems");
        stageScript = Systems.GetComponent<StageScript>();
        Audio = GetComponent<AudioSource>();
        Time.timeScale = 0;
        if (EnteringSound != null)　//侵入時の音
            {
                Audio.clip = EnteringSound;
                Audio.Play();
        }
            yield return StartCoroutine(WaitFadeIn(0.1f, 0.5f));
        if (NextUp != null) //次ステージ形成｡
        {
            Debug.Log("A");
            stageScript.nextComp = NextUp;
            stageScript.TransitTo = TransitTo;
            stageScript.DestroyCurrentMap();
            stageScript.InstantiateNextMap();
            Debug.Log("A'");
            Time.timeScale = 1;
            Debug.Log("C");
        }
        yield return StartCoroutine(WaitFadeOut(1f, 0.5f));
        Debug.Log("C'");
        yield return null;
    }


    public IEnumerator WaitFadeIn(float WaitRealtime, float FadeIntime) {
        yield return fade.FadeIn(FadeIntime, false);
        Debug.Log("B");
        yield return new WaitForSecondsRealtime(WaitRealtime);
        Debug.Log("B'");
    }
    public IEnumerator WaitFadeOut(float WaitRealtime, float Fadeouttime)
    {
        yield return new WaitForSecondsRealtime(WaitRealtime);
        Debug.Log("D");
        yield return fade.FadeOut(Fadeouttime, false);
        Debug.Log("D'");
    }


}