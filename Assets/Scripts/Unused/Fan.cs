using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : BaseObject {

    public event System.Action OnBlowingChange;

    [Header("Fan")]

    [SerializeField] private float windIncrement = 30f;
    [SerializeField] private Animator animator;

    private const string SPIN_CONDITION = "IsSpinning";

    public bool IsRotating => gameObject.activeSelf ? animator.GetBool(SPIN_CONDITION) : false;

    protected override void Awake() {
        base.Awake();
        OnHeatToggle += BaseObject_OnHeatToggle;
        OnBlow += BaseObject_OnBlow;
    }

    private void BaseObject_OnHeatToggle(ObjectState prevState, bool active) {
        if (!active) {
            StopAllCoroutines();
            animator.SetBool(SPIN_CONDITION, false);
            OnBlowingChange?.Invoke();
        }
    }

    private void BaseObject_OnBlow(Vector2 dir, float strength) {
        if (State == ObjectState.Frozen) return;
        animator.SetBool(SPIN_CONDITION, true);
        OnBlowingChange?.Invoke();
        StopAllCoroutines();
        StartCoroutine(AwaitStopAsync(windIncrement));
    }

    private IEnumerator AwaitStopAsync(float spinTime) {
        yield return new WaitForSeconds(spinTime);
        animator.SetBool(SPIN_CONDITION, false);
        OnBlowingChange?.Invoke();
    }
}
