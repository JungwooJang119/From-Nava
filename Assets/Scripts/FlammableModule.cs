using UnityEngine;

public class FlammableModule : ObjectModule {

    [SerializeField] private FireFlame[] flames;

    protected override void Awake() {
        base.Awake();
        foreach (FireFlame flame in flames) flame.AttachTo(baseObject);
        baseObject.OnHeatToggle += OnHeatToggle;
    }

    private void OnHeatToggle(ObjectState prevState, bool heatSignal) {
        switch (prevState) {
            case ObjectState.Default:
                if (heatSignal) {
                    foreach (FireFlame fire in flames) fire.Toggle(fire.State, true);
                } break;
            case ObjectState.Burning:
                foreach (FireFlame fire in flames) fire.Toggle(fire.State, heatSignal);
                break;
        }
    }
}
