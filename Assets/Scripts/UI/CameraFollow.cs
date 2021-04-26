using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float smoothSpeed = 10f;

    [SerializeField] private Vector3 offSet = new Vector3(0, 0.5f, -1.5f);

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        offSet = new Vector3(GameMaster.instance.playerStats.cameraOffsetX.Value, GameMaster.instance.playerStats.cameraOffsetY.Value, -1.5f);
    }

    void FixedUpdate(){
        Vector3  desiredPos = target.position + offSet;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        //transform.LookAt(target);
    }
}