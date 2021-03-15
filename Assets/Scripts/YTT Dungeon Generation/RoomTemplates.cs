using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Basically all the code comes from this youtube tutorial:
//https://youtu.be/qAf9axsyijY



public class RoomTemplates : MonoBehaviour
{
    public int maxRooms = 5;
    public int timesClosed = 0;
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRooms;

    public List<GameObject> rooms;

    public float waitTime;
    public GameObject door;

    private bool spawnedDoor;

    void Update() {
        if(timesClosed == 4 && spawnedDoor == false){
            Instantiate(door, rooms[rooms.Count-1].transform.position, Quaternion.identity);
            spawnedDoor = true;
        }
    }

}
