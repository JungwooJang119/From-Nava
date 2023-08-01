using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationObject : MonoBehaviour {

    private enum State {
        SetUp,
        Start,
        Write,
        Wait,
        End
    } private State state = State.Start;

    private TextMeshProUGUI text;

    private float duration = 2f;
    private float imageWidth;
	private float maxScale;
    private float timer;

    void Awake() {
        imageWidth = GetComponent<Image>().rectTransform.sizeDelta.x;
    }

    private void OnEnable() {
        if (text == null) text = NotificationManager.NotificationText;
        timer = duration;
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        text.color = new Color(1, 1, 1, 0);
        text.ForceMeshUpdate(true);
        maxScale = (text.preferredWidth + 16) / imageWidth;
        state = State.Start;
    }

    void Update() {
        switch (state) {
			case State.Start:
                if (transform.localScale.x < maxScale) {
                    transform.localScale = Vector3.MoveTowards(transform.localScale,
                                                           new Vector3(maxScale, transform.localScale.y, transform.localScale.z), 0.2f);
                } else {
                    state = State.Write;
                } break;
            case State.Write:
                if (text.color.a < 1) {
                    text.color = Vector4.MoveTowards(text.color, new Color(1, 1, 1, 1), Time.deltaTime * 3f);
                } else {
                    state = State.Wait;
                } break;
            case State.Wait:
                if (timer > 0) {
                    timer -= Time.deltaTime;
                } else {
                    state = State.End;
                } break;
            case State.End:
                if (text.color.a > 0) {
                    text.color = Vector4.MoveTowards(text.color, new Color(1, 1, 1, 0), Time.deltaTime * 3f);
                } else if (transform.localScale.x > 0) {
                    transform.localScale = Vector3.MoveTowards(transform.localScale,
                                                           new Vector3(0, transform.localScale.y, transform.localScale.z), 0.2f);
                } else {
                    gameObject.SetActive(false);
                } break;
        }
    }
}
