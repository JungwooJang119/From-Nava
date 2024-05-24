using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseSpellBehavior : MonoBehaviour {

    [Header("Base Behavior")]

    [SerializeField] protected SpellSO spellData;
    [SerializeField] protected Spell spellScript;

    [SerializeField] protected ParticleSystem trailParticles;
    [SerializeField] protected int burstAmount;

    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Collider2D coll;

    protected enum State {
        Start,
        Lifetime,
        End,
        Done,
    } protected State state;

    /// <summary> Transform the spell is glued to before release; </summary>
    protected Transform castPoint;

    protected virtual void Awake() {
        spellScript.OnSpellDestroy += Spell_OnSpellDestroy;
        castPoint = PlayerController.Instance.castPoint;
    }

    protected void ToggleComponents() {
        spellScript.enabled = !spellScript.enabled;
        rb.simulated = !rb.simulated;
        coll.enabled = !coll.enabled;
    }

    protected void CleanUp() {
        Destroy(spellScript);
        Destroy(rb);
        Destroy(coll);
    }

    protected virtual void Spell_OnSpellDestroy(GameObject o) {
        CleanUp();
        ShiftState(State.End);
    }

    protected virtual void ShiftState(State state) => this.state = state;

    protected void GenerateBurst(float particleLifetime, float particleSpeed) {
        var parMainSystem = trailParticles.main;
        parMainSystem.startLifetime = particleLifetime;
        parMainSystem.startSpeed = particleSpeed;
        trailParticles.emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, burstAmount) });
        trailParticles.Stop();
        trailParticles.Play();
        StopTrailAsync();
        Destroy(gameObject, trailParticles.main.startLifetime.constant);
    }

    protected async void LifetimeCountownAsync(float lifetime) {
        await Task.Delay((int) (lifetime * 1000));
        ShiftState(State.End);
    }

    private async void StopTrailAsync() {
        await Task.Delay(10);
        if (trailParticles) trailParticles.Stop();
    }
}