using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusComp : MonoBehaviour {

    public GameObject HP, MPPos ,ChargeGauge, TargetedEnemyHP, TargetedEnemyHpText, EnemyLifeBar_Hontai;
    GameObject Player,  MPOrbsResources;
    [HideInInspector]
    public GameObject TargetedEnemy;
    Text text;
    PlayerMoving PlayerStatus;

    GameObject[] MPGaugeObjects; Vector2 Orbsize;
    Material[] GaugeMat;
    void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerStatus = Player.GetComponent<PlayerMoving>();
        text = GetComponent<Text>();
        MPOrbsResources = Resources.Load("UI/MpsCharge") as GameObject;
        MPGaugeObjects = new GameObject[Mathf.CeilToInt(PlayerStatus.status.SpiritMax)];
        GaugeMat = new Material[Mathf.CeilToInt(PlayerStatus.status.SpiritMax)];

        Orbsize = MPOrbsResources.GetComponent<GaugeSettles>().Gauges.GetComponent<Image>().sprite.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (MPGaugeObjects.Length != Mathf.CeilToInt(PlayerStatus.status.SpiritMax))
        {
            foreach (GameObject Gauges in MPGaugeObjects) {
                Destroy(Gauges);
            }
            MPGaugeObjects = new GameObject[Mathf.CeilToInt(PlayerStatus.status.SpiritMax)];
        }
        for (int i = 0; i < MPGaugeObjects.Length; i++) {
            if (MPGaugeObjects[i] == null) {
                MPGaugeObjects[i] = Instantiate
                    (MPOrbsResources,
                    MPPos.transform.position + new Vector3(Orbsize.x * i  * 18,0,0),
                    Quaternion.AngleAxis(0,Vector3.zero),
                    MPPos.transform);                
            }

            MPGaugeObjects[i].GetComponent<GaugeSettles>().Amount = 
                Mathf.Clamp(PlayerStatus.status.Spirit - i,0,1f);
            Image GaugeImage = MPGaugeObjects[i].GetComponent<GaugeSettles>().Gauges.GetComponent<Image>();
            GaugeMat[i] = GaugeImage.material;
            GaugeMat[i].SetFloat
                ("_Hue",Mathf.Repeat(GaugeMat[i].GetFloat("_Hue") +
                i * 2.5f * Time.deltaTime,360));
        }


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
