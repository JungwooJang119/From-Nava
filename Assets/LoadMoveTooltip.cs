using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadMoveTooltip : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private SpriteRenderer moveTooltipBG;
    [SerializeField] private TextMeshProUGUI moveTooltipText;
    private bool isLoaded;

    // Start is called before the first frame update
    void Start()
    {
        isLoaded = false;
        StartCoroutine(BeginLoading(255f, 215f));
    }

    void Update() {
        if (playerController.hasDoneMoveTooltip && isLoaded) {
            isLoaded = false;
            StopAllCoroutines();
            StartCoroutine(DestroyMoveTooltip(0));
        }
    }

    IEnumerator BeginLoading(float textOpacity, float bgOpacity) {
        yield return new WaitForSeconds(2.5f);
        isLoaded = true;
        Color textCurrColor = moveTooltipText.color;
        Color spriteCurrColor = moveTooltipBG.color;
        while (textCurrColor.a != textOpacity) {
            textCurrColor.a = Mathf.MoveTowards(textCurrColor.a, textOpacity, 0.01f);
            spriteCurrColor.a = Mathf.MoveTowards(spriteCurrColor.a, bgOpacity, 0.01f);
            moveTooltipText.color = textCurrColor;
            moveTooltipBG.color = spriteCurrColor;
            yield return null;
        }
    }

    IEnumerator DestroyMoveTooltip(float opacity) {
        Color textCurrColor = moveTooltipText.color;
        Color spriteCurrColor = moveTooltipBG.color;
        print(spriteCurrColor.a);
        while (textCurrColor.a != opacity) {
            textCurrColor.a = Mathf.MoveTowards(textCurrColor.a, opacity, 0.02f);
            spriteCurrColor.a = Mathf.MoveTowards(spriteCurrColor.a, opacity, 0.02f);
            moveTooltipText.color = textCurrColor;
            moveTooltipBG.color = spriteCurrColor;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
