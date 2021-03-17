using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableWalls : MonoBehaviour
{
    public Text label;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name.Equals("SlashSpriteSheet_0"))
        {
            Debug.Log("Triggered");
            Destroy(this.gameObject);
            label.text = "";
        }
    }
}
