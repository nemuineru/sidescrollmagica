using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGaugeDecreasement : MonoBehaviour {

    public Image GaugeImage;
    Image GaugeImage_Dec;

	// Use this for initialization
	void Start () {
        GaugeImage_Dec = GetComponent<Image>();

    }
    
    private void OnDisable()
    {
        GaugeImage_Dec.fillAmount = 1f;
    }
     // Update is called once per frame
    void Update () {
        if (Mathf.Abs(GaugeImage_Dec.fillAmount - GaugeImage.fillAmount) < 0.001)
        {
            GaugeImage_Dec.fillAmount = GaugeImage.fillAmount;
        }
        else {
            GaugeImage_Dec.fillAmount -= (GaugeImage_Dec.fillAmount - GaugeImage.fillAmount) * Time.deltaTime * 2;
             
        }
	}
}
