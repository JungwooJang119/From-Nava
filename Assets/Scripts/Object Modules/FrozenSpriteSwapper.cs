using UnityEngine;

public class FrozenSpriteSwapper : SpriteSwapper {

    [SerializeField] private Sprite frozenSprite;
    void Awake() {
        Default = spriteRenderer.sprite;
        Frozen = frozenSprite;
    }
} 