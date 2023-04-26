using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    private enum State {
        SetUp,
		Start,
        Write,
        Wait,
        End,
    } private State state = State.Start;

    private string message;
    private Image image;
    private TextMeshProUGUI text;

    [SerializeField] private float duration;
	private float maxScale;
	private float xGrowth;
    private float textAlpha;
    private float timer;

    void Start() {
        timer = duration;
        image = GetComponent<Image>();
        text = transform.parent.GetComponentInChildren<TextMeshProUGUI>(true);
	}

    void OnEnable() {
        state = State.SetUp;
    }

    void Update() {
        switch (state) {
            case State.SetUp:
                text.gameObject.SetActive(true);
                ChangeTextOpacity(0);
				text.text = message;
                state = State.Start;
                break;
			case State.Start:
                if (xGrowth < maxScale) {
                    ChangeXScale(0.2f);
                } else {
                    state = State.Write;
                }
                break;
            case State.Write:
                if (textAlpha < 1) {
                    ChangeTextOpacity(Time.deltaTime * 3f);
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
                if (textAlpha > 0) {
                    ChangeTextOpacity(-Time.deltaTime * 3f);
                } else if (xGrowth > 0) {
                    ChangeXScale(-0.2f);
                } else {
                    timer = duration;
                    text.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                } break;
        }
    }

    private void ChangeXScale(float rate) {
        if (rate > 0) {
            xGrowth = Mathf.Min(maxScale, xGrowth + rate);
		} else {
			xGrowth = Mathf.Max(0, xGrowth + rate);
		} transform.localScale = new Vector3(xGrowth, transform.localScale.y, transform.localScale.z);
	}

    private void ChangeTextOpacity(float rate) {
		if (rate > 0) {
			textAlpha = Mathf.Min(1, textAlpha + rate);
		} else {
		    textAlpha = Mathf.Max(0, textAlpha + rate);
		} text.color = new Color(255, 255, 255, textAlpha);
	}

    public void SetMessage(string message) {
        this.message = message;
    }
    
    public void SetWidth(float maxScale) {
        this.maxScale = maxScale;
    }
}
