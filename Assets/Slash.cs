using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= 1.003f;
        newScale.y *= 1.001f;
        transform.localScale = newScale;
        
        Vector3 newPos = transform.position;
        newPos.y -= .015f;
        transform.position = newPos;

        if (transform.position.y <= -6.5f) {
            Destroy(this.gameObject);
        }        
    }
    /*
    void OnTriggerEnter2D() {
        Debug.Log("touched");
    }
    */
}
