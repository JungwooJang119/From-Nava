using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IInteractable : MonoBehaviour
{
    // Collectible rework done by Joseph Seo
    // This class was created to rework tutorial text prompts into one class rather than having each individual class
    // do their own thing.
    // To use, extend this class in the desired place and add a trigger collider to the desired game object.
    // Ex. public class ExampleClass : IInteractable {}
    // Then define InteractBehavior using override to determine how the object should respond to the interaction. 
    // Ex. protected override void InteractBehavior() { DoSomething; }


    private bool isNear = false;                    // Boolean to determine if the player is nearby or not. 
    protected bool awaitingCollectible = false;     // Boolean to check if the interactable object is currently doing something. Protected so that InteractBehavior can change this status.
    private string intKey = "space";                // Key used to trigger interactions.
    [SerializeField] GameObject buttonTutorial;     // Reference to button tutorial pop-up.
    private GameObject _tutInstance;                // Reference to the game object hosting _tutScript.
    private ButtonTutorial _tutScript;               // Reference to instantiated text script.
    protected bool canTrigger = true;
     
    protected float xMod = 0, yMod = 0;             // floats used to modify the current position of the text. Primarily for doors. Protected so that the individual type of IInteractable can determine positioning.

    //Unity functions used to determine when the player enters or leaves the range of the collectible object. 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && canTrigger) {
            isNear = true;
            StartCoroutine(Nearby());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            isNear = false;
        }
    }

    // Coroutine that creates the initial tutorial prompt and and then checks for if the player decides to interact with it or not.
    // If so, call the overriden InteractBehavior in the child class.
    // Once the player leaves range (isNear = false) then fade the tutorial prompt.
    protected IEnumerator Nearby() {
        CreateButtonTutorial();
        while (isNear) {
            if (canTrigger && !awaitingCollectible && Input.GetKeyDown(intKey)) { //what if we make it just can trigger instead of awaitingCollectible?
                InteractBehavior();
            }
            yield return null;
        }
        _tutScript.Fade();
    }

    // Function that creates the tutorial prompt.
    // First destroys the original instance of the tutorial script.
    // Then instantiates a GameObject of the serialized field ButtonTutorial.
    // Finally grabs the ButtonTutorial script and calls SetUp in ButtonTutorial.
    protected void CreateButtonTutorial() {
        if (_tutInstance != null) return;
        _tutInstance = Instantiate(buttonTutorial, transform.position + new Vector3(xMod, yMod, 0), Quaternion.identity);
        _tutScript = _tutInstance.GetComponent<ButtonTutorial>();
        _tutScript.SetUp(intKey, gameObject);
    }

    // Function that fades out the tutorial button, destroying it once it disappears.
    protected void FadeButton() {
        _tutScript.Fade();
    }

    // Behavior defined by the individual class extending IInteractable.
    protected abstract void InteractBehavior();

    
}
