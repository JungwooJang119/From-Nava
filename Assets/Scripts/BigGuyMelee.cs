using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BigGuyDirection {
    RIGHT,
    LEFT,
    UP,
    DOWN
}

public class BigGuyMelee : MonoBehaviour
{
    public PlayerController player;
    private Vector3 dir;

    public GameObject windBlast;

    public Spell spell;

    public bool isWindBlasting = false;
    public float currTime;

    private Vector2 fireRight = Vector2.right;
    private Vector2 fireLeft = Vector2.left;
    private Vector2 fireUp = Vector2.up;
    private Vector2 fireDown = Vector2.down;

    [SerializeField] private BigGuyDirection facingDir;

    private void Update() {
        currTime += Time.deltaTime;
        spell.CastSpell(fireDown);
        if (isWindBlasting) {
            Fire();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            player.TakeDamage(2);
        }
        dir = player.gameObject.GetComponent<Transform>().position - transform.position;
        if (player.playerHealth > 0) {
            player.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(2000*dir);
        }
    }

    void Fire() {
        if (currTime >= 0) {
            //Instantiate(windBlast, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
            switch(facingDir) {
                case BigGuyDirection.RIGHT:
                    spell.CastSpell(fireDown);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireRight);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireUp);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    break;
                case BigGuyDirection.LEFT:
                    spell.CastSpell(fireDown);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireLeft);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireUp);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    break;
                case BigGuyDirection.UP:
                    spell.CastSpell(fireUp);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireLeft);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireRight);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    break;
                case BigGuyDirection.DOWN:
                    spell.CastSpell(fireDown);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireLeft);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    spell.CastSpell(fireRight);
                    AudioControl.Instance.PlaySFX(spell.spell.sfxString, PlayerController.Instance.gameObject, 0.1f, 0.5f);
                    Instantiate(spell, this.gameObject.GetComponent<Transform>().position, Quaternion.identity);
                    break;
            }
            currTime = -0.1f;
        }
    }
}
