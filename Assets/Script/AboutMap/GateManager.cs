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
            stageScript.nextComp = NextUp;
            stageScript.TransitTo = TransitTo;
            stageScript.DestroyCurrentMap();
            stageScript.InstantiateNextMap();
            Time.timeScale = 1;
        }
        yield return StartCoroutine(WaitFadeOut(1f, 0.5f));
        yield return null;
    }


    public IEnumerator WaitFadeIn(float WaitRealtime, float FadeIntime) {
        yield return fade.FadeIn(FadeIntime, false);
        yield return new WaitForSecondsRealtime(WaitRealtime);
    }
    public IEnumerator WaitFadeOut(float WaitRealtime, float Fadeouttime)
    {
        yield return new WaitForSecondsRealtime(WaitRealtime);
        yield return fade.FadeOut(Fadeouttime, false);
    }


}