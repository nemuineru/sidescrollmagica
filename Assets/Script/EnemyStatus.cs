using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {
    public Status status;
    [System.Serializable]
    public class Status {
        public int hp, coins;
        public float MoveSpeed, AttackSpeed;
        public GameObject[] Itemcontain;
        public bool IsNotDropMoney;
    }
    BaseScoreItems BaseItem;



    int C500, C100, C50, C10, C5, C1;
	// Use this for initialization
	void Start () {
        if(GameObject.Find("BaseItems")  != null)
        BaseItem = GameObject.Find("BaseItems").GetComponent<BaseScoreItems>();
	}
	
	// Update is called once per frame
	void Update () {
        if (status.hp <= 0) {
            if (status.Itemcontain != null)
                for (int i = 0; i < status.Itemcontain.Length; i++) {
                    GameObject Items = Instantiate(status.Itemcontain[i]
                        ,transform.position,Quaternion.Euler(0,0,0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f,3f), Random.Range(0,3f));
                }

                if (status.IsNotDropMoney == false)
                    {
                C500 = Mathf.CeilToInt(status.coins / 500 / 2);
                C100 = Mathf.CeilToInt((status.coins - C500 * 500) / 100 / 1.5f);
                C50 = Mathf.CeilToInt((status.coins - C500 * 500 - C100 * 100) / 50 / 1.3f);
                C10 = Mathf.CeilToInt((status.coins - C500 * 500 - C100 * 100 - C50 * 50) / 10 / 1.2f);
                C5 = Mathf.CeilToInt((status.coins - C500 * 500 - C100 * 100 - C50 * 50 - C10 * 10) / 5 / 1.1f );
                C1 = (status.coins - C500 * 500 - C100 * 100 - C50 * 50 - C10 * 10 - C5 * 5);




                for (int i = 0; i < C500; i++) {
                    GameObject Items = Instantiate(BaseItem.Coin_1
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(4f, 6f));
                }
                for (int i = 0; i < C100; i++)
                {
                    GameObject Items = Instantiate(BaseItem.Coin_5
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(3f, 6f));
                }
                for (int i = 0; i < C50; i++)
                {
                    GameObject Items = Instantiate(BaseItem.Coin_10
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 5f));
                }
                for (int i = 0; i < C10; i++)
                {
                    GameObject Items = Instantiate(BaseItem.Coin_50
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(1f, 4f));
                }
                for (int i = 0; i < C5; i++)
                {
                    GameObject Items = Instantiate(BaseItem.Coin_100
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(0f, 3f));
                }
                for (int i = 0; i < C1; i++)
                {
                    GameObject Items = Instantiate(BaseItem.Coin_500
                        , transform.position, Quaternion.Euler(0, 0, 0));
                    Items.GetComponent<Rigidbody2D>().velocity =
                        new Vector2(Random.Range(-3f, 3f), Random.Range(0f, 2f));
                }

            }
                Destroy(gameObject);
        }
	}
}
