using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClass : MonoBehaviour, IPushable
{
    [SerializeField] private bool isHeavy;
    //public event Action<int> OnLitStatusChange;

    private SpriteRenderer sr;
    private new GameObject light;
    private bool defaulLitStatus;
    private enum TriggerType {
        Fire,
        Ice,
        Wind,
    }
    
    [SerializeField] private Sprite onFireSprite;
    [SerializeField] private Sprite onFreezedSprite;
    private bool isLit;
    private bool isFrozen;
    private bool isPushed;

    private float pushDist;
    private float pushSpd;
    private Vector3 pushDir;
    

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        //light = GetComponentInChildren<LightController>().gameObject;
    }

    void FixedUpdate()
    {
        if (isPushed) {
            PushTranslate();
        }
    }

    public void Push(Vector2 dir, float dist, float spd) {
        isPushed = true;
        pushDir = new Vector3(dir.x, dir.y, 0);
        pushDist = dist;
        pushSpd = spd;
    }

    public void PushTranslate() {
        if (pushDist <= 0) {
            isPushed = false;
        } else {
            transform.Translate(pushDir * pushSpd * Time.deltaTime);
            pushDist -= (pushDir *  pushSpd * Time.deltaTime).magnitude;
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

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<IceballBehavior>()) {
            sr.sprite = onFreezedSprite;
            return;
        }
    }


    //     if (collision.gameObject.GetComponent<FireballBehavior>()) {
    //         SpellReaction(TriggerType.Fire);
    //         if (isLit) return;
    //     } else if (collision.gameObject.GetComponent<IceballBehavior>() && isLit) {
    //         SpellReaction(TriggerType.Ice);
    //     } else if (collision.gameObject.GetComponent<WindblastBehavior>()) {
	// 		SpellReaction(TriggerType.Wind, collision.gameObject.GetComponent<Spell>().direction);
    //         return;
    //     } else {
	// 		return;
    //     }
    //     ChangeLit();
    //     var litChange = isLit ? 1 : -1;
    //     foreach (Firewood_Script firewood in firewoodScripts) {
    //         litChange += !firewood.GetLit() ? 1 : -1;
    //     } OnLitStatusChange?.Invoke(litChange);
    // }

    // public void ChangeLit() {
    //     isLit = !isLit;
	// }

    // public void SetDefaultLit() {
    //     if (isLit == defaulLitStatus) {
    //         return;
    //     } else {
    //         OnLitStatusChange?.Invoke(!isLit ? 1 : -1);
    //         ChangeLit();
    //     }
    // }
}
