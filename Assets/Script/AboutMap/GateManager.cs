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
        yield return fade.FadeIn(0.5f,false);
        if (NextUp != null) //次ステージ形成｡
        {
            stageScript.DestroyCurrentMap();
            stageScript.nextupMap = NextUp.Map;
            stageScript.TransitTo = TransitTo;
            stageScript.InstantiateNextMap();
            yield return new WaitForSecondsRealtime(1.5f);
            Debug.Log("B");
        }
        Time.timeScale = 1;
        Debug.Log("C");
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("D");
        yield return fade.FadeOut(1.5f, false);
    }
    
}