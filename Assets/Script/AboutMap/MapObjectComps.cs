using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectComps : MonoBehaviour {

    public enum ObjectType
    {
        Mobiles
    }
    public ObjectType objType;

    public enum MoveType {
        Straight,
        Circle,
        Accelates
    }
    public MoveType moveType;

    public Vector2 range;
    public Vector2 FirstPos;
    public float TimeCycle;
    [Range(0, 1f)]
    public float progress;

    float CurrentTime;


    Sprite MainSprites;
    Rigidbody2D Ridge2D;
    // Use this for initialization
    void Start() {
        Ridge2D = GetComponent<Rigidbody2D>();
        FirstPos = transform.position;
        switch (moveType)
        {
            case MoveType.Straight:
            StartCoroutine("MoveStraight");
                break;
            case MoveType.Circle:
                StartCoroutine("MoveCircle");
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator MoveStraight()
    {
        CurrentTime = progress * TimeCycle;
        while (Application.isPlaying) {
            MainSprites = Camera.main.gameObject.GetComponent<CameraToScript>().Basesprite;
            Vector2 TargetPoint = (range * MainSprites.bounds.size) * (1 - Mathf.Abs((TimeCycle/2) - CurrentTime) / (TimeCycle / 2)) + FirstPos;//range..のところはプログレスとともに移動
            Ridge2D.MovePosition(TargetPoint);
            CurrentTime += Time.deltaTime;
            progress = CurrentTime / TimeCycle;
            if (CurrentTime > TimeCycle) {
                CurrentTime = 0f;
            }
            yield return null;
        }
    }

    IEnumerator MoveCircle()
    {
        CurrentTime = progress * TimeCycle;
        while (Application.isPlaying)
        {
            MainSprites = Camera.main.gameObject.GetComponent<CameraToScript>().Basesprite;
            Vector2 TargetPoint = new Vector2
                (range.x * Mathf.Cos((Mathf.Sign(range.y) * CurrentTime * 2 / TimeCycle) * Mathf.PI ) ,
                 range.x * Mathf.Sin((Mathf.Sign(range.y) * CurrentTime * 2 / TimeCycle) * Mathf.PI )) * MainSprites.bounds.size
                + FirstPos;//range..のところはプログレスとともに移動
            Ridge2D.MovePosition(TargetPoint);
            CurrentTime += Time.deltaTime;
            progress = CurrentTime / TimeCycle;

            if (CurrentTime > TimeCycle)
            {
                CurrentTime = 0f;
            }


            int Pts = 0,PolyCount = 24;
            Vector2[] RotativePoint = new Vector2[PolyCount];
            for (int i = 0; i < RotativePoint.Length; i++) {
                RotativePoint[i] = new Vector2
                (range.x * Mathf.Cos((Mathf.Sign(range.y) * i / (PolyCount - 1)) * 2 * Mathf.PI),
                 range.x * Mathf.Sin((Mathf.Sign(range.y) * i / (PolyCount - 1)) * 2 * Mathf.PI)) * MainSprites.bounds.size
                + FirstPos;
            }
            foreach (Vector2 Points in RotativePoint)
            {
                if (Pts != (PolyCount - 1))
                {
                    Debug.DrawLine(Points, RotativePoint[Pts + 1], Color.white);
                    Pts++;
                }
                else
                {
                    Debug.DrawLine(Points, RotativePoint[0], Color.white);
                }
            }
            yield return null;
        }
    }


}
