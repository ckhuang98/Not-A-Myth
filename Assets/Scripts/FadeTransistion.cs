using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransistion : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doFade(){
        image.enabled = true;
        StartCoroutine(Fade(image));
    }

    private IEnumerator Fade(Image i){
        i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a);

        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / 1));
            //artText.color = i.color;
            yield return null;
        }

        GameMaster.instance.loadScene();
    }
}
