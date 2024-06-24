using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Non-generic base for the Collectible Managers;
/// <br></br> Facilitates dependency (controller) injection;
/// </summary>
public abstract class CollectibleManager : MonoBehaviour {

	protected CollectibleController controller;
	protected Transform canvasTransform;
	public virtual void Init(CollectibleController controller) {
		this.controller = controller;
		canvasTransform = controller.Transition.transform.parent;
	}
}

public abstract class CollectibleManager<T> : CollectibleManager where T : ScriptableItem, ItemData {

	protected HashSet<ItemData> inventory = new();

	protected enum State {
		Idle,
		Start,
		Reveal,
		Await,
		End,
	} protected State state = State.Idle;

	public override void Init(CollectibleController controller) {
		base.Init(controller);
		controller.OnClaimCollectible += Controller_OnClaimCollectible;
        controller.OnInventoryRequest += Controller_OnInventoryRequest;
	}

    /// <summary> Display an item to the screen; </summary>
    /// <param name="item"> Item to display; </param>
    public abstract void Display(T item, float transitionTime = 0);

	/// <summary> Listener to item claim calls; </summary>
	private void Controller_OnClaimCollectible(object sender, ItemCall call) {
		T item = call.input as T; //If proper manager, then will work properly; otherwise null;
		if (item != null && AddItem(item)) {
			call.output = inventory;
			Display(item, controller.InSequence ? 0f : 1f);
		}
	}

	private void Controller_OnInventoryRequest(object sender, ItemCall call) {
		if (call.inputType == typeof(T)) {
			call.output = inventory;
        }
	}

	/// <summary> Add an item to the inventory; </summary>
	/// <param name="item"> Item to receive; </param>
	/// <returns> True if the item is added, false if it was already present; </returns>
	protected virtual bool AddItem(T item) => inventory.Add(item);
}
