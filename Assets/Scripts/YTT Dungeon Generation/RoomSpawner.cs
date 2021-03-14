using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Basically all the code comes from this youtube tutorial:
//https://youtu.be/qAf9axsyijY



public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    //1 --> Need bottom door
    //2 --> Need top door
    //3 --> Need left door
    //4 --> Need right door




    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    private GameObject grid;

    // public float waitTime = 4f;

    private void Start() {
        // Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        grid = GameObject.FindWithTag("Grid");
        Invoke("Spawn", 0.1f);
    }

    private void Spawn() {
        if(spawned == false) {
            if (openingDirection == 1) {
                //need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                GameObject room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                room.transform.parent = grid.transform;
            }
            else if (openingDirection == 2) {
                //need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length);
                GameObject room = Instantiate(templates.topRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                room.transform.parent = grid.transform;
            }
            else if (openingDirection == 3) {
                //need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                GameObject room = Instantiate(templates.leftRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                room.transform.parent = grid.transform;
            }
            else if (openingDirection == 4) {
                //need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                GameObject room = Instantiate(templates.rightRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                room.transform.parent = grid.transform;
            }
            spawned = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("RoomSpawnPoint") ) {
            if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false) {
                //spawn walls blocking off any openings 
                Instantiate(templates.closedRooms, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }

}
