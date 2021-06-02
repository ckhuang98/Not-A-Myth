using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Basically all the code comes from this youtube tutorial:
//https://youtu.be/qAf9axsyijY



public class AddRoom : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start() {
        //templates = GameObject.FindGameObjectWithTag("RoomTemplates").GetComponent<RoomTemplates>();
        //templates.rooms.Add(this.gameObject);
        Decorate(this.gameObject);
    }

    private void Decorate(GameObject room)
    {
        // Grab all of the trees & building spawn points
        GameObject[] trees = GameObject.FindGameObjectsWithTag("TreeSpawnPoint");
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("BuildingSpawnPoint");

        // Load all the sprites for trees & buildings (Prefabs->Resources)
        Sprite[] treeSprites = Resources.LoadAll<Sprite>("Trees");
        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Buildings");

        // For each tree, either set sprite & collider, or set nothing
        foreach (GameObject tree in trees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            BoxCollider2D box = tree.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = treeSprites[Random.Range(0, treeSprites.Length - 1)];
            if (Random.Range(1,11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }

        // For each building, either set sprite & collider, or set nothing
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