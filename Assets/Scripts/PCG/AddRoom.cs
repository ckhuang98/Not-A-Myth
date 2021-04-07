using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Basically all the code comes from this youtube tutorial:
//https://youtu.be/qAf9axsyijY



public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start() {
        templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
    }
}
