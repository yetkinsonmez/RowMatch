using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum PieceType{
        NORMAL,
        COUNT,
    };

    public int xDimension;
    public int yDimension;

    [System.Serializable]
    public struct PiecePrefab
    {
      public PieceType type;
      public GameObject prefab;  
    };

    public GameObject backgroundPrefab;
    public PiecePrefab[] piecePrefabs;

    private Dictionary<PieceType, GameObject> piecePrefabDict;
    

    private Item[,] pieces; // 2D array of objects

    public Vector2 Center(int x, int y){
        return new Vector2(transform.position.x - (float)xDimension/2f + x + 0.5f,
        transform.position.y + (float)yDimension/2f - y - 0.5f);
    }



    // Start is called before the first frame update
    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>(); 

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDimension; x++){
            for (int y = 0; y < yDimension; y++){
                GameObject background = (GameObject)Instantiate(backgroundPrefab, Center(x, y), Quaternion.identity);
                background.transform.parent = transform;
            }
        }

        pieces = new Item[xDimension, yDimension];

        for (int x = 0; x < xDimension; x++){
            for (int y = 0; y < yDimension; y++){
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], Center(x, y), Quaternion.identity);
                newPiece.name = "Piece(" + x + "," + y + ")"; 
                newPiece.transform.parent = transform;

                pieces[x, y] = newPiece.GetComponent<Item>();
                pieces[x, y].Init(x, y, this, PieceType.NORMAL);

                if (pieces[x, y].IsMovable()) {
                    pieces[x, y].MovableComponent.Move(x, y);
                }

                if (pieces[x, y].IsColored()) {
                    pieces[x, y].ColorComponent.SetColor((ColoredItem.ColorType)Random.Range(0, pieces[x, y].ColorComponent.NumColors));
                }
            }
        }

 
    }


    // Update is called once per frame
    void Update(){

    }

    
}