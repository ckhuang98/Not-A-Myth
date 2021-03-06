using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallTimer : MonoBehaviour
{
    private float timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }
}
