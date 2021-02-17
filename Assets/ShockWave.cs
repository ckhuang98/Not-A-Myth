using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private float timer = 5.0f;
    //Transform transform = GameObject.GetComponent<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0.0f) {
            timer -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
        Vector3 newScale = transform.localScale;
        newScale *= 1.006f;
        transform.localScale = newScale;
        
    }
}
