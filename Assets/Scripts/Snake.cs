using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snake : MonoBehaviour {
    public Direction originalDirection = Direction.DOWN;
    public Direction currentDirection {get; private set;}
    public List<Piece> SnakePieces {get; private set;}
    public GameObject snakePieceGO;
    private int currentLastPieceX, currentLastPieceY;
    private int currentTargetLength = 3;
    private bool alreadyMoved;
    private int initX, initY;
    Vector3 lastHeadPosition = Vector3.zero;
    private void Awake() {
        initX = (int)transform.position.x;
        initY = (int)transform.position.y;
        InitializeSnake();
    }
    public void Reset(){
        Debug.Log("Reset::Reset Game");
        foreach(Piece sp in SnakePieces){
            Destroy(sp.gameObject);
        }
        InitializeSnake();
    }
    private void InitializeSnake(){
        Piece firstPiece = Instantiate(snakePieceGO, new Vector3(initX, initY), Quaternion.identity).GetComponent<Piece>();
        SnakePieces = new List<Piece>(){firstPiece};
        currentDirection = originalDirection;
    }
    public void SetDirection(Direction dir) {
        if(alreadyMoved && IsDirectionValid(dir)){
            currentDirection = dir;
            alreadyMoved = false;
        }
    }
    public int Length => SnakePieces.Count;
    private Piece HeadPiece => SnakePieces[Length-1];
    private Piece LastTailPiece => SnakePieces[0];
    public void Move() {
        currentLastPieceX = (int)LastTailPiece.transform.position.x;
        currentLastPieceY = (int)LastTailPiece.transform.position.y;

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
        MoveTailPieces();
        alreadyMoved = true;
    }
    private bool IsDirectionValid(Direction direction) {
        return !((currentDirection==Direction.LEFT && direction==Direction.RIGHT) ||
                (currentDirection==Direction.RIGHT && direction==Direction.LEFT) || 
                (currentDirection==Direction.UP && direction==Direction.DOWN) || 
                (currentDirection==Direction.DOWN && direction==Direction.UP));
    }
    private void MoveHeadPiece(int x, int y) {
        lastHeadPosition = HeadPiece.transform.position;
        SnakePieces.Add(Instantiate(snakePieceGO, new Vector3(HeadPiece.transform.position.x+x, HeadPiece.transform.position.y+y), Quaternion.identity).GetComponent<Piece>());
        GameObject lastPiece = LastTailPiece.gameObject;
        SnakePieces.RemoveAt(0);
        Destroy(lastPiece);
    }
    private void MoveTailPieces(){
        Vector3 lastPosition = lastHeadPosition;
        int tailsPiecesCount = Length-2;
        for (int i = tailsPiecesCount; i >= 0; i--) {
            Vector3 temp = SnakePieces[i].transform.position;
            SnakePieces[i].UpdatePosition((int)lastPosition.x, (int)lastPosition.y);
            if(lastPosition==temp){     //couldn't figure out why this happens
                return;
            }
            lastPosition = temp;
        }
    }
    public void Grow(){
        SnakePieces.Insert(0, Instantiate(snakePieceGO, new Vector3(currentLastPieceX, currentLastPieceY), Quaternion.identity).GetComponent<Piece>());
    }
    public bool RunsInto(Piece piece) {
        if(piece==null){
            Debug.Log("RunsInto::Piece is null => return false.");
            return false;
        }
        return HeadPiece.InTheSamePositionOf(piece);
    }
    public bool RunsIntoItself(){
        for (int i = 0; i < Length-1; i++) {
            if(SnakePieces[i].Equals(HeadPiece)){
                Debug.Log("RunsIntoItself::SnakePiece[i]="+SnakePieces[i]+" is equal to the HeadPiece="+HeadPiece+". => return true.");
                return true;
            }
        }
        return false;
    }
}
public enum Direction {LEFT, RIGHT, UP, DOWN}
