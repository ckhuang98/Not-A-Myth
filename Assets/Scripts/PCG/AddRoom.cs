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
        /*
        List<GameObject> trees = new List<GameObject>();
        List<GameObject> buildings = new List<GameObject>();

        int i = 0, j = 0;
        GameObject[] gameobjs = GetComponents<GameObject>();
        foreach (GameObject child in gameobjs) {
            Debug.Log(child.name);
            if (child.name.Contains("TreeSpawnPoint")) {
                trees.Insert(i, child);
                i += 1;
            }
            if (child.name.Contains("BuildingSpawnPoint")) {
                buildings.Insert(j, child);
                j += 1;
            }
        }
        */
        
        GameObject[] trees = GameObject.FindGameObjectsWithTag("TreeSpawnPoint");
        // GameObject[] trees = GameObject.Find("Tree/TreeSpawnPoint");

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("BuildingSpawnPoint");
        // GameObject[] buildings = GameObject.Find("BuildingSpawnPoint");

        Sprite[] treeSprites = Resources.LoadAll<Sprite>("Trees");
        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Buildings");

        foreach (GameObject tree in trees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            renderer.sprite = treeSprites[Random.Range(0, treeSprites.Length)];
            renderer.enabled = (Random.Range(1,11) <= 7);
        }

        foreach (GameObject building in buildings) {
            SpriteRenderer renderer = building.GetComponent<SpriteRenderer>();
            BoxCollider2D box = building.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = buildingSprites[Random.Range(0, buildingSprites.Length)];
            if (Random.Range(1,11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }
    }
}