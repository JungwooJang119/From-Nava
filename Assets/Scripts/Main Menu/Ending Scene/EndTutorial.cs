using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTutorial : MonoBehaviour {

    [SerializeField] private float amplitude;
    [SerializeField] private float endTime;

    private TMPro.TextMeshProUGUI text;
    private Image image;
    private float ogPosition;
    private float alphaTarget = 1;

    private void Awake() {
        image = GetComponentInChildren<Image>();
        ogPosition = image.rectTransform.anchoredPosition.y;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.alpha = 0;
    }

    void Update() {
        var rt = image.rectTransform;
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x,
                                          ogPosition + Mathf.Sin(Time.time * 1.5f) * amplitude);
        image.color = Vector4.MoveTowards(image.color, new Color(image.color.r, image.color.g, image.color.b, alphaTarget), Time.deltaTime * 1.5f);
        text.alpha = Mathf.MoveTowards(text.alpha, alphaTarget, Time.deltaTime * 1.5f);
        if (image.color.a == 0) Destroy(gameObject);
    }

    public void Fade() {
        alphaTarget = 0;
    }
}
