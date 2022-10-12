using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece : MonoBehaviour {
    public void UpdatePosition(int x, int y){
        transform.position = new Vector3(x, y);
    }
    public bool InTheSamePositionOf(Piece piece) {
        if(piece==null){
            Debug.Log("InTheSamePositionOf::Piece is null => return false");
            return false;
        }
        return IsAt((int)piece.transform.position.x, (int)piece.transform.position.y);
    }
    public bool IsAt(int x, int y) => transform.position.x==x && transform.position.y==y; 

    public override string ToString() => "("+transform.position.x+","+transform.position.y+")";
}
