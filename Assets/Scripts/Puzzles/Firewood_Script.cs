using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewood_Script : MonoBehaviour
{
    [SerializeField] private Sprite unlitSprite, litSprite;
    public bool isLit = false;
    private SpriteRenderer render;

    //gets the sprite
    void Start() {
        render = GetComponent<SpriteRenderer>();
        if (isLit) {
            render.sprite = litSprite;
        }
    }

    /*unfortunately i was unable to get the trigger colliders of fireball to work with a regular collider, so i've added another 
    circle collider and set it to trigger to make this work. Enemies aren't working so not sure what to do about that :/.
    The trigger attached to firewood will check for collisions by anything. If it's FIreball or Iceball, then it will light up
    or unlight.
    */
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == ("Fireball_Spell(Clone)")) {
            render.sprite = litSprite;
            isLit = true;
        }
        if (collision.gameObject.name == ("Iceball_Spell(Clone)")) {
            render.sprite = unlitSprite;
            isLit = false;
        }
    }
}
