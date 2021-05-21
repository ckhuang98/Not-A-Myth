using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Color tree_c;

    Transform player;
    float player_y;
    float tree_y;
    float y_diff;

    // Start is called before the first frame update
    void Start()
    {
        // Get the tree's sprite renderer
        renderer = gameObject.GetComponent<SpriteRenderer>();
        
        // Get the player's transform & y-value
        player = GameObject.Find("MC Prefab").transform;
        player_y = player.position.y;

        // Get the transform & y-value for the tree
        tree_y = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the sprite renderer's color
        tree_c = renderer.color;
        
        // Update player's y postion & calculate y difference
        player_y = player.position.y;
        y_diff = player_y - tree_y;

        // Handle switching layers for player in front / behind tree
        if (player_y > tree_y) { renderer.sortingOrder = 10; }
        else { renderer.sortingOrder = 0; }

        
        // If the player is within 5 x-units of the tree,
        float player_x = player.position.x;
        float tree_x = gameObject.transform.position.x;
        if ((player_x - 5 <= tree_x) && (tree_x <= player_x + 5))
        {
            /* Change the opacity of the tree if player is behind it
            * While the player is between 0 and 8 units above the tree,
            *  Decrease the opacity (0 = 255f, 8 = 0f)
            * While the player is between 8 and 10 units above the tree,
            *  Increase the opacity (8 = 0f, 10 = 255f) */
            if (y_diff >= 0 && y_diff <= 8) tree_c.a = 1f - (0.125f * y_diff);
            if (y_diff >= 8 && y_diff <= 10) tree_c.a = (0.5f * (y_diff - 8));
        }
        else { tree_c.a = 1f; }

        // Set the renderer's color afterwards
        renderer.color = tree_c;
    }
}
