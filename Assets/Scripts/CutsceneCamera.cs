using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneCamera : MonoBehaviour
{
    public Camera mainCam;

    public GameObject[] views;

    private int sceneNum = 0;

    private int nextScene;
    // Start is called before the first frame update
    void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nextScene);
        if(Input.GetKeyDown(KeyCode.Space)){
            if(sceneNum == views.Length - 1){
                if(nextScene == 10){
                    SceneManager.LoadScene(0);
                } else{
                    SceneManager.LoadScene(nextScene);
                }
            } else{
                sceneNum++;
                resetZoom();
            }
        }

        if(Input.GetKeyDown(KeyCode.Backspace)){
            if(nextScene == 10){
                SceneManager.LoadScene(0);
            } else{
                SceneManager.LoadScene(nextScene);
            }
        }
        zoomOut();
    }

    void LateUpdate(){
        mainCam.transform.position = Vector3.Lerp(transform.position, views[sceneNum].transform.position, Time.deltaTime * 5f);
    }

    void zoomOut(){
        if(mainCam.orthographicSize < 7.5f){
            mainCam.orthographicSize = mainCam.orthographicSize + 0.1f *Time.deltaTime;
        }
        if(mainCam.orthographicSize == 7.5f){
            mainCam.orthographicSize = 7.5f;
        }
    }

    void resetZoom(){
        mainCam.orthographicSize = 5f;
    }


}
