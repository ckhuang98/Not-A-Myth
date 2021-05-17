using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panner : MonoBehaviour
{   
    /*******************************************************************
     *  IN CAMERA, ATTACH PANNER & SET THE FOLLOWING INSPECTOR ELEMENTS:
     *  ----------------------------------------------------------------
     *  1.)     Main_cam                   from    Camera -> Main Camera
     *  2.)     Player                     from    Player -> MC Prefab
     *  3.)     Pan_trans -> Elements 1-4  from    Pan Cams -> Pan(1-4)
     *******************************************************************/
    [SerializeField] Camera main_cam;
    [SerializeField] Transform player;
    [SerializeField] private Transform[] pan_trans = new Transform[4];

    CameraFollow follower;          // Camera follower script
    float panTimer = 1.0f;          // Start cam on player for 1.5s (2.5-1)
    int cam_point = 0;              // Integer for camera iteration

    // Start is called before the first frame update
    void Start()
    {
        // Get the camera's parent (holder)
        GameObject parent = main_cam.transform.parent.gameObject;

        // Get the parent's follow script
        follower = parent.GetComponent<CameraFollow>();

        // Get the player's transform
        player = GameObject.Find("MC Prefab").transform;

        // Zoom the camera out for panning
        ZoomOut();

        // Set the follower's smooth speed to a low 3.0
        follower.smoothSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("timer: " + panTimer + "     pan_count: " + cam_point);

        // Only pan once per 2.5 seconds
        if (panTimer >= 2.5f) {

            // Maximum of four pans
            if (cam_point < 4) {
                follower.target = pan_trans[cam_point++];
                if (pan_trans[cam_point-1] == null) { cam_point = 5; }
                panTimer = 0f;
            }
            else cam_point++;    // 5 = pan back to player
        }
        
        // Keep the timer counting until 5+
        if (cam_point <= 4) {
            panTimer += Time.deltaTime;
        }
        
        // Stop the timer, reset the follower, zoom in
        if (cam_point == 5) {
            cam_point++;
            panTimer = 0f;
            follower.target = player;
            ZoomIn();
        }

        // Reset the follower's smooth speed after return to player
        if (cam_point == 6 && follower.smoothSpeed == 3f) {
            follower.smoothSpeed = 10f;
        }
    }

    public void ZoomOut() { main_cam.orthographicSize += 2; }
    public void ZoomIn() { main_cam.orthographicSize -= 2; }
}
