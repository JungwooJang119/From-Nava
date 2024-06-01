using UnityEngine;

[RequireComponent(typeof(FlammableModule))]
public class Firewood : BaseObject {

    public event System.Action OnLitStatusChange;

    [UnityEngine.Serialization.FormerlySerializedAs("adjFirewoods")]
    [SerializeField] private GameObject[] linkedFirewoods;
    [SerializeField] private Material lineMaterial;
    public bool IsPuzzleLit { get; set; }

    private Firewood[] firewoodScripts;
    private FirewoodLine[] firewoodLines;

    protected override void Awake() {
        base.Awake();
		firewoodScripts = new Firewood[linkedFirewoods.Length];
        firewoodLines = new FirewoodLine[linkedFirewoods.Length];
		for (int i = 0; i < linkedFirewoods.Length; i++) {
			firewoodScripts[i] = linkedFirewoods[i].GetComponent<Firewood>();
            firewoodLines[i] = (new GameObject("Firewood Line " + i)).AddComponent<FirewoodLine>();
			firewoodLines[i].Init(transform, firewoodScripts[i].transform, lineMaterial);
		} OnHeatToggle += BaseObject_OnHeatToggle;
    }
 
    private void BaseObject_OnHeatToggle(ObjectState prevState, bool active) {
        if (prevState == ObjectState.Frozen) {
            if (active) Ignite();
            return;
        } if (State == ObjectState.Frozen) return;
        SetState(IsPuzzleLit && !active ? ObjectState.Default 
               : !IsPuzzleLit && active ? ObjectState.Burning : State);
        bool wasDefault = prevState == ObjectState.Default;
        foreach (FirewoodLine lineScript in firewoodLines) {
            bool interaction = false;
            if (active && wasDefault) {
                interaction = true;
                var color1 = new Color32(223, 113, 38, 255);
                var color2 = new Color32(222, 54, 54, 255);
                lineScript.DrawLine(color1, color2, false);
            } else if (!active && !wasDefault) {
                interaction = true;
                var color1 = new Color32(8, 100, 153, 255);
                var color2 = new Color32(128, 249, 255, 255);
                lineScript.DrawLine(color1, color2, false);
            } if (interaction) {
                foreach (Firewood firewood in firewoodScripts) {
                    firewood.IsPuzzleLit = !(firewood.State == ObjectState.Burning);
                }
            }
        } OnLitStatusChange?.Invoke();
    }

    public override void SetState(ObjectState state) {
        base.SetState(state);
        IsPuzzleLit = State == ObjectState.Burning;
    }

    public override void ObjectReset() {
        OnHeatToggle -= BaseObject_OnHeatToggle;
        ObjectState prevState = State;
        base.ObjectReset();
        if (prevState != defaultState && (prevState == ObjectState.Burning
                                          || prevState == ObjectState.Burning)) {
            OnLitStatusChange?.Invoke();
        } OnHeatToggle += BaseObject_OnHeatToggle;
    }

    public void Toggle() {
        OnHeatToggle -= BaseObject_OnHeatToggle;
        if (State == ObjectState.Burning) Freeze(); else ForceState(ObjectState.Burning);
        OnHeatToggle += BaseObject_OnHeatToggle;
    }

    #if UNITY_EDITOR
    public void EDITOR_AssignFirewoods(GameObject[] linkedFirewoods) {
        this.linkedFirewoods = linkedFirewoods;
    }
    private void OnDrawGizmosSelected() {
        foreach (GameObject firewood in linkedFirewoods) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, firewood.transform.position);
        }
    }
    #endif
}