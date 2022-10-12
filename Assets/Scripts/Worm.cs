using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Worm {
    public Direction currentDirection {get; private set;}
    public List<Piece> WormPieces{get; private set;}
    private int currentLastPieceX, currentLastPieceY;
    private int currentTargetLength = 3;
    private bool alreadyMoved;
    public Worm(int originalX, int originalY, Direction originalDirection) {
        currentDirection = originalDirection;
        WormPieces = new List<Piece>(){ new Piece(originalX, originalY)};
    }
    public void SetDirection(Direction dir) {
        if(alreadyMoved && IsDirectionValid(dir)){
            currentDirection = dir;
            alreadyMoved = false;
        }
    }
    public int Length => WormPieces.Count;
    public Piece HeadPiece => WormPieces[Length-1];
    public void Move() {
        currentLastPieceX = WormPieces[0].X;
        currentLastPieceY = WormPieces[0].Y;

        switch(currentDirection){
            case Direction.LEFT:
                MoveHeadPiece(-1, 0);
                break;
            case Direction.RIGHT:
                MoveHeadPiece(1,0);
                break;
            case Direction.UP:
                MoveHeadPiece(0, 1);
                break;
            case Direction.DOWN:
                MoveHeadPiece(0, -1);
            break;
        }

        if(Length < currentTargetLength) {
            Grow();
        }
        alreadyMoved = true;
    }
    private bool IsDirectionValid(Direction direction) {
        return !(currentDirection==Direction.LEFT && direction==Direction.RIGHT ||
                currentDirection==Direction.RIGHT && direction==Direction.LEFT || 
                currentDirection==Direction.UP && direction==Direction.DOWN || 
                currentDirection==Direction.DOWN && direction==Direction.UP);
    }
    public void MoveHeadPiece(int x, int y) {
        WormPieces.Add(new Piece(HeadPiece.X+x, HeadPiece.Y+y));
        WormPieces.RemoveAt(0);
    }
    public void IncrementLength() => currentTargetLength++;
    public void Grow() => WormPieces.Insert(0, new Piece(currentLastPieceX, currentLastPieceY));
    public bool RunsInto(Piece piece) {
        if(piece==null){
            Debug.Log("RunsInto, piece is null => return false.");
            return false;
        }
        return HeadPiece.InTheSamePositionOf(piece);
    }
    public bool RunsIntoItself(){
        for (int i = 0; i < Length-1; i++) {
            if(WormPieces[i].Equals(HeadPiece)){
                Debug.Log("RunsIntoItself, WormPiece[i]="+WormPieces[i]+" is equal to the HeadPiece="+HeadPiece+". => return true.");
                return true;
            }
        }
        return false;
    }
}
public enum Direction {LEFT, RIGHT, UP, DOWN}
