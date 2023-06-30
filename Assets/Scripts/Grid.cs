using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum PieceType{
        NORMAL,
        EMPTY,
        CHECK_MARK, // for the matched row
        COUNT,
    };

    public int xDimension;
    public int yDimension;

    public float fillTime;

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

    private Item firstItem;
    private Item secondItem;

    public Vector2 Center(int x, int y){
        return new Vector2(transform.position.x - (float)xDimension/2f + x + 0.5f,
        transform.position.y + (float)yDimension/2f - y - 0.5f);
    }

    public MoveCounter moveCounter;
    public ScoreManager scoreManager;

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
                
                EnterLevelWithEmptyItems(x, y, PieceType.EMPTY);

                // GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], Center(x, y), Quaternion.identity);
                // newPiece.name = "Piece(" + x + "," + y + ")"; 
                // newPiece.transform.parent = transform;

                // pieces[x, y] = newPiece.GetComponent<Item>();
                // pieces[x, y].Init(x, y, this, PieceType.NORMAL);

                // if (pieces[x, y].IsMovable()) {
                //     pieces[x, y].MovableComponent.Move(x, y);
                // }

                // if (pieces[x, y].IsColored()) {
                //     pieces[x, y].ColorComponent.SetColor((ColoredItem.ColorType)Random.Range(0, pieces[x, y].ColorComponent.NumColors));
                // }
            }
        }

        StartCoroutine(Fill());
 
    }

    public Item EnterLevelWithEmptyItems(int x, int y, PieceType type ){
        GameObject obj = (GameObject)Instantiate(piecePrefabDict[type], Center(x, y), Quaternion.identity);
        obj.transform.parent = transform;

        pieces[x, y] = obj.GetComponent<Item>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
        
    }

    public IEnumerator Fill(){
        while (FillStep())
        {
            yield return new WaitForSeconds(fillTime);
        }
    }

    public bool FillStep(){
        bool movedPiece = false;

        for(int y = yDimension-2; y >= 0; y--) { // fill from the upper row
            for(int x = 0; x < xDimension; x++) {
                Item piece = pieces[x, y];

                if (piece.IsMovable()){
                    Item pieceBelow = pieces[x, y + 1];

                    if (pieceBelow.Type == PieceType.EMPTY){
                        Destroy(pieceBelow.gameObject);

                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;  
                        EnterLevelWithEmptyItems(x , y, PieceType.EMPTY);
                        movedPiece = true;
                    }

                }
            
            }
        }

        for(int x = 0; x < xDimension; x++) {    // for the first row
            Item pieceBelow = pieces[x, 0];

            if (pieceBelow.Type == PieceType.EMPTY){
                
                Destroy(pieceBelow.gameObject);

                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], Center(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;
                
                pieces[x, 0] = newPiece.GetComponent<Item>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor((ColoredItem.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors - 1 ));  // exclude green check mark       
                movedPiece = true;
            }
           
        }

        return movedPiece; 
    }

    // Update is called once per frame
    void Update(){

    }

    public bool IsAdjacent(Item itemFirst, Item itemSecond){
        return (itemFirst.X == itemSecond.X && (int)Mathf.Abs(itemFirst.Y - itemSecond.Y) == 1) 
        || (itemFirst.Y == itemSecond.Y && (int)Mathf.Abs(itemFirst.X - itemSecond.X) == 1);
    }

    public void SwapItems(Item itemFirst, Item itemSecond){
        if (itemFirst.IsMovable() && itemSecond.IsMovable()){
            pieces[itemFirst.X, itemFirst.Y] = itemSecond;
            pieces[itemSecond.X, itemSecond.Y] = itemFirst;

            int itemFirstX = itemFirst.X;
            int itemFirstY = itemFirst.Y;

            itemFirst.MovableComponent.Move(itemSecond.X, itemSecond.Y, fillTime);
            itemSecond.MovableComponent.Move(itemFirstX, itemFirstY, fillTime);

            CheckCompleteRows();
            moveCounter.DecreaseMoveCount();
        }
    }

    public void FirstItem(Item item){
        firstItem = item;
    }
    
    public void SecondItem(Item item){
        secondItem = item;
    }

    public void ReleaseItems(){
        if (IsAdjacent(firstItem, secondItem)){
            SwapItems(firstItem, secondItem);
        }
    }

    public void CheckCompleteRows()
    {
        for (int y = 0; y < yDimension; y++)
        {
            if (RowIsComplete(y))
            {
                UpdateRow(y);
            }
        }
    }

    private bool RowIsComplete(int row)
    {
        ColoredItem.ColorType firstColor = pieces[0, row].ColorComponent.Color;

        for (int x = 1; x < xDimension; x++)
        {
            if (pieces[x, row].ColorComponent.Color != firstColor || pieces[x, row].Type == PieceType.EMPTY)
            {
                return false;
            }
        }

        return true;
    }

    private void UpdateRow(int row)
    {
        for (int x = 0; x < xDimension; x++)
        {   
            //get color of the matched row
            ColoredItem.ColorType colorType = pieces[x, row].ColorComponent.Color;
        
            // replace each item with a Green Check Mark
            Destroy(pieces[x, row].gameObject);

            GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.CHECK_MARK], Center(x, row), Quaternion.identity);
            newPiece.transform.parent = transform;

            pieces[x, row] = newPiece.GetComponent<Item>();
            pieces[x, row].Init(x, row, this, PieceType.CHECK_MARK);
            pieces[x, row].ColorComponent.SetColor(ColoredItem.ColorType.CheckMark); // set color to CheckMark

            scoreManager.AddScore(colorType);
        }
    }

}