using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAngle : MonoBehaviour
{
    public float angle;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        if (angle < 0.0f ) {
            angle = angle + 360f;
        } 
    }
}
