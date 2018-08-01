using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking_1 : MonoBehaviour {



    public enum AttackType{
        Collisions,
        Shooting
    }
    public AttackType Type;
    public GameObject[] Shot;

    EnemyStatus status;
    Animator animator;
    Rigidbody2D rigidbody2D;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Type == AttackType.Collisions) {
            PlayerMoving player = collision.gameObject.GetComponent<PlayerMoving>();
            player.Damaged = true;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += GetComponent<Rigidbody2D>().velocity * 3;
        }
    }

    // Use this for initialization
    void Start () {
        status = GetComponent<EnemyStatus>();
        animator = GetComponent<Animator>();
        switch (Type)
        {
            case AttackType.Collisions:
                break;
            case AttackType.Shooting:
                StartCoroutine("A_Shooting");
                break;
            default:
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator A_Shooting()
    {
        float shoot = 0;
        while (Application.isPlaying)
        {
            shoot += Time.deltaTime;
            if (shoot > status.status.AttackSpeed && Shot != null) {
                shoot = 0;
                Instantiate(Shot[0],transform.position,Quaternion.Euler(0,0,0));
                animator.SetBool("IsAttack",true);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
            yield return null;
        }
    }

}
