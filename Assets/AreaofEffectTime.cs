using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaofEffectTime : MonoBehaviour
{
    private float destroyTime = 3f;
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
