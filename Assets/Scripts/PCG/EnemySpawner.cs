using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private RoomTemplates templates;

    private bool spawned = false;

    private int rand;
    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }


    void Spawn()
    {
        if(spawned == false){
            rand = Random.Range(0, templates.enemies.Length);
            Instantiate(templates.enemies[rand], transform.position, Quaternion.identity);
        }

        spawned = true;
    }
}
