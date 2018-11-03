using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBorders : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StageScript stage = GameObject.Find("Systems").GetComponent<StageScript>();
            
            collision.transform.position =
                               new Vector2((stage.TransitTo.x - stage.MapLength / 2f) * stage.baseSprite.bounds.size.x,
                               (stage.MapHeight / 2f - stage.TransitTo.y) * stage.baseSprite.bounds.size.y);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
