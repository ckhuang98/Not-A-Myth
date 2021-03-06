using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Vector3 localScale;
    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Scale corresponds to the enemy health amount variable in the Enemy script
        if (transform.parent != null && transform.parent.tag == "Hammer Giant") {
            localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().healthAmount;
        } else if (transform.parent != null && transform.parent.tag == "Fire Imp") {
            localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().healthAmount;
        }
        */
        localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().healthAmount;
        
        transform.localScale = localScale;
    }
}
