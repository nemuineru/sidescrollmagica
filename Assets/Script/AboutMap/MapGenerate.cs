using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour {


    public TextAsset MapResourceCSS;
    public GameObject[] MapChips;
    public GameObject DefaultTile;
    public bool setMapGlobalSize;

    GameObject[,] maps;

    int MapLength, MapHeight;

    private CsvReader MapData;
    TileConnectSet connectSet;
    // Use this for initialization
    void Start()
    {
        SpriteRenderer sprite =
            DefaultTile.GetComponent<SpriteRenderer>();
        if (gameObject.GetComponent<CsvReader>() == null)
        {
            MapData = gameObject.AddComponent<CsvReader>();
        }
        else
            MapData = gameObject.GetComponent<CsvReader>();
        connectSet = gameObject.AddComponent<TileConnectSet>();
        MapData.CsvResource = MapResourceCSS;
        MapData.ReadData(); //CSSファイルから読み出し
    

        MapLength = MapData.csvDatas[0].Length;
        MapHeight = MapData.csvDatas.Count;
        //マップの高さと長さをCSVファイルから読み取る


        GameObject Room = gameObject;
        Room.layer = LayerMask.NameToLayer("Terrain");
        Room.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        CompositeCollider2D CpCol2D =  Room.AddComponent<CompositeCollider2D>();
        CpCol2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
        CpCol2D.vertexDistance = 0.16f;
        //CompositColliderですべてのコリダーを設定｡
        maps = new GameObject[MapLength, MapHeight];
        //Debug.Log(MapLength + "," + MapHeight);

        if(setMapGlobalSize)
        Camera.main.GetComponent<CameraToScript>().WorldSize = new Vector2Int(MapLength,MapHeight);

        for (int j = 0; j < MapHeight; j++)
        {
            string Debugs = "";
            for (int i = 0; i < MapLength; i++)
            {
                Debugs += Convert.ToInt32(MapData.csvDatas[j][i]) + " ";
                if (Convert.ToInt32(MapData.csvDatas[j][i]) != -1)
                {
                    Vector2 Worldpos =
                          new Vector2((i - MapLength / 2f) * sprite.bounds.size.x,
                          (MapHeight / 2f - j) * sprite.bounds.size.y);
                    maps[i, j]
                        = Instantiate(MapChips[Convert.ToInt32(MapData.csvDatas[j][i])],
                        new Vector3(Worldpos.x, Worldpos.y, 5),
                        Quaternion.Euler(0, 0, 0)); ;
                    if (maps[i, j].GetComponent<TileGenerate>() != null)
                    {
                        maps[i, j].GetComponent<TileGenerate>().WorldPositions = new Vector2Int(i, j);
                    }
                    connectSet.SetTileConnections(maps, i, j,
                        MapLength, MapHeight);
                    maps[i, j].transform.parent = Room.transform;
                    //マップのチップごとに生成して設定｡
                }
            }
        }
        GameObject Borders = Resources.Load("PhisicallyBorder") as GameObject;
        Vector2 Borders_Size = Borders.GetComponent<BoxCollider2D>().size;

        Instantiate(Borders, 
            new Vector2(-((MapLength + 1) / 2f) * sprite.bounds.size.x - Borders_Size.x / 2 ,
                          (MapHeight / 2f) * sprite.bounds.size.y), 
            Quaternion.Euler(0, 0, 0), gameObject.transform);
        Instantiate(Borders,
            new Vector2(((MapLength - 1) / 2f) * sprite.bounds.size.x + Borders_Size.x / 2 ,
                          (MapHeight / 2f) * sprite.bounds.size.y),
            Quaternion.Euler(0, 0, 0), gameObject.transform);
        Instantiate(Borders,
            new Vector2(0f ,
                          ((MapHeight + 1) / 2f) * sprite.bounds.size.y + Borders_Size.y / 2),
            Quaternion.Euler(0, 0, 0), gameObject.transform);

        //右､左､上にボーダー生成｡

        Camera.main.GetComponent<CameraToScript>().CameraInstaTransit
               (GameObject.Find("Systems").GetComponent<StageScript>().TransitTo, new Vector2(MapLength, MapHeight));
        //カメラの即時移動｡
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
