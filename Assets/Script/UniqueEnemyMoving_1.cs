using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEnemyMoving_1 : MonoBehaviour {

    public enum MoveType
    {
        Golems
    }
    public MoveType Type;
    public GameObject[] Things;
    EnemyStatus status;
    Animator animator;
    float ThinkTime = 0;


    Rigidbody2D rigidbody2D;

    GameObject Player;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        rigidbody2D = GetComponent<Rigidbody2D>();
        switch (Type)
        {
            case MoveType.Golems:
                StartCoroutine("MovType_Golem");
                break;
            default:
                break;
        }
        StartCoroutine("MovingScript");
    }
	
	// Update is called once per frame
	void Update () {

    }

    enum AI {
        Attack,
        Move,
        Nothing
    }

    IEnumerator MovType_Golem()
    {
        AI ai = AI.Nothing;
        while (Application.isPlaying)
        {
            if (ThinkTime > 0.5f)
            {
                if (Mathf.Abs(Vector2.Distance
                    (transform.position, Player.transform.position)) > 2.0
                    && Random.Range(0, 100) < 3)
                {
                    ai = AI.Move;
                    ThinkTime = 0;
                }
            else
                ai = AI.Nothing;
            }
            switch (ai){
                case AI.Move:
                    rigidbody2D.velocity = Vector2.right *
                        Mathf.Sign(Player.transform.position.x - transform.position.x);
                    Debug.Log("Moving");
                    break;
                default:
                    rigidbody2D.velocity = Vector2.zero;
                    break;
            }
            ThinkTime += Time.deltaTime;
            
            yield return null;
        }
    }

    IEnumerator MovingScript() {
        while (Application.isPlaying)
        {
            transform.localScale = new Vector2(1 * Mathf.Sign(rigidbody2D.velocity.x),1);
            if (Mathf.Abs(rigidbody2D.velocity.x) > 0) {
                animator.SetBool("IsMoving", true);
            }
            else
                animator.SetBool("IsMoving", false);
            yield return null;
        }
    }
}
