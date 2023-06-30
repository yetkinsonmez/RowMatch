using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{

    private Item item;

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

    public void Move(int xNew, int yNew, float time){
        item.X = xNew;
        item.Y = yNew;

        item.transform.localPosition = item.GridRef.Center(xNew, yNew);
    }
}
