using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusComp : MonoBehaviour {

    public GameObject HP, MP;
    GameObject Player;
    Text text;
    PlayerMoving PlayerStatus;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<PlayerMoving>();
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        HP.GetComponent<Image>().fillAmount =
        (float)PlayerStatus.status.Life / PlayerStatus.status.LifeMax;
        text.text = PlayerStatus.status.Life + "/" +
            PlayerStatus.status.LifeMax;
        float type_SP, type_EX;
        if (PlayerStatus.status.castingTime / PlayerStatus.status.CastTime_SP > 1) {
            type_SP = 0.5f;
            type_EX = (PlayerStatus.status.castingTime - PlayerStatus.status.CastTime_SP) / 
                PlayerStatus.status.CastTime_EX * 0.5f;
        }
        else {
            type_SP = PlayerStatus.status.castingTime / 
                PlayerStatus.status.CastTime_SP * 0.5f;
            type_EX = 0;
        }
        MP.GetComponent<Image>().fillAmount = type_SP + type_EX; 
            }
}
