using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate_Script : MonoBehaviour
{
    [SerializeField] private Sprite unpressed, pressed;
    public bool isPressed = false;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private float setTime;
    private bool isChair = false;
    private float time = 0;
    private int numObject = 0;
    private Collider2D chairCollider;

    void Update() {
        if (isChair) {
            time += Time.deltaTime;
        }
        if (time >= setTime) {
            isChair = false;
            time = 0;
            numObject--;
            OnTriggerExit2D(chairCollider);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell") {
            return;
        }
        numObject++;
        if (other.attachedRigidbody.mass > 1){
            render.sprite = pressed;
            isPressed = true;
        }
        if (other.tag == "Chair") {
            if (time > 0) {
                numObject -= 2;
            }
            time = 0;
            isChair = true;
            chairCollider = other;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        numObject--; 
        if (numObject == 0) {
            isPressed = false;
            render.sprite = unpressed;
        }
    }

    public bool getIsPressed() {
        return isPressed;
    }
}