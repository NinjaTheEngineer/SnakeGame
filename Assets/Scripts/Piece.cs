using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Piece{
    public int X;
    public int Y;
    public Piece(int x, int y) {
        X = x;
        Y = y;
    }
    public bool InTheSamePositionOf(Piece piece) {
        if(piece==null){
            Debug.Log("InTheSamePositionOf, piece is null => return false");
            return false;
        }
        return this.Equals(piece);
    }
    public override bool Equals(object other) {
        var otherPiece = other as Piece;
        return otherPiece!=null && X==otherPiece.X && Y==otherPiece.Y;
    }
    public override int GetHashCode() => (X.GetHashCode() + Y.GetHashCode());
    public override string ToString() => "("+X+","+Y+")";
}
