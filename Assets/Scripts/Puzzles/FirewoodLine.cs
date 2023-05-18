using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewoodLine : MonoBehaviour
{
	private new LineRenderer renderer;
	private float key1 = 0.0f;
	private float key2 = 0.05f;
	private float keyRate = 3f;
	private Color color1;
	private Color color2;
	private Color edgeColor;
	private float alpha = 0f;
	private Firewood_Script targetFirewood;

	private bool start = true;

    // Like Start() but... no >:(
    public void SetUpLine(Transform parent, Transform target, Material lineMaterial) {
		renderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
		transform.SetParent(parent);
		transform.position = parent.position;
		renderer.textureMode = LineTextureMode.Tile;
		renderer.material = lineMaterial;
		renderer.widthMultiplier = 0.5f;
		renderer.positionCount = 4;
		renderer.SetPosition(0, parent.position);
		SetPositionKey(1, key1, parent.position, target.position);
		SetPositionKey(2, key2, parent.position, target.position);
		renderer.SetPosition(3, target.position);
		renderer.sortingOrder = -1;
		targetFirewood = target.GetComponent<Firewood_Script>();
		gameObject.SetActive(false);
	}

    public void DrawLine(Color color1, Color color2, bool whiteEnd) {
		start = true;
		this.color1 = color1;
		this.color2 = color2;
		if (whiteEnd) edgeColor = Color.white;
		else edgeColor = Color.black;
		SetGradient();
	}

    void Update() {
		var rate = keyRate * Time.deltaTime;
		if (start) {
			ComputeKeys(rate, rate, rate*2f);
			SetGradient();
			if (key2 >= 1f) {
				targetFirewood.ChangeLit();
				start = false;
			}
		} else {
			ComputeKeys(rate/2f, rate/2f, -rate);
			SetGradient();
			if (alpha <= 0) Reset();
		}
    }

	private void ComputeKeys(float gradientRate1, float gradientRate2, float alphaRate) {
		key1 = Mathf.Min(key1 + gradientRate1, 1f);
		key2 = Mathf.Min(key2 + gradientRate2, 1f);
		SetPositionKey(1, key1, renderer.GetPosition(0), renderer.GetPosition(3));
		SetPositionKey(2, key2, renderer.GetPosition(0), renderer.GetPosition(3));
		if (alphaRate > 0) {
			alpha = Mathf.Min(alpha + alphaRate, 1f);
		} else {
			alpha = Mathf.Max(0f, alpha + alphaRate);
		}
	}

	private void SetPositionKey(int index, float keyPercent, Vector3 start, Vector3 end) {
		renderer.SetPosition(index, Vector3.Lerp(start, end, keyPercent));
	}

	private void SetGradient() {
		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(edgeColor, 0f), new GradientColorKey(color1, key1),
									 new GradientColorKey(color2, key2), new GradientColorKey(edgeColor, 1f) },
			new GradientAlphaKey[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(alpha, key1),
									 new GradientAlphaKey(alpha, key2), new GradientAlphaKey(0f, 1f) }
		);
		renderer.colorGradient = gradient;
	}

	private void Reset() {
		key1 = 0.0f;
		key2 = 0.05f;
		alpha = 0f;
		gameObject.SetActive(false);
	}
}
