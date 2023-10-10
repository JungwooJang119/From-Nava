using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPolaroidDisplay : MonoBehaviour {

    [SerializeField] private tranMode transition;

    private Image image;
    private float alphaTarget = 1;

    private void Start() {
        image = GetComponentInChildren<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    void OnEnable() => transition.DarkenOut();

    void Update() {
        image.color = Vector4.MoveTowards(image.color, new Color(image.color.r, image.color.g, image.color.b, alphaTarget), Time.deltaTime / 1.5f);
        if (Input.GetKeyDown(KeyCode.Space)) {
            Fade();
        }
        //Fade();
    }

    public void Fade() {
        StartCoroutine(FinalFade());
        alphaTarget = 0;
    }

    IEnumerator FinalFade() {
        while (image.color.a != 0) yield return null;
        Destroy(gameObject);
    }
}
