/*
    Some basic camera panning usage seen here:
    https://www.youtube.com/watch?v=R6scxu1BHhs&ab_channel=TheGameDevShack
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraPan : MonoBehaviour
{
    // Inspector field for attaching the camera
    [SerializeField]
    private Camera cam;

    // Inspector field for parent (holder) of camera
    [SerializeField]
    private GameObject holder;

    // The original position of the camera (centered at Player)
    [SerializeField]
    private Vector2 player_cam_pos;

    // Scene string allows definition of pan locations for scenes
    [SerializeField]
    private string scene;

    /* Panning locations (camera will center at these coords)
     * Maximum of four panning locations for the camera to visit
     * All instantiated to "negativeInfinity" by default
     * Camera will only visit coords which != "negativeInfinity"
     */
    [SerializeField]
    private Vector3[] panCoords =
    {
        Vector3.negativeInfinity,
        Vector3.negativeInfinity,
        Vector3.negativeInfinity,
        Vector3.negativeInfinity
    };

    // Timer field for controlling when the camera pans
    [SerializeField]
    private float panTimer = 0f;

    // Inspector field to adjust zoom
    [SerializeField]
    private float zoomStep;


    void Start()
    {
        // Get the camera's parent (holder)
        holder = cam.transform.parent.gameObject;
        
        // Set the camera's original position (centered at Player)
        player_cam_pos = cam.transform.position;
        
        // The scene name will influence the panning of the camera
        Scene current_scene = SceneManager.GetActiveScene();
        scene = current_scene.name;

        // The first level will only require a pan upwards
        if (scene == "3-16 Tutorial Scene")
        {
            panCoords[0] = new Vector3(-1 * (float)66.72, -1 * (float)115);
        }

        PanZoomCamera();
    }

    private void PanZoomCamera()
    {
        // ZoomOut();

        for (int i = 0; i < panCoords.Length; i++)
        {
            // Get the coordinate we'll be moving to
            Vector3 next = panCoords[i];

            // Break iteration if next == negativeInfinity
            if (next.x == float.NegativeInfinity) { break; }

            // Wait for >= 1.5 seconds before proceeding to pan
            while (panTimer < 5.0f) { panTimer += Time.deltaTime; }

            cam.transform.position = next;

            /*
            // Get the vector difference of current pos. & next
            Vector3 diff = cam.transform.position - next;

            // Get the camera step amount
            Vector3 step = diff / 10f;

            // Move in 10 smaller moves, each taking 0.5 seconds
            for (int j = 0; j < 10; j++)
            {
                // Wait for 0.1 seconds before proceeding to pan
                while (panTimer < 0.5f) { panTimer += Time.deltaTime; }
                
                // Move the camera a step
                cam.transform.position += step;
                
                // Reset the pan timer for the next iteration
                panTimer = 0f;
            }
            */
        }

        // ZoomIn();
    }

    public void ZoomOut() { cam.orthographicSize += zoomStep; }
    public void ZoomIn() { cam.orthographicSize -= zoomStep; }
}