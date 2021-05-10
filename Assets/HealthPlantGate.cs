using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlantGate : MonoBehaviour
{

    public Gate gate;
    public GameObject[] healthPlants;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < healthPlants.Length; i++) {
            if(healthPlants[i] == null) {
                gate.unlocked = true;
            }
        }
    }
}
