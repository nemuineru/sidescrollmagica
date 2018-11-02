using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEventScript_Child : MonoBehaviour {

    public enum Type {
        loadscene,
        loadsaves,
        loadmenu,
        stop
    };

    public string menuname;
    public Type type;
    public UnityEngine.SceneManagement.Scene scene;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
