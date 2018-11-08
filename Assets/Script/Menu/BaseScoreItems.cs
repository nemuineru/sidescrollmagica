using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScoreItems : MonoBehaviour {

    public int CurrentCoin;
    public GameObject Coin_1, Coin_5, Coin_10, Coin_50, Coin_100, Coin_500;
    Text UIText;

    // Use this for initialization
    void Start () {
        UIText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        UIText.text = CurrentCoin.ToString();
	}
}
