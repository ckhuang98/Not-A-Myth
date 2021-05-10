using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiantGate : MonoBehaviour
{

    public Gate gate;
    public GameObject[] Giants;
    private int count;
    private bool a = false;
    private bool b = false;
    private bool c = false;
    // Start is called before the first frame update
    void Start()
    {
        count = Giants.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(count <= 0) {
            gate.unlocked = true;
        }

        if (Giants[0] == null) {
            if (a == false) {
                a = true;
                count--;
                Debug.Log("a");
            }
        }
        if (Giants[1] == null) {
            if (b == false) {
                b = true;
                Debug.Log("b");
                count--;
            }
        }
        if (Giants[2] == null) {
            if (c == false) {
                c = true;
                count--;
                Debug.Log("c");
            }
        }
    }
}
