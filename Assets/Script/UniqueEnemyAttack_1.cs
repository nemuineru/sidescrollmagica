using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEnemyAttack_1 : MonoBehaviour {

    public enum AttackType
    {
        Golems
    }
    public AttackType Type;
    public GameObject[] Things;
    public GameObject[] Shot;

    EnemyStatus status;
    Animator animator;
    Rigidbody2D rigidbody2D;

    GameObject Player;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
        switch (Type) { 
        case AttackType.Golems:
                StartCoroutine("AtkType_Golem");
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator AtkType_Golem() {

        yield return null;
    }

}
