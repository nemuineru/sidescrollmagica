using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
[SerializeField]
public class TileGenerate : MonoBehaviour {
    public Vector2Int WorldPositions;
    public Vector2Int RoomPositions;


    public bool isEcoChanged; //そのタイル自体の環境が変わったか
    public bool isDebug, isSingleton;
    public enum TileType
    {
        PassableTerrain,
        ImpassableTerrain,
        Wall,
        StaticObject,
        Gate
    }
    public TileType tileType;
    [SerializeField]
    [Space]
    [Header("Property")]
    public TexProp property;
    [System.Serializable]
    public class TexProp
    {
        public string Tilename; //タイルの名前
        public Texture2D Texture; //タイルのテクスチャ
        public Vector2Int TexSize = new Vector2Int(64, 96);
        public Vector2Int CutSize = new Vector2Int(32, 32);

        [SerializeField, Space(15)]

        public StageComp StageConnectedTo; //ステージの選択
        public Vector2Int ConnectTo; //ゲート使用時どこにつながるか
        public bool isContactToGo = true;
        public AudioClip EnteringSound, SteppingSound;//走ったときの音と侵入時の音
    }
    public int Db_GroupNum, Db_CutNum; 
    public bool[] TileWithin = new bool[9]; //タイル9周辺にタイル存在するか

    public string[] TW_Name = new string[9]; //タイル9周辺に同タイルの名前が存在するか

    /*
     タイル接続先
     6 7 8 
     3(4)5
     0 1 2
     */

    Sprite BakedSprite;

    Color[] TileColors(Texture2D texture ,int TileGroup, int GroupNumber) { //タイルのテクスチャ確認
        int X = 0, Y = 0; //タイルはMV式。 

        /*
         (0)1
          2 3
          4 5 <-タイルグループ
          
          0 1<-グループナンバー
          2 3
         */
        int Xadd_as_tileGroup, Yadd_as_tileGroup;
        Xadd_as_tileGroup = property.TexSize.x / 2 * (TileGroup % 2);
        Yadd_as_tileGroup = property.TexSize.y / 3 * (2 - Mathf.CeilToInt(TileGroup / 2));
        X = Xadd_as_tileGroup + property.CutSize.x / 2 * (GroupNumber % 2);
        Y = Yadd_as_tileGroup + (property.CutSize.y / 2 * Mathf.CeilToInt((3 - GroupNumber) / 2));
       
        return texture.GetPixels(X, Y, property.CutSize.x / 2 , property.CutSize.y / 2);
    }

    Texture2D Tiles;

