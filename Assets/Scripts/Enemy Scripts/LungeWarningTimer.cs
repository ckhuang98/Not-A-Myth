using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeWarningTimer : MonoBehaviour
{
    private float destroyTime = .9f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTime >= 0f) {
            destroyTime -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }
}
