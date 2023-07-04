using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // for Enum.Any()
using UnityEngine.SceneManagement;
using DG.Tweening;


public class Grid : MonoBehaviour
{
    public enum PieceType{
        NORMAL,
        CHECK_MARK, // for the matched row
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


    public MoveCounter moveCounter;
    public ScoreManager scoreManager;

    public Button cancelButton; 

    private void Awake()
    {
        cancelButton.onClick.AddListener(GoToMainMenu);
    }

    public Vector2 Center(int x, int y){
        return new Vector2(transform.position.x - (float)xDimension/2f + x + 0.5f,
        transform.position.y + (float)yDimension/2f - y - 0.5f);
    }


    // Start is called before the first frame update
    void Start()
    {

        
        xDimension = PlayerPrefs.GetInt("GridWidth", 8);
        yDimension = PlayerPrefs.GetInt("GridHeight", 8);
        MoveCounter.isGameOver = false;
        
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
                    pieces[x, y].ColorComponent.SetColor((ColoredItem.ColorType)Random.Range(0, pieces[x, y].ColorComponent.NumColors - 1));
                }
            }
        }

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

            itemFirst.MovableComponent.Move(itemSecond.X, itemSecond.Y);
            itemSecond.MovableComponent.Move(itemFirstX, itemFirstY);

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
                StartCoroutine(UpdateRow(y));
            }
        }
    }

    private bool RowIsComplete(int row)
    {
        ColoredItem.ColorType firstColor = pieces[0, row].ColorComponent.Color;

        for (int x = 1; x < xDimension; x++)
        {
            if (pieces[x, row].ColorComponent.Color != firstColor)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator UpdateRow(int row)
    {
        for (int x = 0; x < xDimension; x++)
        {   
            // get color of the matched row
            ColoredItem.ColorType colorType = pieces[x, row].ColorComponent.Color;
            
            // replace each item with a Green Check Mark
            Destroy(pieces[x, row].gameObject);

            GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.CHECK_MARK], Center(x, row), Quaternion.identity);
            newPiece.transform.parent = transform;

            pieces[x, row] = newPiece.GetComponent<Item>();
            pieces[x, row].Init(x, row, this, PieceType.CHECK_MARK);
            pieces[x, row].ColorComponent.SetColor(ColoredItem.ColorType.CheckMark); // set color to CheckMark

            newPiece.transform.DOScale(1.2f, 0.5f).SetLoops(2, LoopType.Yoyo);

            scoreManager.AddScore(colorType);

            yield return new WaitForSeconds(0.0625f);
        }
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}