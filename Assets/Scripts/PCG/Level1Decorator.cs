using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Decorator : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Decorate();
    }

    private void Decorate() {

        GameObject[] NormalTrees = GameObject.FindGameObjectsWithTag("NormalTreeSpawnPoint");
        // GameObject[] trees = GameObject.Find("Tree/TreeSpawnPoint");

        GameObject[] IceTrees = GameObject.FindGameObjectsWithTag("IceTreeSpawnPoint");
        // GameObject[] trees = GameObject.Find("Tree/TreeSpawnPoint");

        GameObject[] SnowTrees = GameObject.FindGameObjectsWithTag("SnowTreeSpawnPoint");
        // GameObject[] trees = GameObject.Find("Tree/TreeSpawnPoint");

        GameObject[] buildings = GameObject.FindGameObjectsWithTag("BuildingSpawnPoint");
        // GameObject[] buildings = GameObject.Find("BuildingSpawnPoint");

        Sprite[] NormalTreeSprites = Resources.LoadAll<Sprite>("NormalTrees");
        Sprite[] IceTreeSprites = Resources.LoadAll<Sprite>("IceTrees");
        Sprite[] SnowTreeSprites = Resources.LoadAll<Sprite>("SnowTrees");

        Sprite[] buildingSprites = Resources.LoadAll<Sprite>("Buildings");

        foreach (GameObject tree in NormalTrees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            BoxCollider2D box = tree.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = NormalTreeSprites[Random.Range(0, NormalTreeSprites.Length)];
            if (Random.Range(1, 11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }

        foreach (GameObject tree in IceTrees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            BoxCollider2D box = tree.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = IceTreeSprites[Random.Range(0, IceTreeSprites.Length)];
            if (Random.Range(1, 11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }

        foreach (GameObject tree in SnowTrees) {
            SpriteRenderer renderer = tree.GetComponent<SpriteRenderer>();
            BoxCollider2D box = tree.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = SnowTreeSprites[Random.Range(0, SnowTreeSprites.Length)];
            if (Random.Range(1, 11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }


        foreach (GameObject building in buildings) {
            SpriteRenderer renderer = building.GetComponent<SpriteRenderer>();
            BoxCollider2D box = building.GetComponent<BoxCollider2D>();
            renderer.enabled = true;
            box.enabled = true;
            renderer.sprite = buildingSprites[Random.Range(0, buildingSprites.Length)];
            if (Random.Range(1, 11) <= 5) {
                renderer.enabled = false;
                box.enabled = false;
            }
        }


    }
}
