using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEnemyAttack_1 : MonoBehaviour {


    public BoxCollider2D[] Colliders; 


    EnemyStatus status;
    Animator animator;
    Rigidbody2D rigidbody2D;

    GameObject Player;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator AtkType_Golem() {

        yield return null;
    }

    

}
