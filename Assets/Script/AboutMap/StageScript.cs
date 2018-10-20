using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour {

    public MapGenerate[] prevupMap,currentMap,nextupMap;
    public Vector2 TransitTo;
    GameObject Player;
    GameObject InstMap;

    int MapLength, MapHeight;
    
    private CsvReader MapData;

    void Awake()
    {
        //NextUpに記載されたマップを出力｡
        InstantiateNextMap();
    }

    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyCurrentMap()
    {
        for (int i = 0; i < currentMap.Length; i++) {
            Destroy(currentMap[i].gameObject,1.0f);
        }
    }
    public void InstantiateNextMap()
    {
        //まず読み込むマップの数を取得
        //マップごとに生成｡　現在マップを生成ごとに更新｡
        MapGens(InstMap, nextupMap);
    }

    public void InstantiatePrevMap()
    {
        currentMap = new MapGenerate[nextupMap.Length];
        MapGens(InstMap, prevupMap);
    }

    void MapGens(GameObject InstaMaps,MapGenerate[] Maps) {
        int i = 0;
        currentMap = new MapGenerate[Maps.Length];
        prevupMap = new MapGenerate[currentMap.Length];
        //現在のマップを過去のマップに記載｡
        foreach (MapGenerate Map in currentMap) {
            prevupMap[i] = Map;
            i++;
        }
        i = 0;
        //生成するマップを現在マップに設定｡
        foreach (MapGenerate Map in Maps)
        {
            Player = GameObject.Find("Player");
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
            InstaMaps = Instantiate(Map.gameObject);
            currentMap[i] = InstaMaps.GetComponent<MapGenerate>();
            i++;
        }
    }
}
