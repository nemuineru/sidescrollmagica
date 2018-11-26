using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusComp : MonoBehaviour {

    public GameObject HP, MPPos ,ChargeGauge, MpText,TargetedEnemyHP, TargetedEnemyHpText, EnemyLifeBar_Hontai;
    GameObject Player,  MPOrbsResources;
    [HideInInspector]
    public GameObject TargetedEnemy;
    Text text;
    PlayerMoving PlayerStatus;

    Vector2 Orbsize;
    struct OrbGauge
    {
       public GameObject MPGaugeObjects;
       public Material GaugeMat;
       public Image GaugeImage;
    }

    OrbGauge[] gauge;

    void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<PlayerMoving>();
        text = GetComponent<Text>();
        MPOrbsResources = Resources.Load("UI/MpsCharge") as GameObject;
        gauge = new OrbGauge[Mathf.CeilToInt(PlayerStatus.status.SpiritMax)];

        Orbsize = MPOrbsResources.GetComponent<GaugeSettles>().Gauges.GetComponent<Image>().sprite.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Menu")){
            Application.Quit();
        }
        //SPゲージ増減
        if (gauge.Length != Mathf.CeilToInt(PlayerStatus.status.SpiritMax))
        {
            foreach (OrbGauge orbGauge in gauge) {
                Destroy(orbGauge.MPGaugeObjects);
            }
            gauge = new OrbGauge[Mathf.CeilToInt(PlayerStatus.status.SpiritMax)];
        }
        for (int i = 0; i < gauge.Length; i++)
        {
            if (gauge[i].MPGaugeObjects == null) {
                gauge[i].MPGaugeObjects = Instantiate
                    (MPOrbsResources,
                    MPPos.transform.position + new Vector3(Orbsize.x * i  * 12,0,0),
                    Quaternion.AngleAxis(0,Vector3.zero),
                    MPPos.transform);
                gauge[i].GaugeImage = gauge[i].MPGaugeObjects.
                    GetComponent<GaugeSettles>().Gauges.GetComponent<Image>();
                gauge[i].GaugeMat = Instantiate(gauge[i].GaugeImage.material);
            }
            gauge[i].MPGaugeObjects.GetComponent<GaugeSettles>().Amount =
                Mathf.Clamp(PlayerStatus.status.Spirit - i, 0, 1f);
            gauge[i].GaugeMat.SetFloat
                ("_Hue", -i * 12f);
            gauge[i].GaugeImage.material = gauge[i].GaugeMat;
        }

        MpText.GetComponent<Text>().text = PlayerStatus.status.Spirit.ToString();


        //HPゲージ
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
        ChargeGauge.GetComponent<Image>().fillAmount = type_SP + type_EX;

        //相手のHPゲージ
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
