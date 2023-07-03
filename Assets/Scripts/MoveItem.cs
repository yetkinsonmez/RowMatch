using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MoveItem : MonoBehaviour
{

    private Item item;

    public float tweenTime = 0.5f;
    void Awake(){
        item = GetComponent<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int xNew, int yNew){
        item.X = xNew;
        item.Y = yNew;

        item.transform.DOLocalMove(item.GridRef.Center(xNew, yNew), tweenTime).SetAutoKill(false)
        .OnComplete(() => Debug.Log("Move complete: item now at (" + item.X + ", " + item.Y + ")"));

    }
}
