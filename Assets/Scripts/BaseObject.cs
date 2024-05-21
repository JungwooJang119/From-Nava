using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ObjectState { Default, Burning, Frozen }

public class BaseObject : MonoBehaviour {

    [System.Serializable] public class ObjectAttributes {
        public bool isHeavy;
        public bool isPushable;
        public bool isFlammable;
        public bool isFreezable;
        public bool isCopyable;
    } [SerializeField] private ObjectAttributes attributes;
    public ObjectAttributes Attributes => attributes;
    [SerializeField] private ObjectState defaultState;
    public ObjectState State { get; private set; }

    public event System.Action<Vector2, float> OnBlow;
    public event System.Action<bool> OnHeatToggle;
    public event System.Action OnCopy;
    public event System.Action<ObjectState> OnObjectReset;

    private Vector2 origin;

    void Awake() => origin = transform.position;

    public void Ignite() => OnHeatToggle?.Invoke(true);
    public void Freeze() => OnHeatToggle?.Invoke(false);
    public void Blow(Vector2 dir, float strength) => OnBlow?.Invoke(dir, strength);
    public void SignalCopy() => OnCopy?.Invoke();

    public virtual void ObjectReset() {
        transform.position = origin;
        State = defaultState;
        OnObjectReset?.Invoke(State);
    }

    private SpriteRenderer sr;
    private enum TriggerType {
        Fire,
        Ice,
        Wind,
        None,
    }
    
    [SerializeField] private GameObject onFire;
    [SerializeField] private Sprite onFreezedSprite;
    public Sprite defaultSprite;
    private bool isFrozen;
    private bool isPushed;

    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;
    private string elementType;
    private bool hasSwitched;

    public ParticleSystem dust;
    private FirewoodFire firewoodFire;
    private static GameObject auditor;

    /*
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origin = transform.position;
        // light = GetComponentInChildren<LightController>().gameObject;
    }*/

    private void Start() {
        if (auditor == null) {
            auditor = GameObject.Find("Auditor");
        }
        if (onFire != null) {
            onFire.SetActive(false);
            firewoodFire = onFire.GetComponent<FirewoodFire>();
        }
        defaultSprite = sr.sprite;
        elementType = "none";
        hasSwitched = false;
    }

    void FixedUpdate()
    {
        if (isPushed /*&& !isHeavy*/) {
            PushTranslate();
        }
    }

    public void Push(Vector2 dir, float dist, float spd) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
        //if (!isHeavy) {
            dust.Play();
        //}
    }

    public void PushTranslate() {
        if (pushDist <= 0) {
            isPushed = false;
        } else {
            transform.Translate(pushDir * pushSpd * Time.deltaTime, relativeTo:Space.World);
			pushDist -= (pushDir * pushSpd * Time.deltaTime).magnitude;
        }
    }

    public bool GetPushed() {
        return isPushed;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("WindBlast")) {
            if (isPushed) {
                pushDist = 0;
                isPushed = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PType a = collision.gameObject.GetComponent<PType>();
        if (a != null && !hasSwitched) {
            if (a.getType() == "ICE") {
                if (elementType == "none") {
                    sr.sprite = onFreezedSprite;
                    elementType = "ice";
                    hasSwitched = true;
                } else if (elementType == "fire") {
                    onFire.SetActive(false);
                    firewoodFire.Toggle(true);
                    elementType = "none";
                    hasSwitched = true;
                    if (transform.parent.gameObject.GetComponent<AuditTarget>() != null) {
                        auditor.GetComponent<Auditor>().updateLightUp(-1);
                    }
                }
            } else if (a.getType() == "FIRE") {
                if (elementType == "none") {
                    onFire.SetActive(true);
                    firewoodFire.Toggle(true);
                    elementType = "fire";
                    hasSwitched = true;
                    if (transform.parent.gameObject.GetComponent<AuditTarget>() != null) {
                        auditor.GetComponent<Auditor>().updateLightUp(1);
                    }
                } else if (elementType == "ice") {
                    sr.sprite = defaultSprite;
                    elementType = "none";
                    hasSwitched = true;
                }
            }
        }
        StartCoroutine(ChangeLag());
    }

    IEnumerator ChangeLag() {
        yield return new WaitForSeconds(0.1f);
        hasSwitched = false;
    }


}

public abstract class ObjectModule : MonoBehaviour {
    protected BaseObject baseObject;
    protected virtual void Awake() => baseObject = GetComponent<BaseObject>();
}

public class FlammableModule : ObjectModule {
    [SerializeField] private FirewoodFire[] flames;
    void Start() => baseObject.OnHeatToggle += BaseObject_OnFireToggle;

    private void BaseObject_OnFireToggle(bool heatSignal) {
        switch (baseObject.State) {
            case ObjectState.Default:
                if (heatSignal) {
                    foreach (FirewoodFire fire in flames) fire.Toggle(heatSignal);
                } break;
            case ObjectState.Burning:
                foreach (FirewoodFire fire in flames) fire.Toggle(heatSignal);
                break;
        }
    }
}

[RequireComponent(typeof(SpriteRenderer))]
public class FreezableModule : ObjectModule {
    [SerializeField] private SpriteRenderer sr;
    void Start() => baseObject.OnHeatToggle += BaseObject_OnFireToggle;

    private void BaseObject_OnFireToggle(bool heatSignal) {
        switch (baseObject.State) {
            case ObjectState.Default:
                if (!heatSignal) {

                }
                break;
            case ObjectState.Frozen:
                if (heatSignal) {

                }
                break;
        }
    }

    private IEnumerator FreezeAsync() {
        while (baseObject.State == ObjectState.Frozen) {

            yield return null;
        }
    }
}

public class PushableModule : ObjectModule {
    [SerializeField] private ParticleSystem dust;
    void Start() => baseObject.OnBlow += BaseObject_OnBlow;

    private void BaseObject_OnBlow(Vector2 dir, float strength) {
        throw new System.NotImplementedException();
    }
}

public class CopyableModule : ObjectModule {

    void Start() => baseObject.OnCopy += BaseObject_OnCopy;

    private void BaseObject_OnCopy() {
        throw new System.NotImplementedException();
    }
}

[CustomEditor(typeof(BaseObject))]
public class BaseObjectEditor : Editor {

    public override void OnInspectorGUI() {
        
    }
}