    GameObject Systems;
    GateManager SystemGate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && tileType == TileType.Gate && property.isContactToGo)
        {
            Systems = GameObject.Find("Systems");
            SystemGate = Systems.GetComponent<GateManager>();
            SystemGate.NextUp = property.StageConnectedTo;
            SystemGate.EnteringSound = property.EnteringSound;
            SystemGate.TransitTo = property.ConnectTo;
            StartCoroutine(SystemGate.ExitAndEnter());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && 
            tileType == TileType.Gate && property.isContactToGo != true)
        {
            Systems = GameObject.Find("Systems");
            SystemGate = Systems.GetComponent<GateManager>();
            SystemGate.NextUp = property.StageConnectedTo;
            SystemGate.EnteringSound = property.EnteringSound;
            SystemGate.TransitTo = property.ConnectTo;
            StartCoroutine(SystemGate.ExitAndEnter());
        }
    }

    private void Start()
    {
        if (isSingleton != true)
        {
            Tiles = TileCreate(property.Texture); //タイル画像を実際に生成
            GetComponent<SpriteRenderer>().sprite
                = Sprite.Create(Tiles, new Rect(Vector2.zero, Vector2.one * 32), Vector2.one / 2);
            GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;
        }
        if (tileType == TileType.Gate) {
            GetComponent<Collider2D>().usedByComposite = false;
            GetComponent<Collider2D>().isTrigger = true;
        }
    }


    private void Update()
    {
        if (isDebug)
        {
            Tiles.SetPixels(0,0,16,16, TileColors(property.Texture, Db_GroupNum, Db_CutNum));
            Tiles.Apply();
            GetComponent<SpriteRenderer>().sprite
            = Sprite.Create(Tiles, new Rect(Vector2.zero, Vector2.one * 16), Vector2.one / 2);
           
        }
        else if (isEcoChanged == true && isSingleton != true)　//タイル変更‥
        {
            Tiles = TileCreate(property.Texture);
            GetComponent<SpriteRenderer>().sprite
                = Sprite.Create(Tiles, new Rect(Vector2.zero, Vector2.one * 32), Vector2.one / 2);
            GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;
            isEcoChanged = false;
        }
    }

    public Texture2D TileCreate(Texture2D SourceTex)
    { /*タイルのテクスチャを四辺に貼り付け
        タイルを貼り付ける順番は
        12 
        34
        となっている
        */

        Color[] TileColor = new Texture2D(property.CutSize.x / 2, property.CutSize.y / 2).GetPixels();
        Texture2D Tiles = new Texture2D(property.CutSize.x, property.CutSize.y);
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (TileWithin[4 - 1] == false && TileWithin[8 - 1] == false)
                        TileColor = TileColors(SourceTex, 2, 0);
                    else if (TileWithin[4 - 1] == true && TileWithin[8 - 1] == false)
                        TileColor = TileColors(SourceTex, 3, 0);
                    else if (TileWithin[4 - 1] == false && TileWithin[8 - 1] == true)
                        TileColor = TileColors(SourceTex, 4, 0);
                    else if (TileWithin[4 - 1] == true && TileWithin[8 - 1] == true)
                        if (TileWithin[7 - 1] == true)
                        {
                            TileColor = TileColors(SourceTex, 5, 0);
                        }
                        else
                        {
                            TileColor = TileColors(SourceTex, 1, 0);
                        }
                    break;
                case 1:
                    if (TileWithin[6 - 1] == false && TileWithin[8 - 1] == false)
                        TileColor = TileColors(SourceTex, 3, 1);
                    else if (TileWithin[6 - 1] == true && TileWithin[8 - 1] == false)
                        TileColor = TileColors(SourceTex, 2, 1);
                    else if (TileWithin[6 - 1] == false && TileWithin[8 - 1] == true)
                        TileColor = TileColors(SourceTex, 5, 1);
                    else if (TileWithin[6 - 1] == true && TileWithin[8 - 1] == true)
                        if (TileWithin[9 - 1] == true)
                        {
                            TileColor = TileColors(SourceTex, 4, 1);
                        }
                        else
                        {
                            TileColor = TileColors(SourceTex, 1, 1);
                        }
                    break;
                case 2:
                    if (TileWithin[4 - 1] == false && TileWithin[2 - 1] == false)
                        TileColor = TileColors(SourceTex, 4, 2);
                    else if (TileWithin[4 - 1] == true && TileWithin[2 - 1] == false)
                        TileColor = TileColors(SourceTex, 5, 2);
                    else if (TileWithin[4 - 1] == false && TileWithin[2 - 1] == true)
                        TileColor = TileColors(SourceTex, 2, 2);
                    else if (TileWithin[4 - 1] == true && TileWithin[2 - 1] == true)
                        if (TileWithin[1 - 1] == true)
                        {
                            TileColor = TileColors(SourceTex, 3, 2);
                        }
                        else
                        {
                            TileColor = TileColors(SourceTex, 1, 2);
                        }

                    break;
                case 3:
                    if (TileWithin[6 - 1] == false && TileWithin[2 - 1] == false)
                        TileColor = TileColors(SourceTex, 5, 3);
                    else if (TileWithin[6 - 1] == true && TileWithin[2 - 1] == false)
                        TileColor = TileColors(SourceTex, 4, 3);
                    else if (TileWithin[6 - 1] == false && TileWithin[2 - 1] == true)
                        TileColor = TileColors(SourceTex, 3, 3);
                    else if (TileWithin[6 - 1] == true && TileWithin[2 - 1] == true)
                        if (TileWithin[3 - 1] == true)
                        {
                            TileColor = TileColors(SourceTex, 2, 3);
                        }
                        else
                        {
                            TileColor = TileColors(SourceTex, 1, 3);
                        }
                    break;
                default:

                    break;
            }
            int Boundries = 0;
            if (i > 1)
            {
                Boundries = property.CutSize.y / 2;
            }
            Tiles.SetPixels(property.CutSize.x / 2 * (i % 2), property.CutSize.y / 2 - Boundries, property.CutSize.x / 2, property.CutSize.y / 2, TileColor);
        }
        Tiles.Apply();
        return Tiles;
    }
}