using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float smoothSpeed = 10f;

    [SerializeField] private Vector3 offSet;

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate(){
        Vector3  desiredPos = target.position + offSet;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        //transform.LookAt(target);
    }
}