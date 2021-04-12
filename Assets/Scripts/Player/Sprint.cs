using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    private List<GameObject> ObjectsInRange = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if(ObjectsInRange != null && ObjectsInRange.Contains(null)){
            ObjectsInRange.RemoveAll(null);
        }
        if(ObjectsInRange.Count == 0){
            stats.inCombat.Value = false;
        } else{
            stats.inCombat.Value = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Hammer Giant") || collider.CompareTag("Fire Eel") || collider.CompareTag("Fire Imp")){
            ObjectsInRange.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider collider) {
        if(collider.CompareTag("Hammer Giant") || collider.CompareTag("Fire Eel") || collider.CompareTag("Fire Imp")){
            ObjectsInRange.Remove(collider.gameObject);
        }
    }
}
