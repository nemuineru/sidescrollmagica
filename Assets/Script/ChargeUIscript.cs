using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeUIscript : MonoBehaviour {

    [SerializeField]
    GameObject gaugeObject,Text;
    GameObject Player;

	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
	}

    // Update is called once per frame
    void Update() {
        PlayerMoving moving = Player.GetComponent<PlayerMoving>();
        float Bpl = moving.BombPressed;
        if (Bpl > 0)
        {
            gaugeObject.GetComponent<Image>().fillAmount = Mathf.Clamp01(Bpl / moving.status.BombTime_SP) * 0.5f
                + Mathf.Clamp01((Bpl - moving.status.BombTime_SP) / (moving.status.BombTime_EX - moving.status.BombTime_SP)) * 0.5f;
        }
        gaugeObject.transform.parent.gameObject.SetActive(moving.bombpressed);
        Text.SetActive(moving.bombpressed);
        if (Bpl < moving.status.BombTime_SP) {
            Text.GetComponent<Text>().text = "LV1";
        }
        else if (Bpl < moving.status.BombTime_EX)
        {
            Text.GetComponent<Text>().text = "LV2";
        }
        else
        {
            Text.GetComponent<Text>().text = "LV3";
        }

            GetComponent<RectTransform>().position
            = RectTransformUtility.WorldToScreenPoint(Camera.main, Player.transform.position);
	}
}
