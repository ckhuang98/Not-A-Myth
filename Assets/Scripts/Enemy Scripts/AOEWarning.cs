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
        StartCoroutine("TurnDark");
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

    void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            inWarning = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        inWarning = false;
    }

    private IEnumerator TurnDark() {
        colorChange.a = .51764f;
        colorChange.r = .329411f;
        colorChange.g = .062745f;
        colorChange.b = .062745f;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnRed");
    }

    private IEnumerator TurnRed() {
        colorChange.a = .51764f;
        colorChange.r = 1f;
        colorChange.g = 0f;
        colorChange.b = 0f;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnDark");
    }

    public bool getWarning() { return inWarning; }
}
