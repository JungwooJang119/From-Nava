using UnityEngine;

public class SpriteSwapper : MonoBehaviour {

    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Sprite Default { get; protected set; }
    public Sprite Frozen { get; protected set; }
}