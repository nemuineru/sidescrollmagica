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
        GlobalSC = GameObject.Find("Score");
         Audi = GameObject.Find("Score").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        float size_x = GetComponent<BoxCollider2D>().size.x;
        float size_y = GetComponent<BoxCollider2D>().size.x;
        LayerMask Mask = LayerMask.GetMask("Player");
        if (TerrainNearby(new Vector2(size_x, 0),Vector2.right,Mask.value) ||
            TerrainNearby(new Vector2(-size_x, 0), Vector2.left, Mask.value)||
            TerrainNearby(new Vector2(0, size_y), Vector2.up, Mask.value) ||
            TerrainNearby(new Vector2(0, -size_y), Vector2.down, Mask.value))
        {
            if (Audios != null) {
                Debug.Log("Get");
                Audi.clip = Audios;
                Audi.Play();
            }
            Destroy(gameObject);
        }
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
