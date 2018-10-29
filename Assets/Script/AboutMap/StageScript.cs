using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public StageComp FirstComp, nextComp;
    public MapGenerate[] currentMap;
    public Vector2Int TransitTo;
    GameObject Player;
    GameObject InstMap;

    int MapLength, MapHeight;

    private CsvReader MapData;

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
        for (int i = 0; i < currentMap.Length; i++)
        {
            Destroy(currentMap[i].gameObject);
            Debug.Log("Destroyed");
        }
    }

    public void InstantiateFirstMap()
    {
        //まず読み込むマップの数を取得
        //マップごとに生成｡　現在マップを生成ごとに更新｡
        MapGens(InstMap, FirstComp);
    }

    public void InstantiateNextMap()
    {
        //まず読み込むマップの数を取得
        //マップごとに生成｡　現在マップを生成ごとに更新｡
        MapGens(InstMap, nextComp);
    }


    void MapGens(GameObject InstaMaps, StageComp Comps)
    {
        Player = GameObject.Find("Player");
        Player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        MapGenerate[] Maps = Comps.Map;
        int i = 0;
        currentMap = new MapGenerate[Maps.Length];
        //生成するマップを現在マップに設定｡
        foreach (MapGenerate Map in Maps)
        {
            if (Map.gameObject.GetComponent<CsvReader>() == null)
            {
                MapData = Map.gameObject.AddComponent<CsvReader>();
            }
            else {
                MapData = Map.gameObject.GetComponent<CsvReader>();
            }
            MapData.CsvResource = Map.MapResourceCSS;
            MapData.ReadData();
            MapLength = MapData.csvDatas[0].Length;
            MapHeight = MapData.csvDatas.Count;
            Sprite baseSprite = Map.DefaultTile.GetComponent<SpriteRenderer>().sprite;
            Vector2 TransitWorldpos =
                           new Vector2((TransitTo.x - MapLength / 2f) * baseSprite.bounds.size.x,
                           (MapHeight / 2f - TransitTo.y) * baseSprite.bounds.size.y);
            Player.transform.position = TransitWorldpos;
            InstMap = Instantiate(Map.gameObject);
            currentMap[i] = InstMap.GetComponent<MapGenerate>();
            i++;
        }
        Debug.Log("Instaned");
    }
}
