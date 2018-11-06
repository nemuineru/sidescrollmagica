using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusComp : MonoBehaviour {

    public GameObject HP, MP, TargetedEnemyHP, TargetedEnemyHpText;
    public GameObject Player, TargetedEnemy, EnemyLifeBar_Hontai;
    Text text;
    PlayerMoving PlayerStatus;

    void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<PlayerMoving>();
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<PlayerMoving>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        HP.GetComponent<Image>().fillAmount =
        (float)PlayerStatus.status.Life / PlayerStatus.status.LifeMax;
        text.text = PlayerStatus.status.Life + "/" +
            PlayerStatus.status.LifeMax;
        float type_SP, type_EX;
        if (PlayerStatus.status.castingTime / PlayerStatus.status.CastTime_SP > 1)
        {
            type_SP = 0.5f;
            type_EX = (PlayerStatus.status.castingTime - PlayerStatus.status.CastTime_SP) /
                PlayerStatus.status.CastTime_EX * 0.5f;
        }
        else
        {
            type_SP = PlayerStatus.status.castingTime /
                PlayerStatus.status.CastTime_SP * 0.5f;
            type_EX = 0;
        }
        MP.GetComponent<Image>().fillAmount = type_SP + type_EX;

        if (TargetedEnemy != null)
        {
            EnemyLifeBar_Hontai.SetActive(true);
            EnemyStatus EStatus = TargetedEnemy.GetComponent<EnemyStatus>();
            TargetedEnemyHpText.GetComponent<Text>().text =
                Mathf.FloorToInt(EStatus.status.hp * 100 / EStatus.status.Maxhp) + "."+ Mathf.FloorToInt((EStatus.status.hp * 1000 / EStatus.status.Maxhp) % 10) + "%" ;
            TargetedEnemyHP.GetComponent<Image>().fillAmount =
                EStatus.status.hp / EStatus.status.Maxhp;
        }
        else {
            EnemyLifeBar_Hontai.SetActive(false);
        }
    }
}
