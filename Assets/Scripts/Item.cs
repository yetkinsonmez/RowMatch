using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    private int x;
    private int y;

    public int X{
        get { return x; }
    }

    public int Y{
        get { return y; }
    }

    private Grid.PieceType type;

    public Grid.PieceType Type{
        get { return type; }
    }

    private Grid grid;

    public Grid GridRef{
        get { return grid; }
    }

    public void Init(int _x, int _y, Grid _grid, Grid.PieceType _type){
        x = _x; y = _y; grid = _grid; type = _type;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
