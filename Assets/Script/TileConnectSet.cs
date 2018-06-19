using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConnectSet : MonoBehaviour {
    public GameObject DefaultTile;

    public void SetTileConnections(GameObject[,] Tile , int X, int Y , int Width, int Height)
    {
        /*
         タイルのセレクトは
         123
         456
         789
         の順番

        そこからさらに
        123
        456
        789
        で変更が決定

         */
        for (int j = 0; j < 9; j++)
        { //先ず指定周囲9マスのタイルの所在を確認。
            int Row = 0;
            switch (j)
            {
                case 0:
                case 1:
                case 2:
                    Row = -1;
                    break;
                case 3:
                case 4:
                case 5:
                    Row = 0;
                    break;
                case 6:
                case 7:
                case 8:
                    Row = 1;
                    break;
                default:
                    break;
            }
            int X_Sel = X - 1 + (j % 3), Y_Sel = Y + Row;

            if ((X_Sel >= 0 && Y_Sel >= 0) && (X_Sel < Width && Y_Sel < Height) 
                && Tile[X_Sel,Y_Sel] != null && 
                Tile[X_Sel,Y_Sel].GetComponent<TileGenerate>() != null)
            { 
                //周りの中で指定したものが存在するなら 
            TileGenerate TileGen = Tile[X_Sel, Y_Sel].GetComponent<TileGenerate>();
            int Row_After = 0;
                for (int i = 0; i < 9; i++)
                {
                    switch (i)
                    {
                        case 0:
                        case 1:
                        case 2:
                            Row_After = -1;
                            break;
                        case 3:
                        case 4:
                        case 5:
                            Row_After = 0;
                            break;
                        case 6:
                        case 7:
                        case 8:
                            Row_After = 1;
                            break;
                        default:
                            break;
                    }
                    Vector2Int selection = new Vector2Int((X_Sel + 1 - (i % 3)), Y_Sel + Row_After);

                    if (selection.x >= 0 && selection.y >= 0 && 
                        selection.x < Width && selection.y < Height &&
                        Tile[selection.x, selection.y] != null
                        && Tile[selection.x, selection.y].GetComponent<TileGenerate>() != null)
                    {
                        if (TileGen.property.Tilename == 
                            Tile[selection.x,
                            selection.y].GetComponent
                            <TileGenerate>().property.Tilename)
                        //9マス周りの指定したマスの周囲のタイルが同じ名前かを確認。
                        {
                            TileGen.TileWithin[8 - i] = true;
                        }
                        else {
                            TileGen.TileWithin[8 -i] = false;
                        }
                        TileGen.isEcoChanged = true;
                        }
                    else
                    {
                        TileGen.TileWithin[8 - i] = false;
                        TileGen.TW_Name[8 - i] = "None";
                        TileGen.isEcoChanged = true;
                    }
                }
            }
        }
    }


    public void DebugLine(Vector2 size , int MapWidth,int MapHeight) { //デバッグ用のグリッド出力 Basespriteのサイズに合わせてグリッドが存在。
        
        for (int x = 0; x < MapWidth; x++)
        {
            Vector2 Bottom = new Vector2(size.x * (x - MapWidth / 2), size.y * (-MapHeight / 2));
            Vector2 Upward = new Vector2(size.x * (x - MapWidth / 2), size.y * (MapHeight / 2));
            Debug.DrawLine(Bottom, Upward, Color.green);
        }
        for (int y = 0; y < MapHeight; y++)
        {
            Vector2 Left = new Vector2(size.x * (-MapWidth / 2), size.y * (y - MapHeight / 2));
            Vector2 Right = new Vector2(size.x * (MapWidth / 2), size.y * (y - MapHeight / 2));
            Debug.DrawLine(Left, Right, Color.cyan);
        }
    }
    
    
}