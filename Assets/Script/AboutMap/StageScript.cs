using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public StageComp FirstComp, nextComp;
    public MapGenerate[] currentMap;
    public Vector2Int TransitTo;
    GameObject Player;
    GameObject InstMap;

    public int MapLength, MapHeight;
    public Sprite baseSprite;

    CsvReader MapData;

    void Awake()
    {
        //NextUpに記載されたマップを出力｡
        InstantiateFirstMap();
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void DestroyCurrentMap()
    {
        MapLength = 0;
        MapHeight = 0;
        for (int i = 0; i < currentMap.Length; i++)
        {
            Destroy(currentMap[i].gameObject);
            Debug.Log("Destroyed");
        }
    }

    public void InstantiateFirstMap()
    {
        MapLength = 0;
        MapHeight = 0;
        //まず読み込むマップの数を取得
        //マップごとに生成｡　現在マップを生成ごとに更新｡
        MapGens(InstMap, FirstComp);
    }

    public void InstantiateNextMap()
    {
        MapLength = 0;
        MapHeight = 0;
        //まず読み込むマップの数を取得
        //マップごとに生成｡　現在マップを生成ごとに更新｡
        MapGens(InstMap, nextComp);
    }


    void MapGens(GameObject InstaMaps, StageComp Comps)
    {
        int Length = 0, Height = 0;

           Player = GameObject.Find("Player");
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        MapGenerate[] Maps = Comps.Map;
        int i = 0;
        currentMap = new MapGenerate[Maps.Length];
        //生成するマップを現在マップに設定｡
        for(i = 0;i < Maps.Length;i++)
        {
            MapGenerate Map = Maps[i];
            if (Map.gameObject.GetComponent<CsvReader>() != null)
            {
                MapData = Map.gameObject.GetComponent<CsvReader>();
            }
            MapData.CsvResource = Map.MapResourceCSS;
            MapData.ReadData();
            Length = MapData.csvDatas[0].Length;
            Debug.Log("X : " + MapData.csvDatas[0].Length);
            Height = MapData.csvDatas.ToArray().Length;
            Debug.Log("Y : " + MapData.csvDatas.ToArray().Length);
            baseSprite = Map.DefaultTile.GetComponent<SpriteRenderer>().sprite;
            InstMap = Instantiate(Map.gameObject);
            currentMap[i] = InstMap.GetComponent<MapGenerate>();
        }
        MapLength = Length;
        MapHeight = Height;
        Vector2 TransitWorldpos =
                       new Vector2((TransitTo.x - Length / 2f) * baseSprite.bounds.size.x,
                       (Height / 2f - TransitTo.y) * baseSprite.bounds.size.y);
        Player.transform.position = TransitWorldpos;
        Debug.Log("Instaned");
    }
}
