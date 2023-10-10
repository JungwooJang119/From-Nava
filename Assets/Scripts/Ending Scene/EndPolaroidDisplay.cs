using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPolaroidDisplay : MonoBehaviour {

    [SerializeField] private tranMode transition;
    [SerializeField] private float timeDuration;
    public float time;
    private bool hasNotPlayed;

    private Image image;
    private float alphaTarget = 1;

    private void Start() {
        image = GetComponentInChildren<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        hasNotPlayed = true;
    }

    void OnEnable() => transition.DarkenOut();

    void Update() {
        image.color = Vector4.MoveTowards(image.color, new Color(image.color.r, image.color.g, image.color.b, alphaTarget), Time.deltaTime / 1.5f);
        if (time >= timeDuration) {
            Fade();
        } else {
            time += Time.deltaTime;
        }
        if (hasNotPlayed) {
            AudioControl.Instance.PlayMusic("EndingPart2", false);
            hasNotPlayed = false;
        }

        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     Fade();
        // }
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
