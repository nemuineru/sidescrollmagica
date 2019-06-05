using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour {


    public TextAsset MapResourceCSS;
    public GameObject[] MapChips;
    public GameObject DefaultTile;
    public bool setMapGlobalSize;
    public Vector2Int IfMakeFirst_posset;

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
        GameObject System = GameObject.Find("Systems");
        System.GetComponent<StageScript>().MapHeight = MapHeight;
        //マップの高さと長さをCSVファイルから読み取る


        GameObject Room = gameObject;
        GameObject[] Terrains = new GameObject[MapChips.Length];
        int ChipRooms = 0;
        foreach (GameObject Chip in  MapChips)
        {
            if (!Chip) {
                ChipRooms++;
            }
            else
            {
                GameObject Terrain = new GameObject("Terrain " + ChipRooms);
                Terrains[ChipRooms] = Terrain;
                Terrains[ChipRooms].AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                CompositeCollider2D CpCol2D = Terrains[ChipRooms].AddComponent<CompositeCollider2D>();
                CpCol2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
                CpCol2D.vertexDistance = 0.16f;
                if (Chip.GetComponent<TileGenerate>())
                {
                    CpCol2D.sharedMaterial = Chip.GetComponent<TileGenerate>().property.physicMaterials;
                }
                Terrains[ChipRooms].layer = Chip.layer;
                Terrains[ChipRooms].transform.parent = Room.transform;
                ChipRooms++;
            }
        }
        //設定した地形ごとに親を設定してコンポジットコリダーを作る｡
        maps = new GameObject[MapLength, MapHeight];

        if(setMapGlobalSize)
        Camera.main.GetComponent<CameraToScript>().WorldSize = new Vector2Int(MapLength,MapHeight);

        for (int height = 0; height < MapHeight; height++)
        {
            for (int lengths = 0; lengths < MapLength; lengths++)
            {
                if (Convert.ToInt32(MapData.csvDatas[height][lengths]) != -1)
                {
                    Vector2 Worldpos =
                          new Vector2((lengths - MapLength / 2f) * sprite.bounds.size.x,
                          (MapHeight / 2f - height) * sprite.bounds.size.y);
                    maps[lengths, height]
                        = Instantiate(MapChips[Convert.ToInt32(MapData.csvDatas[height][lengths])],
                        new Vector3(Worldpos.x, Worldpos.y, 5),
                        Quaternion.Euler(0, 0, 0)); ;
                    if (maps[lengths, height].GetComponent<TileGenerate>() != null)
                    {
                        maps[lengths, height].GetComponent<TileGenerate>().WorldPositions = new Vector2Int(lengths, height);
                    }
                    connectSet.SetTileConnections(maps, lengths, height,
                        MapLength, MapHeight);
                    maps[lengths, height].transform.parent = Terrains[Convert.ToInt32(MapData.csvDatas[height][lengths])].transform;
                    //マップのチップごとに生成して設定｡
                }
            }
        }
        GameObject Borders = Resources.Load("PhisicallyBorder") as GameObject;
        GameObject DeadBorders = Resources.Load("DeadBorder") as GameObject;
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
        Instantiate(DeadBorders,
            new Vector2(0f,
                          -((MapHeight + 1) / 2f) * sprite.bounds.size.y
                          - DeadBorders.GetComponent<BoxCollider2D>().size.y / 2),
            Quaternion.Euler(0, 0, 0), gameObject.transform);

        //右､左､上にボーダー生成｡ 下にはデッドラインボーダーを生成｡

        Camera.main.GetComponent<CameraToScript>().CameraInstaTransit
               (GameObject.Find("Systems").GetComponent<StageScript>().TransitTo, new Vector2(MapLength, MapHeight));
        //カメラの即時移動｡
        if (IfMakeFirst_posset.x != 0 && IfMakeFirst_posset.y != 0) {
            GameObject.Find("Systems").GetComponent<StageScript>().TransitTo = IfMakeFirst_posset;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
