using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStates : MonoBehaviour {
    [System.Serializable]
    public class Weapon {
        public GameObject Short, Long, Super;
    }
    public Weapon weapon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
