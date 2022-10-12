using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece : MonoBehaviour{
    public int X {get; private set;}
    public int Y{get; private set;}
    private void Awake() {
        X = (int)transform.position.x;
        Y = (int)transform.position.y;
    }
    public void UpdatePosition(int x, int y){
        X = x;
        Y = y;    
        transform.position = new Vector3(X, Y);
    }
    public bool InTheSamePositionOf(Piece piece) {
        if(piece==null){
            Debug.Log("InTheSamePositionOf::Piece is null => return false");
            return false;
        }
        return this.Equals(piece);
    }
    public bool InTheSamePositionOf(int x, int y) => X==x && Y==y; 
    public override bool Equals(object other) {
        var otherPiece = other as Piece;
        return otherPiece!=null && X==otherPiece.X && Y==otherPiece.Y;
    }
    public override int GetHashCode() => (X.GetHashCode() + Y.GetHashCode());
    public override string ToString() => "("+X+","+Y+")";
}
