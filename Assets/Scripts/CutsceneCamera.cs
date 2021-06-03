using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneCamera : MonoBehaviour
{
    public Camera mainCam;

    public Text skipText;
    private bool skipReady = false;

    public float skipTimer;
    private float skipTimeRemaining;

    public float sceneTimer;
    private float timeRemaining;

    public float zoomInScale;
    public float zoomOutScale;

    public GameObject[] views;

    public VideoPlayer videoPlayer;

    private int sceneNum = 0;

    private int nextScene;
    // Start is called before the first frame update
    void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        timeRemaining = sceneTimer;
        skipTimeRemaining = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nextScene);

        if (timeRemaining > 0){
            timeRemaining -= Time.deltaTime;
        }

        // if (skipTimeRemaining > 0){
        //     skipTimeRemaining -= Time.deltaTime;
        // } else {
        //     skipReady = false;
        // }

        // if (skipReady) {
        //     skipText.text = "Press Backspace To Skip";
        // } else {
        //     skipText.text = "";
        // }

        // if(Input.GetKeyDown(KeyCode.Space) || timeRemaining <= 0){
        //     if(sceneNum == views.Length - 1){
        //         if(nextScene == 10){
        //             SceneManager.LoadScene(0);
        //         } else{
        //             SceneManager.LoadScene(nextScene);
        //         }
        //     } else{
        //         timeRemaining = sceneTimer;
        //         sceneNum++;
        //         resetZoom();
        //     }
        // }

        if(Input.GetKeyDown(KeyCode.Backspace)){
            if(nextScene == SceneManager.sceneCount){
                SceneManager.LoadScene(0);
            } else{
                SceneManager.LoadScene(nextScene);
            }
        }
        if(( videoPlayer.frame) > 0 && (videoPlayer.isPlaying == false))
        {
            SceneManager.LoadScene(nextScene);
        }

        // if (Input.anyKeyDown){
        //     Debug.Log("A key or mouse click has been detected");
        //     skipReady = true;
        //     skipTimeRemaining = skipTimer;
        // }

        // zoomOut();
    }

    void LateUpdate(){
        // mainCam.transform.position = Vector3.Lerp(transform.position, views[sceneNum].transform.position, Time.deltaTime * 2f);
    }

    void zoomOut(){
        if(mainCam.orthographicSize < zoomOutScale){
            mainCam.orthographicSize = mainCam.orthographicSize + 0.1f *Time.deltaTime;
        }
        if(mainCam.orthographicSize >= zoomOutScale){
            mainCam.orthographicSize = zoomOutScale;
        }
    }

    void resetZoom(){
        mainCam.orthographicSize = zoomInScale;
    }


}
