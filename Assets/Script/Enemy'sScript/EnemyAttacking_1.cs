using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking_1 : MonoBehaviour {

    public enum AttackType{
        Collisions,
        Shooting
    }
    public AttackType Type;
    public GameObject Shot;
    public int DamageNum, Impact; 

    EnemyStatus status;
    Animator animator;
    Rigidbody2D rigid2D;

    public BoxCollider2D[] Colliders;
    public ContactFilter2D filter2D;
    Collider2D[] result;

    
    // Use this for initialization
    void Start () {
        result = new Collider2D[10];
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
        foreach (BoxCollider2D Collider in Colliders)
        {
            result = new Collider2D[10];
            if (Collider != null)
            {
                int I = Collider.OverlapCollider(filter2D, result);
            }
            foreach (Collider2D ColResult in result)
            {
                if (ColResult != null) {
                    if (ColResult.gameObject.tag == "Player")
                    {
                        Damaging(ColResult.transform.root.gameObject);
                        Debug.Log("Hit");
                        break;
                    }
                }
            }
        }
    }


    IEnumerator A_Shooting()
    {
        float shoot = 0;
        while (Application.isPlaying)
        {
            shoot += Time.deltaTime;
            if (shoot > status.status.AttackSpeed && Shot != null) {
                shoot = 0;
                Instantiate(Shot,transform.position,Quaternion.Euler(0,0,0));
                animator.SetBool("IsAttack",true);
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
            yield return null;
        }
    }

    public void Damaging(GameObject Player)
    {
        PlayerMoving player = Player.GetComponent<PlayerMoving>();
        if (player.status.InvisTime == 0)
        {
            player.Damaged = true;
            player.status.Life -= DamageNum;
            Player.gameObject.GetComponent<Rigidbody2D>().velocity =
                Mathf.Sign(Player.transform.localScale.x) * Vector2.left * Impact * 4
                +
                Vector2.up * Impact * 2;
         }
    }
}
