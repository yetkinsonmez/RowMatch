using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredItem : MonoBehaviour
{
    public enum ColorType{
        Yellow, Red, Green, Blue, Any, Count
    };

    [System.Serializable]
    public struct ColorSprite{
        public ColorType color;
        public Sprite sprite;
    }

    public ColorSprite[] colorSprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
