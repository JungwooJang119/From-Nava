using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationObject : MonoBehaviour {

    public System.Action OnNotificationFinished;

    private enum State {
        SetUp,
        Start,
        Write,
        Wait,
        End
    } private State state = State.Start;

    private TextMeshProUGUI text;

    private float duration = 1f;
    private float imageWidth;
	private float maxScale;
    private float timer;

    void Awake() {
        imageWidth = GetComponent<Image>().rectTransform.sizeDelta.x;
    }

    public void Initialize(TextMeshProUGUI notificationText, float scaleConstraint) {
        if (text == null) text = notificationText;
        timer = duration;
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        text.color = new Color(1, 1, 1, 0);
        text.ForceMeshUpdate(true);
        maxScale = (text.preferredWidth + 16) / imageWidth;
        if (maxScale > scaleConstraint) {
            text.fontSize *= scaleConstraint / maxScale;
            maxScale = scaleConstraint;
        } state = State.Start;
    }

    void Update() {
        switch (state) {
			case State.Start:
                if (transform.localScale.x < maxScale) {
                    transform.localScale = Vector3.MoveTowards(transform.localScale,
                                                           new Vector3(maxScale, transform.localScale.y, transform.localScale.z), 0.05f);
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
                } else {
                    transform.localScale = Vector3.MoveTowards(transform.localScale,
                                                           new Vector3(0, transform.localScale.y, transform.localScale.z), 0.2f);
                }
                if (transform.localScale.x <= 0) {
                    OnNotificationFinished?.Invoke();
                    gameObject.SetActive(false);
                } break;
        }
    }

    public void EndNotification() {
        timer = 0;
    }
}
