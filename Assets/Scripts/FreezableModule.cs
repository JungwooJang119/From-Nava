using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FreezableModule : ObjectModule {

    public event System.Action OnFreezeEnd;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float freezeSpeed = 2.5f;
    [SerializeField] private float shineSpeed = 3.5f;
    [SerializeField] private Vector2 shineWaitRange = new Vector2(6, 8);
    [SerializeField] private Material freezeMaterial;

    private float patternSeed;

    protected override void Awake() {
        base.Awake();
        patternSeed = Random.Range(1.5f, 10f);
        baseObject.OnHeatToggle += BaseObject_OnFireToggle;
    }

    private void BaseObject_OnFireToggle(ObjectState prevState, bool heatSignal) {
        switch (prevState) {
            case ObjectState.Default:
                if (!heatSignal) {
                    baseObject.SetState(ObjectState.Frozen);
                    StopAllCoroutines();
                    StartCoroutine(FreezeAsync(1));
                } break;
            case ObjectState.Frozen:
                if (heatSignal) {
                    baseObject.SetState(ObjectState.Default);
                    StopAllCoroutines();
                    StartCoroutine(FreezeAsync(0));
                } break;
        }
    }

    private IEnumerator FreezeAsync(float targetFreeze) {
        if (spriteRenderer.sharedMaterial != freezeMaterial) {
            spriteRenderer.sharedMaterial = freezeMaterial;
        } MaterialPropertyBlock mpb = new();
        spriteRenderer.GetPropertyBlock(mpb);
        float frost = mpb.GetFloat("_Frost");
        mpb.SetFloat("_Frost_Pattern_Seed", patternSeed);
        mpb.SetVector("_Frost_Pattern_Spread_Seed", new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
        while (!Mathf.Approximately(frost, targetFreeze)) {
            frost = Mathf.MoveTowards(frost, targetFreeze, Time.deltaTime * freezeSpeed);
            mpb.SetFloat("_Frost", frost);
            spriteRenderer.SetPropertyBlock(mpb);
            yield return null;
        } if (targetFreeze == 0) {
            patternSeed = Random.Range(1.5f, 10f);
            OnFreezeEnd?.Invoke();
        } while (targetFreeze > 0) {
            float shinePass = -1;
            yield return new WaitForSeconds(Random.Range(shineWaitRange.x, shineWaitRange.y));
            while (!Mathf.Approximately(shinePass, 1)) {
                shinePass = Mathf.MoveTowards(shinePass, 1, Time.deltaTime * shineSpeed);
                mpb.SetFloat("_Shine_Pass", shinePass);
                spriteRenderer.SetPropertyBlock(mpb);
                yield return null;
            } 
        }
    }
}
