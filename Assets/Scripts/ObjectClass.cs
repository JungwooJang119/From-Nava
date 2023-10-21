using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClass : MonoBehaviour, IPushable
{
    [SerializeField] private bool isHeavy;
    //public event Action<int> OnLitStatusChange;

    private SpriteRenderer sr;
    private new GameObject light;
    private bool defaultLitStatus;
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

    private Vector3 origin;
    public ParticleSystem dust;
    private FirewoodFire firewoodFire;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origin = transform.position;
        // light = GetComponentInChildren<LightController>().gameObject;
    }

    private void Start() {
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
        if (isPushed && !isHeavy) {
            PushTranslate();
        }
    }

    public void Push(Vector2 dir, float dist, float spd) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
        if (!isHeavy) {
            dust.Play();
        }
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

    public void Reset() {
		transform.position = origin;
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
                }
            } else if (a.getType() == "FIRE") {
                if (elementType == "none") {
                    onFire.SetActive(true);
                    firewoodFire.Toggle(true);
                    elementType = "fire";
                    hasSwitched = true;
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
