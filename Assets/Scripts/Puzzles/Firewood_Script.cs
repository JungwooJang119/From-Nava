using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firewood_Script : MonoBehaviour
{
    [SerializeField] private Sprite unlitSprite, litSprite;
    public bool isLit;
    public int prevLitStatus;
    public int currLitStatus;
    private bool checkIfRepeat = false;
    private SpriteRenderer render;
    private GameObject light;

    [SerializeField] private GameObject adjFirewood1;
    [SerializeField] private GameObject adjFirewood2;
    [SerializeField] private GameObject adjFirewood3;
    [SerializeField] private GameObject adjFirewood4;

    private Firewood_Script f1;
    private Firewood_Script f2;
    private Firewood_Script f3;
    private Firewood_Script f4;

    //gets the sprite
    void Start() {
        render = GetComponent<SpriteRenderer>();
        light = this.transform.GetChild(0).gameObject;
    }

    //activate firelight effect if the firewood is lit
    void Update() {
        light.SetActive(isLit);
        if (isLit) {
            render.sprite = litSprite;
        }
        if (adjFirewood1 != null) {
            f1 = adjFirewood1.GetComponent<Firewood_Script>();
            if (adjFirewood2 != null) {
                f2 = adjFirewood2.GetComponent<Firewood_Script>();
                if (adjFirewood3 != null) {
                    f3 = adjFirewood3.GetComponent<Firewood_Script>();
                    if (adjFirewood4 != null) {
                        f4 = adjFirewood4.GetComponent<Firewood_Script>();
                    }
                }
            }
        }
        if (isLit) {
            prevLitStatus = 1;
            currLitStatus = 1;
        } else {
            prevLitStatus = 0;
            currLitStatus = 0;
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
            currLitStatus = 1;
            if (prevLitStatus == currLitStatus) {
                print("copy");
                checkIfRepeat = true;
                return;
            }
            prevLitStatus = 1;
        } else if (collision.gameObject.name == ("Iceball_Spell(Clone)")) {
            render.sprite = unlitSprite;
            isLit = false;
            currLitStatus = 0;
            if (prevLitStatus == currLitStatus) {
                print("copy");
                checkIfRepeat = true;
                return;
            }
            prevLitStatus = 0;
        }
        //if (!checkIfRepeat) {
            if (adjFirewood1 != null) {
                f1.ChangeLit();
                if (adjFirewood2 != null) {
                    f2.ChangeLit();
                    if (adjFirewood3 != null) {
                        f3.ChangeLit();
                        if (adjFirewood4 != null) {
                            f4.ChangeLit();
                        }
                    }
                }
            }
        //}
    }

    public void ChangeLit() {
        if (isLit) {
            render.sprite = unlitSprite;
            isLit = false;
            currLitStatus = 0;
            if (prevLitStatus == currLitStatus) {
                print("copy");
                checkIfRepeat = true;
                return;
            }
            prevLitStatus = 0;
            
        } else {
            render.sprite = litSprite;
            isLit = true;
            currLitStatus = 1;
            if (prevLitStatus == currLitStatus) {
                print("copy");
                checkIfRepeat = true;
                return;
            }
            prevLitStatus = 1;
        }
    }
}
