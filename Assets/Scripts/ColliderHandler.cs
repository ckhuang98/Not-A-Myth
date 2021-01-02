using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{

    //Shamelessly stolen from 

    //https://answers.unity.com/questions/575363/how-can-i-change-my-polygon-collider-to-correctly.html
    //made by user basavaraj_guled
    

    /* Basically the whole purpose of this script is to draw a collider box
     * around each frame of an animation. For our game, I use this with the
     * attack animation so that the only collider is there when the animation
     * is actually on screen. I use an additional method within 
     * (PlayerController --> attackInDirection) to make sure all of the 
     * colliders are disabled after a short while.
     * I'm not sure exactly what this script does, but I could probably puzzle
     * it out if we need. We may need to find some other way of handling 
     * collision detection anyway. 
     */





    public bool iStrigger = true;
    //public PhysicsMaterial2D _material ;

    private SpriteRenderer spriteRenderer;
    private List<Sprite> spritesList;
    private Dictionary<int, PolygonCollider2D> spriteColliders;
    private bool _processing;

    private int _frame;
    public int Frame {
        get { return _frame; }
        set {
            if (value != _frame) {
                if (value > -1) {
                    spriteColliders[_frame].enabled = false;
                    _frame = value;
                    spriteColliders[_frame].enabled = true;
                }
                else {
                    _processing = true;
                    StartCoroutine(AddSpriteCollider(spriteRenderer.sprite));
                }
            }
        }
    }

    private IEnumerator AddSpriteCollider(Sprite sprite) {
        spritesList.Add(sprite);
        int index = spritesList.IndexOf(sprite);
        PolygonCollider2D spriteCollider = gameObject.AddComponent<PolygonCollider2D>();
        spriteCollider.isTrigger = iStrigger;
        //    spriteCollider.sharedMaterial = _material;
        spriteColliders.Add(index, spriteCollider);
        yield return new WaitForEndOfFrame();
        Frame = index;
        _processing = false;
    }

    private void OnEnable() {
        spriteColliders[Frame].enabled = true;
    }

    private void OnDisable() {
        spriteColliders[Frame].enabled = false;
    }

    private void Awake() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        spritesList = new List<Sprite>();

        spriteColliders = new Dictionary<int, PolygonCollider2D>();

        Frame = spritesList.IndexOf(spriteRenderer.sprite);
    }

    private void LateUpdate() {
        if (!_processing)
            Frame = spritesList.IndexOf(spriteRenderer.sprite);
    }



    


}
