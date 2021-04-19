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
        Decorate(this.gameObject);
    }

    private void Decorate(GameObject room)
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("TreeSpawnPoint");
        Sprite[] treeSprites = Resources.LoadAll<Sprite>("Trees");

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("BuildingSpawnPoint");
        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Buildings");

        foreach (GameObject tree in trees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            renderer.sprite = treeSprites[Random.Range(0, treeSprites.Length - 1)];
            renderer.enabled = (Random.Range(1,11) <= 7);
        }

        foreach (GameObject building in buildings) {
            SpriteRenderer renderer = building.GetComponent<SpriteRenderer>();
            BoxCollider2D box = building.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = buildingSprites[Random.Range(0, buildingSprites.Length - 1)];
            if (Random.Range(1,11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }
    }
}