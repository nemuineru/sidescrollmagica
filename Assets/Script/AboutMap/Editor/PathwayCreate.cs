using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathwayCreate : MonoBehaviour {


    public Vector2[] PathPoint;
    // Use this for initialization
	void Start () {
		
	}

    string text1,text2;
    void OnGUI()
    {
       text2 = GUI.TextArea(new Rect(200,10,50,4),text1);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
