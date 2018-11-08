using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeSettles : MonoBehaviour {
    public GameObject Gauges;
    public float Amount;

    Image image;
	// Use this for initialization
	void Start () {
        image = Gauges.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        image.fillAmount = Amount;
	}
}
