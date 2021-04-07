using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Basically all the code comes from this youtube tutorial:
//https://youtu.be/qAf9axsyijY



public class RoomTemplates : MonoBehaviour
{
    public int maxRooms = 5;
    public int timesClosed = 0;
    public bool lastRoomClosed = false;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRooms;

    public List<GameObject> rooms;

    public float waitTime;
    public GameObject door;

    public GameObject[] enemies;

    private bool spawnedDoor;

    void Update() {
        if(timesClosed >= 4 && spawnedDoor == false){
            GameObject doorLocation = rooms[rooms.Count-1].transform.Find("DoorSpawnPoint").gameObject;
            // if(lastRoomClosed){
            //     Instantiate(door, rooms[rooms.Count-2].transform.position, Quaternion.identity);
            // } else{
            //     Instantiate(door, rooms[rooms.Count-1].transform.position, Quaternion.identity);
            // }
            Instantiate(door, doorLocation.transform.position, Quaternion.identity);
            spawnedDoor = true;
        }
    }

}
