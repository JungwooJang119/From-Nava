using System.Linq;
using UnityEngine;

public class FireballBehavior : BaseSpellBehavior {

    [Header("Fireball Behavior")]

    [SerializeField] private LightController lightController;
    [SerializeField] private float scaleOscillation;

    private Vector2 baseScale;

    protected override void Awake() {
        base.Awake();
        baseScale = transform.localScale;
        transform.localScale = Vector2.zero;
    }

    void Start() {
        lightController.SetLightBounds(0, 0);
        ShiftState(State.Start);
    }

    void Update() {
        switch (state) {
            case State.Start:
                /// Stay on cast point until release;
                transform.position = castPoint.position;
                /// Deactivate spell on start;
                if (spellScript != null && spellScript.enabled) {
                    ToggleComponents();
                } /// Scale up the sprite;
                transform.localScale = Vector2.MoveTowards(transform.localScale, baseScale, Time.deltaTime * 20f);
                if (Mathf.Approximately(Vector2.Distance(transform.localScale, baseScale), 0)) {
                    trailParticles.Play();
                    if (spellScript != null) ToggleComponents();
                    LifetimeCountownAsync(spellData.lifetime);
                    ShiftState(State.Lifetime);
                } break;
            case State.Lifetime:
                baseScale = Mathf.Sin(Time.time * 10f) * Vector2.one * scaleOscillation + baseScale;
                transform.localScale = baseScale;
                break;
            case State.End:
                /// Scale down the sprite;
                transform.localScale = Vector2.MoveTowards(transform.localScale, Vector2.zero, Time.deltaTime * 20f);
                if (Mathf.Approximately(Vector2.Distance(transform.localScale, Vector2.zero), 0)) {
                    if (spellScript != null) {
                        CleanUp();
                        GenerateBurst(1f, 100f);
                    } else {
                        GenerateBurst(0.75f, 150f);
                    } state = State.Done;
                } break;
        }
    }

    protected override void Spell_OnSpellDestroy(GameObject o) {
        AudioControl.Instance.PlaySFX("Fireball Collision", gameObject, 0.2f);
        base.Spell_OnSpellDestroy(o);
    }

    protected override void ShiftState(State state) {
        base.ShiftState(state);
        if (lightController == null) return;
        lightController.MoveLightBounds(1f, state == State.End ? 0 : 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (spellScript.CasterColliders != null
            && spellScript.CasterColliders.Contains(collision)) return;
        if (collision.TryGetComponent(out BaseObject baseObject)) {
            baseObject.Ignite(this);
        }
    }
}