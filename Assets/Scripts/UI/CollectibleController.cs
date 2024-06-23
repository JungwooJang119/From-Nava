using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory Manager. Handles interactions with collectible (Lab Reports, Tutorials, Polaroid, etc);
/// <br></br> Responsible for fading in and out of the showcases and communicating success to the caller.
/// </summary>
public class CollectibleController : MonoBehaviour {

	/// <summary> Signal a collectible interaction;
	/// <br></br> Listeners: All Collectible Managers; </summary>
	public event System.EventHandler<ItemCall> OnClaimCollectible;
	/// <summary> Follow-up signal with the result of a collectible interaction;
	/// <br></br> Listener: Notification Manager, PauseMenu; </summary>
	public event System.Action<ItemCall> OnClaimResult;
	/// <summary> Request a check on the posession of an item;
	/// <br></br> Listeners: All Collectible Managers; </summary>
	public event System.EventHandler<ItemCall> OnInventoryRequest;
	/// <summary> Signal all collectible interactions have ended;
	/// <br></br> Listeners: All Collectible Managers, Notification Manager, PauseMenu; </summary>
	public event System.Action OnCallsEnd;

	[SerializeField] private TransitionManager transition;
	public TransitionManager Transition => transition;
	private readonly Queue<ItemData> callQueue = new();

	/// <summary> Whether a collectible interaction is currently being displayed; </summary>
	public bool IsBusy { get; private set; }
	/// <summary> Whether a sequence of interactions is taking place; </summary>
	public bool InSequence { get; private set; }

	void Awake() {
		foreach (CollectibleManager cm in GetComponentsInChildren<CollectibleManager>(true)) { cm.Init(this); };
		GetComponentInChildren<NotificationManager>(true).Init(this);
	}

	/// <summary>
	/// Add a collectible call to the queue, followed by a dequeue attempt;
	/// </summary>
	/// <param name="data"> ItemData to enqueue; </param>
	public void AddCall(ItemData data) {
		callQueue.Enqueue(data);
		TryDequeue();
	}

	/// <summary>
	/// Attempt to process a call from the queue, if applicable;
	/// </summary>
	private void TryDequeue() {
		if (!IsBusy && callQueue.Count > 0) {
            ItemCall call = new(callQueue.Dequeue());
            OnClaimCollectible?.Invoke(this, call);

            //Edge case for doors to not send out notifications
            if (call.inputType != typeof(DoorData)) { 
                OnClaimResult?.Invoke(call);
            }

			if (call.output != null) {
				PlayerController.Instance.DeactivateMovement();
				Transition.DarkenOut();
				IsBusy = true;
				InSequence = true;
			} else Poke();
		}
    }

    public bool CheckClaimedStatus(ItemData itemData) {
		ItemCall call = new(itemData);
		OnInventoryRequest?.Invoke(this, call);
		return call.output.Contains(itemData);
	}

	/// <summary>
	/// Interrupt the controller when a manager finishes their display;
	/// </summary>
	public void Poke() {
		if (callQueue.Count == 0) {
			InSequence = false;
			OnCallsEnd?.Invoke();
			Transition.DarkenIn();
			PlayerController.Instance.ActivateMovement();
		} IsBusy = false;
		TryDequeue();
	}

	/// <summary>
	/// Fetch a reference to the inventory set of a given ItemData type;
	/// </summary>
	/// <typeparam name="T"> Type of items of interest; </typeparam>
	/// <returns> A reference to the current set of items; </returns>
	public HashSet<ItemData> GetItems<T>() where T : ItemData {
		ItemCall call = new(typeof(T));
		OnInventoryRequest?.Invoke(this, call);
		return call.output;
    }
}

/// <summary>
/// Argument for one-sided Controller-Manager communication;
/// </summary>
public class ItemCall : System.EventArgs {
	public ItemData input;
	public System.Type inputType;
	public HashSet<ItemData> output;
	public ItemCall(ItemData input) {
		this.input = input;
		inputType = input.GetType();
	}
	public ItemCall(System.Type inputType) => this.inputType = inputType;
}