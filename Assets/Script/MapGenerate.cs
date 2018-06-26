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
        MapData = gameObject.AddComponent<CsvReader>();
        connectSet = gameObject.AddComponent<TileConnectSet>();
        MapData.CsvResource = MapResourceCSS;
        MapData.ReadData();

        MapLength = MapData.csvDatas[0].Length;
        MapHeight = MapData.csvDatas.Count;

        GameObject Room = new GameObject("Room");
        Room.layer = LayerMask.NameToLayer("Terrain");
        Room.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        CompositeCollider2D CpCol2D =  Room.AddComponent<CompositeCollider2D>();
        CpCol2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
        CpCol2D.vertexDistance = 0.16f;
        maps = new GameObject[MapLength, MapHeight];
        Debug.Log(MapLength + "," + MapHeight);

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
                        maps[i, j].transform.parent = Room.transform;
                    }
                    connectSet.SetTileConnections(maps, i, j,
                        MapLength, MapHeight);
                    }
            }
                    Debug.Log(j + "," + Debugs);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
