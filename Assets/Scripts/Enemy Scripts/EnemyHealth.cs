using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().healthAmount;
        if(localScale.x < 0){
            localScale.x = 0;
        }
        transform.localScale = localScale;
    }
}
