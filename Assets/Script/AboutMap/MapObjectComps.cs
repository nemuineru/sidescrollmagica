using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectComps : MonoBehaviour {

    public enum MoveType {
        Straight,
        Circle,
        Accelates
    }
    public MoveType moveType;

    public Vector2 range;
    public Vector2 Position;
    [Range(0, 1f)]
    public float progress;
    public float Speed;

	// Use this for initialization
	void Start () {
        if (moveType == MoveType.Straight)
        {
            StartCoroutine("MoveStraight");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
