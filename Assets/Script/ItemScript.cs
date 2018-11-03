using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour {
    public int score;
    public AudioClip Audios;

    GameObject GlobalSC;
    AudioSource Audi;
    // Use this for initialization
    void Start () {
        GlobalSC = GameObject.Find("Status");
         Audi = GameObject.Find("Status").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Audios != null)
            {
                GameObject.Find("BaseItems").GetComponent<BaseScoreItems>().CurrentCoin += score;
                Audi.clip = Audios;
                Audi.Play();
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    bool TerrainNearby(Vector2 pos1, Vector2 Direction, int layerMask)
    {
        Ray2D ray1 = new Ray2D();
        ray1.direction = Direction;
        ray1.origin = (Vector2)transform.position + pos1;
        RaycastHit2D Wall_1 = Physics2D.Raycast(ray1.origin, ray1.direction, 100f, layerMask);
        if ((Wall_1.distance > 0.08f || Wall_1.collider == null))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
