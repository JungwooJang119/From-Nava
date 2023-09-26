using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClass : MonoBehaviour
{
    public bool isHeavy;
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
    

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        //light = GetComponentInChildren<LightController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
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
