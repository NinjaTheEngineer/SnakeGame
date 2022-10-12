using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeGame : MonoBehaviour {
    [SerializeField]private int Width = 15;
    [SerializeField]private int Height = 10;
    private Worm Worm;
    private Apple Apple;
    private bool GameStarted;
    [SerializeField] private float currentMoveDelay = 0;
    private float currentTimer;
    public float minMoveDelay = 0.25f;
    public float initialMoveDelay = 2f;
    public float moveDelayDecreaseAmount = 0.05f;
    public GameObject appleGO;
    public GameObject wormGO;
    public GameObject wallGO;
    List<Wall> walls = new List<Wall>();

    private void Start() {
        Camera.main.transform.position = new Vector3(Width/2, Height/2, -10);
        InitializeGame();
    }
    private void InitializeGame(){
        Worm = new Worm(Width/2, Height/2, Direction.DOWN);
        CreateApple();
        DrawWalls();
        DrawGame();    
        currentTimer = 0;
        GameStarted = true;
    }
    private void Update() {
        if(!GameStarted){
            InitializeGame();
            return;
        }
        currentTimer += Time.unscaledDeltaTime;
        if(currentTimer>currentMoveDelay && Worm!=null){
            currentTimer=0;
            Worm.Move();
            if(Worm.RunsIntoItself() || WormRunIntoWall){
                GameStarted = false;
                return;
            }
            if(Worm.RunsInto(Apple)){
                Worm.Grow();
                CreateApple();
            }
            HandleMoveDelay();
            DrawGame();
        }
        HandleInput();
    }
    private bool WormRunIntoWall{
        get{
            if(Worm==null){
                Debug.Log("WormRunIntoWall, Worm is null => return true.");
                return true;
            }
            if(wallsInstantiated){
                int wallsCount = walls.Count;
                for (int i = 0; i < wallsCount; i++) {
                    if(Worm.RunsInto(walls[i])){
                        Debug.Log("WormRunIntoWall, Worm runs into wall piece="+walls[i] +" => return true.");
                        return true;
                    }
                }
            }
            return false;
        }
    }
    private void HandleMoveDelay(){
        float newMoveDelay = initialMoveDelay - Worm.WormPieces.Count*moveDelayDecreaseAmount;
        currentMoveDelay = newMoveDelay<minMoveDelay?minMoveDelay:newMoveDelay;
    }
    private void DrawGame(){
        int wormPiecesCount = Worm.WormPieces.Count;
        for (int i = 0; i < wormPiecesCount; i++) {
            Destroy(Instantiate(wormGO, new Vector2(Worm.WormPieces[i].X, Worm.WormPieces[i].Y), Quaternion.identity),currentMoveDelay+0.02f);
        }
        Destroy(Instantiate(appleGO, new Vector2(Apple.X, Apple.Y), Quaternion.identity),currentMoveDelay+0.02f);
    }
    bool wallsInstantiated = false;
    private void DrawWalls(){
        if(wallsInstantiated){
            return;
        }
        for(int i = 0; i <= Width; i++){
            for(int j = 0; j <= Height; j++){
                if(j==0 || j == Height || i==0 || i==Width){
                    walls.Add(new Wall(i, j));
                    Instantiate(wallGO, new Vector2(i, j), Quaternion.identity);
                }
            }
        }
        wallsInstantiated = true;
    }
    private void HandleInput(){
        if(Input.GetKeyDown(KeyCode.A)){
            Worm.SetDirection(Direction.LEFT);
        } else if(Input.GetKeyDown(KeyCode.D)){
            Worm.SetDirection(Direction.RIGHT);
        } else if(Input.GetKeyDown(KeyCode.W)){
            Worm.SetDirection(Direction.UP);
        } else if(Input.GetKeyDown(KeyCode.S)){
            Worm.SetDirection(Direction.DOWN);
        }
    }
    private void CreateApple(){
        bool compatibleLocation = true;
        int x = 0;
        int y = 0;
        while(true){
            x = Utils.Random.Next(1, Width-1);
            y = Utils.Random.Next(1, Height-1);
            int wormPiecesCount = Worm.WormPieces.Count;
            Piece newPiece = new Piece(x,y);
            for (int i = 0; i < wormPiecesCount; i++) {
                if(newPiece.InTheSamePositionOf(Worm.WormPieces[i])) {
                    Debug.Log("CreateApple, newPiece="+newPiece+" is in the same position of Worm.WormPieces[i]="+Worm.WormPieces[i]+" => compatibleLocation=false");
                    compatibleLocation = false;
                }
            }
            if(compatibleLocation){
                break;
            }
        }
        Apple = new Apple(x, y);
    }
    
}
