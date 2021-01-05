using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Equals("Player") && pickedUp == false) {
            collider.GetComponent<PlayerController>().gainStrength();
            var shardObject = this.gameObject;
            shardObject.GetComponent<Renderer>().enabled = false;
            Destroy(shardObject);
            pickedUp = true;
        }
        
    }


}
