using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderHandler : MonoBehaviour
{

    //Shamelessly stolen from 

    //https://answers.unity.com/questions/575363/how-can-i-change-my-polygon-collider-to-correctly.html
    //made by user basavaraj_guled


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
        //if(spritesList.Count() < 7) { 
        spritesList.Add(sprite);
        int index = spritesList.IndexOf(sprite);
        PolygonCollider2D spriteCollider = gameObject.AddComponent<PolygonCollider2D>();
        spriteCollider.isTrigger = iStrigger;
        //    spriteCollider.sharedMaterial = _material;
        //if(spriteColliders.Count() < 6) {
            spriteColliders.Add(index, spriteCollider);
        //}
        
        yield return new WaitForEndOfFrame();
        Frame = index;
        _processing = false;
        //}
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
        //if (spritesList.Count() == 7) {
                spritesList.RemoveAt(spritesList.Count() - 1);
        //}
        //if (spritesList.Count() == 8) {
           // Debug.Log("removing last element");
        //} 
        if (!_processing)
            Frame = spritesList.IndexOf(spriteRenderer.sprite);
        
    }



    


}
