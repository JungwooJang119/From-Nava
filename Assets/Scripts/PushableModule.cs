using UnityEngine;

public class PushableModule : ObjectModule {

    [SerializeField] private ParticleSystem dust;
    [SerializeField] private float pushMultiplier = 1f;

    private float pushDist;
    private float pushSpeed;
    private Vector2 pushDir;
    private bool isPushed;

    void Start() => baseObject.OnBlow += BaseObject_OnBlow;

    private void BaseObject_OnBlow(Vector2 dir, float strength) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = strength * pushMultiplier;
        pushSpeed = strength * pushMultiplier;
        if (!baseObject.Attributes.isHeavy) dust.Play();
    }

    void FixedUpdate() {
        if (isPushed) {
            if (pushDist <= 0) {
                isPushed = false;
            } else {
                transform.Translate(pushDir * pushSpeed * Time.deltaTime, relativeTo: Space.World);
                pushDist -= (pushDir * pushSpeed * Time.deltaTime).magnitude;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("WindBlast")) {
            pushDist = 0;
            isPushed = false;
        }
    }
}
