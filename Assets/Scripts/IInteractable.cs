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
    private ButtonTutorial _tutScript;              // Reference to instantiated text script.
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
            if (CanInteract() && Input.GetKeyDown(intKey)) { //what if we make it just can trigger instead of awaitingCollectible?
                InteractBehavior();
            }
            yield return null;
        }
        FadeButton();
    }


    // Helper function done to check if the player should be able to interact with the object
    // Time.timeScale != 0 is done to check if the game is paused or not. If pause implementation changes in the future, this will break
    // canTrigger is done for lasers. 
    // AwaitingColectible is checked to see if we're in the middle of a collectible popup.
    private bool CanInteract() {
        return Time.timeScale != 0 && canTrigger && !awaitingCollectible;
    }

    // Function that creates the tutorial prompt.
    // First destroys the original instance of the tutorial script.
    // Then instantiates a GameObject of the serialized field ButtonTutorial.
    // Finally grabs the ButtonTutorial script and calls SetUp in ButtonTutorial.
    protected void CreateButtonTutorial() {
        // This first line ensures that we are not trying to create a ButtonTutorial when the player is not next to it.
        // This can happen during Chest, Door, etc.
        if (!isNear) return;

        // If we already have a tutInstance instantiated, then we'll keep using this.
        // We cancel fade in the instance that the script is fading but not destroyed, and we need to reinstantiate it.
        if (_tutInstance != null) {
            _tutScript.CancelFade(); 
            return;
        }
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
