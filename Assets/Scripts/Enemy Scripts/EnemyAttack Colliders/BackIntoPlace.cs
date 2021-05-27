using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackIntoPlace : MonoBehaviour
{
    private float x;
    private float y;
    // Start is called before the first frame update
    void Start()
    {
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(x);
        //Debug.Log(y);
        if (this.transform.localPosition.x != x || this.transform.localPosition.y != y) {
            this.transform.localPosition = new Vector3(x, y, 1f);
        } 
    }
}
