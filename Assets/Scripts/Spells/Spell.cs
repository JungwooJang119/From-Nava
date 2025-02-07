using System.Linq;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellSO spell;
    public event System.Action<GameObject> OnSpellDestroy;

    //[SerializeField] private LogicScript logicScript;
    [SerializeField] private Collider2D bruhCollider;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Vector3 rotate;
    [SerializeField] private bool spellActive = false;
    [SerializeField] private bool isChair = false;
    [SerializeField] private bool isWind = false;
    [SerializeField] private bool isPiplup = false;

    public Vector2 direction;
    public Collider2D[] CasterColliders { get; private set; }

    public int damage;


    private void Awake()
    {

        if (bruhCollider == null) bruhCollider = GetComponent<Collider2D>();
        bruhCollider.isTrigger = true;

        rb = GetComponent<Rigidbody2D>();
        if (!isChair) {
            rb.isKinematic = true;
        }
        

        damage = spell.damageAmt;

        // Spells no longer get destroyed automatically.
        // They must carry out their own destroy sequence
        // on lifetime and collision. 1/3 Spells Done;
    }

    protected virtual void Update()
    {
        if(spellActive)
            MoveSpell();
    }

    private void MoveSpell()
    {
        transform.Translate(Vector3.up * spell.speed * Time.deltaTime);
    }

    public virtual void CastSpell(MonoBehaviour caster, Vector2 dir) {
        direction = dir;
        if(dir.x < 0)
            rotate = new Vector3(0, 0, 90);
        if (dir.x > 0)
            rotate = new Vector3(0, 0, -90);
        if (dir.y > 0)
            rotate = new Vector3(0, 0, 0);
        if (dir.y < 0)
            rotate = new Vector3(0, 0, 180);
        if (!isChair || !isPiplup) transform.Rotate(rotate);
        CasterColliders = caster.GetComponentsInChildren<Collider2D>(true);
        spellActive = true;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (CasterColliders != null 
            && CasterColliders.Contains(other)) return;
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null) {
            // print("spellscript");
        }
        if (other.gameObject.CompareTag("Player")) {
            return;
        }
        
        if (other.gameObject.CompareTag("IceTower")) {
            return;
        }
        if (other.gameObject.CompareTag("Pit")) {
            return;
        }

        if (other.gameObject.CompareTag("CrackedWall")) {
            return;
        }

        if (isChair || isPiplup) {
            return;
        }

        if (isWind) {
            if (other.gameObject.CompareTag("WindBlast")) {
                return;
            }
            if (other.gameObject.CompareTag("Walls")) {
                OnSpellDestroy?.Invoke(other.gameObject);
            } return;
        }

        //Apply hit particle effects, sfx, spell effects\
        if(other.gameObject.CompareTag("Enemy")) {
            Enemy enemyHealth = other.GetComponent<Enemy>();
            enemyHealth.Damage(spell.damageAmt);
            Destroy(this.gameObject);
        }
		OnSpellDestroy?.Invoke(other.gameObject);
    }
}