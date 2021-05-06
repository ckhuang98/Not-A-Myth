using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEWarning : MonoBehaviour
{
    private bool inWarning;
    private float destroyTime = 1.3f;
    private Color colorChange;
    private SpriteRenderer sr;
    // Start is called before the first frame update

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        colorChange = gameObject.GetComponent<SpriteRenderer>().color;
        StartCoroutine("TurnWhite");
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTime >= 0f) {
            destroyTime -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")){
            inWarning = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D col) {
        if (inWarning == true) {
            inWarning = false;
        }
    }

    private IEnumerator TurnWhite() {
        colorChange = Color.white;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnRed");
    }

    private IEnumerator TurnRed() {
        colorChange = Color.red;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnWhite");
    }

    public bool getWarning() { return inWarning; }
}
