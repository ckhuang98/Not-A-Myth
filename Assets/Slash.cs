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
        newScale *= 1.03f;
        transform.localScale = newScale;
        
        Vector3 newPos = transform.position;
        newPos.y -= .15f;
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
