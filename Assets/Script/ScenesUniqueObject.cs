using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesUniqueObject : MonoBehaviour {

    public static ScenesUniqueObject Instance
    {
        get; private set;
    }

    // Use this for initialization
    void Awake () {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
