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
        //Scale corresponds to the enemy health amount variable in the Enemy script
        localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().healthAmount;
        transform.localScale = localScale;
    }
}
