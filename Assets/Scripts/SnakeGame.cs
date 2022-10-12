using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SnakeGame : MonoBehaviour {
    [SerializeField]private int Width = 15;
    [SerializeField]private int Height = 10;
    private Snake Snake;
    private Piece Apple;
    private bool GameStarted;
    [SerializeField] private float currentMoveDelay = 1;
    private float currentTimer;
    public float minMoveDelay = 0.25f;
    public float initialMoveDelay = 2f;
    public float moveDelayDecreaseAmount = 0.05f;
    public GameObject appleGO;
    public GameObject snakeGO;
    public GameObject wallGO;
    List<Piece> walls = new List<Piece>();

    private void Start() {
        Camera.main.transform.position = new Vector3(Width/2, Height/2, -10);
        InitializeGame();
    }
    private void InitializeGame(){
        if(Snake==null){
            Debug.Log("Instantiate Snake!");
            Snake = Instantiate(snakeGO, new Vector2(Width/2, Height/2), Quaternion.identity).GetComponent<Snake>();
        } else {
            Debug.Log("Reset Snake!");
            Snake.Reset();
        }
        GameStarted = true;
        DrawWalls();
        CreateApple();
        currentTimer = 0;
    }
    private void Update() {
        if(!GameStarted){
            InitializeGame();
            return;
        }
        currentTimer += Time.unscaledDeltaTime;
        if(currentTimer>currentMoveDelay && Snake!=null){
            currentTimer=0;
            Snake.Move();
            if(Snake.RunsIntoItself() || WormRunIntoWall){
                GameStarted = false;
                return;
            }
            if(Snake.RunsInto(Apple)){
                Snake.Grow();
                CreateApple();
            }
            HandleMoveDelay();
        }
        HandleInput();
    }
    private bool WormRunIntoWall{
        get{
            if(Snake==null){
                Debug.Log("SnakeRunIntoWall, Snake is null => return true.");
                return true;
            }
            if(wallsInstantiated){
                int wallsCount = walls.Count;
                for (int i = 0; i < wallsCount; i++) {
                    if(Snake.RunsInto(walls[i])){
                        Debug.Log("SnakeRunIntoWall, Snake runs into wall piece="+walls[i] +" => return true.");
                        return true;
                    }
                }
            }
            return false;
        }
    }
    private void HandleMoveDelay(){
        float newMoveDelay = initialMoveDelay - Snake.Length*moveDelayDecreaseAmount;
        currentMoveDelay = newMoveDelay<minMoveDelay?minMoveDelay:newMoveDelay;
    }
    bool wallsInstantiated = false;
    private void DrawWalls(){
        if(wallsInstantiated){
            return;
        }
        for(int i = 0; i <= Width; i++){
            for(int j = 0; j <= Height; j++){
                if(j==0 || j == Height || i==0 || i==Width){
                    walls.Add(Instantiate(wallGO, new Vector2(i, j), Quaternion.identity).GetComponent<Piece>());
                }
            }
        }
        wallsInstantiated = true;
    }
    private void HandleInput(){
        if(Input.GetKeyDown(KeyCode.A)){
            Snake.SetDirection(Direction.LEFT);
        } else if(Input.GetKeyDown(KeyCode.D)){
            Snake.SetDirection(Direction.RIGHT);
        } else if(Input.GetKeyDown(KeyCode.W)){
            Snake.SetDirection(Direction.UP);
        } else if(Input.GetKeyDown(KeyCode.S)){
            Snake.SetDirection(Direction.DOWN);
        }
    }
    private void CreateApple(){
        int x = 0;
        int y = 0;
        while(true){
            bool compatibleLocation = true;
            x = Utils.Random.Next(1, Width-1);
            y = Utils.Random.Next(1, Height-1);
            int snakePiecesCount = Snake.SnakePieces.Count;

            for (int i = 0; i < snakePiecesCount; i++) {
                if(Snake.SnakePieces[i].InTheSamePositionOf(x,y)) {
                    Debug.Log("CreateApple, location=("+x+","+y+") is in the same position of Snake.SnakePieces[i]="+Snake.SnakePieces[i]+" => compatibleLocation=false");
                    compatibleLocation = false;
                }
            }
            if(compatibleLocation){
                break;
            }
        }
        if(Apple==null){
            Debug.Log("CreateApple, Instantiate Apple!");
            Apple = Instantiate(appleGO, new Vector3(x, y), Quaternion.identity).GetComponent<Piece>();
        } else {
            Debug.Log("CreateApple, Update Apple position!");
            Apple.UpdatePosition(x, y);
        }
    }
    
}
