using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private int x;
    private int y;

    public bool isAnimated = false; // flag for matched row animation

    public int X{
        get { return x; }
        set {
            if (IsMovable()){
                x = value;
            }
        }
    }

    public int Y{
        get { return y; }
        set {
            if (IsMovable()){
                y = value;
            }
        }
    }

    private Grid.PieceType type;

    public Grid.PieceType Type{
        get { return type; }
    }

    private Grid grid;

    public Grid GridRef{
        get { return grid; }
    }

    public MoveCounter moveCounter;
    
    public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type){
        x = _x; y = _y; grid = _grid; type = _type;
        isAnimated = false;
    }

    void OnMouseEnter(){
        if(!MoveCounter.isGameOver)
            grid.FirstItem(this);
    }

    void OnMouseDown(){
        if(!MoveCounter.isGameOver)
            grid.SecondItem(this);
    }

    void OnMouseUp(){
        if(!MoveCounter.isGameOver)
            grid.ReleaseItems();
    }

    private MoveItem movableComponent;

    public MoveItem MovableComponent{
        get { return movableComponent; }
    }

    private ColoredItem colorComponent;

    public ColoredItem ColorComponent{
        get { return colorComponent; }
    }

    void Awake() {
        movableComponent = GetComponent<MoveItem>();
        colorComponent = GetComponent<ColoredItem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsMovable()
    {
        return movableComponent != null && type != Grid.PieceType.CHECK_MARK;
    }


    public bool IsColored(){
        return colorComponent != null;
    }
}
