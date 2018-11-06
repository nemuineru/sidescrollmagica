using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitComp_RefTo : MonoBehaviour {

    public EnemyStatus Status;
    public GameObject RefObj;
    // Use this for initialization

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Status != null)
        {
            
        }
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
