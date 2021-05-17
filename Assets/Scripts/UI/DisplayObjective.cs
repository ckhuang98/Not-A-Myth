using UnityEngine;

public class DisplayObjective : MonoBehaviour
{
    public string objectiveText;
    public float displayTimer = 1.5f;

    private UI ui;

    void Start()
    {
        StartCoroutine(UI.instance.displayerPlayerUpdate(objectiveText, displayTimer));
    }
}
