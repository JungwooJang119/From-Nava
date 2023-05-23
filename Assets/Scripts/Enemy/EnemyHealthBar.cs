using System.Collections;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

    private int maxHP;
    private int hp;

    [Tooltip("Speed of the white line progress and the gradient swipe;")] [SerializeField]
    private float swipeSpeed = 0.025f;

    [Tooltip("Transparency of the health bar;")] [SerializeField]
    private float opacity = 0.5f;

    [Tooltip("Length of the bar, in pixels\nMinimum value: 24 px;")] [Min(24)] [SerializeField]
    private int barLength = 48;

    [Tooltip("Use this value to control the scale\nPlease, don't use the transform component ;-;")] [SerializeField]
    private float scale = 1;

    [Tooltip("Width of the health line, in pixels;")] [SerializeField]
    private int lineWidth = 4;

    [Tooltip("Offset between the bar ends and the actual bars, in pixels;")] [SerializeField]
    private float endOffset;

    [SerializeField] private Gradient[] lineColors;
    [SerializeField] private Sprite[] barSprites;

    private float[] tileBaseX;
    private float tileBaseY;

    private float tileOffset;
    private float pixelUnit;
    private int orderVal;

    private SpriteRenderer[] nodes;
    private Vector2[] nodePositions;
    private LineRenderer[] lineRenderers; // 0 -> Bright line | 1 -> White line
    private Vector2 whitelineOffset;
    private float keyValue = 1f;
    private float alphaRate = 5f;

    private enum State {
        FadeIn,
        Vibe,
        FadeOut,
    } private State state = State.FadeIn;

    // Start, but better >:D
    public void SetUp(int maxHP, int hp) {
        this.maxHP = maxHP;
        this.hp = hp;
        var enemyScript = transform.parent.GetComponent<Enemy>();
        if (enemyScript != null) {
            enemyScript.OnDamageTaken += EnemyHealthBar_OnDamageTaken;
            enemyScript.OnPlayerInRange += EnemyHealthBar_OnPlayerInRange;
        }

        pixelUnit = 1f / (float)barSprites[0].pixelsPerUnit;
        orderVal = PlayerController.Instance.GetComponent<SpriteRenderer>().sortingOrder + 4;
        swipeSpeed = swipeSpeed * scale * barLength / 48f;

        // Get the base length of each sprite tile (0.0625f * # of pixels);
        tileBaseX = new float[barSprites.Length];
        for (int i = 0; i < barSprites.Length; i++) tileBaseX[i] = barSprites[i].bounds.size.x;
        tileBaseY = barSprites[0].bounds.size.y; // The Y length should be the same for all sprites;

        // Logic (I might forget this in a week so might as well):
        // (barLength - endOffset * 2) -> Remaining pixels to fill out;
        // - (maxHP * 4 - 1)   -> Subtract initial pixels (accounted for in base). Final node has no ending (-1);
        // * pixelUnit         -> Multiply by the pixel scale to worldspace based on the pixels per unit of the sprite;
        tileOffset = ((float)((barLength - endOffset * 2) - (maxHP * 4 - 1)) / maxHP) * pixelUnit;

        nodes = new SpriteRenderer[maxHP];
        nodePositions = new Vector2[maxHP + 1];
        var currentPosition = -(barLength / 2f) * pixelUnit * scale;
        // Calculate the position of each node based on computed node length;
        for (int i = 0; i < maxHP; i++) {
            var spriteIndex = 1;
            if (i == 0) { spriteIndex = 0; } else if (i == maxHP - 1) { spriteIndex = 2; }

            var node = new GameObject("Health Node " + (i + 1));
            node.transform.SetParent(transform);
            node.transform.localPosition = new Vector2(currentPosition, transform.localPosition.y);
            if (spriteIndex == 0) nodePositions[0] = new Vector2(node.transform.position.x + endOffset * pixelUnit * scale - pixelUnit, node.transform.position.y);
            else nodePositions[i] = (Vector2)node.transform.position + new Vector2(pixelUnit / 2f, 0);
            nodes[i] = node.AddComponent<SpriteRenderer>();
            nodes[i].sprite = barSprites[spriteIndex];
            nodes[i].drawMode = SpriteDrawMode.Sliced;
            nodes[i].size = new Vector2(tileBaseX[spriteIndex] + tileOffset, tileBaseY);
            nodes[i].transform.localScale = Vector2.one * scale;
            nodes[i].sortingOrder = orderVal;
            currentPosition += (tileBaseX[spriteIndex] + tileOffset - pixelUnit) * scale;
            if (spriteIndex == 2) nodePositions[i + 1] = new Vector2(node.transform.position.x + pixelUnit / 2f, node.transform.position.y);
        }

        lineRenderers = new LineRenderer[2];
        // Set up line renderers with initial positions;
        for (int i = 0; i < lineRenderers.Length; i++) {
            lineRenderers[i] = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderers[i].transform.SetParent(transform);
            lineRenderers[i].positionCount = i != 0 ? 2 : 3;
            lineRenderers[i].SetPosition(0, nodePositions[0]);
            lineRenderers[i].SetPosition(1, nodePositions[maxHP]);
            if (i == 0) lineRenderers[i].SetPosition(2, nodePositions[maxHP]);
            var curve = new AnimationCurve();
            curve.AddKey(0.0f, lineWidth * pixelUnit);
            curve.AddKey(1.0f, lineWidth * pixelUnit);
            lineRenderers[i].widthCurve = curve;
            lineRenderers[i].material = nodes[0].material;
            lineRenderers[i].sortingOrder = orderVal - i - 1;
            lineRenderers[i].colorGradient = lineColors[i];
        }
        SetBarAlpha(0f);
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        state = State.FadeIn;
    }

    void Update() {
        if (nodes != null) {
            switch (state) {
                case State.FadeIn:
                    if (nodes[0].color.a < opacity) {
                        var color = nodes[0].color;
                        color.a = Approach(color.a, opacity, Time.deltaTime * alphaRate);
                        SetBarAlpha(color.a);
                    } else state = State.Vibe;
                    break;
                case State.FadeOut:
                    if (nodes[0].color.a > 0) {
                        var color = nodes[0].color;
                        color.a = Approach(color.a, 0, Time.deltaTime * alphaRate);
                        SetBarAlpha(color.a);
                    } else {
                        state = State.Vibe;
                        gameObject.SetActive(false);
                    } break;
            }
            UpdateLinePositions();
        }
    }

    // Bring a value closer to another without overthrowing the limit;
    private float Approach(float currentValue, float targetValue, float rate) {
        rate = Mathf.Abs(rate);
        if (currentValue < targetValue) {
            currentValue += rate;
            if (currentValue > targetValue) return targetValue;
        } else {
            currentValue -= rate;
            if (currentValue < targetValue) return targetValue;
        } return currentValue;
    }

    private void EnemyHealthBar_OnDamageTaken(int dmg) {
        if (hp < dmg) { dmg = hp; hp = 0; }
        else { hp -= dmg; }
        whitelineOffset = new Vector2(nodePositions[hp + dmg].x - nodePositions[hp].x, 0);
        StartCoroutine(BarShake(dmg * 2.5f));
        if (hp == 0) state = State.FadeOut;
    }

    private void EnemyHealthBar_OnPlayerInRange(bool playerInRange) {
        state = playerInRange ? State.FadeIn : State.FadeOut;
    }

    IEnumerator BarShake(float wiggleTarget) {
        var alpha = opacity;
        var wiggle = 0f;
        while (wiggle < wiggleTarget) {
            alpha = Approach(alpha, 1f, Time.deltaTime * 5f);
            SetBarAlpha(alpha);
            wiggle = Approach(wiggle, wiggleTarget, Time.deltaTime * 40f);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * 25f) * wiggle);
            transform.localScale = Vector3.one * (1 + wiggle / 50f);
            yield return null;
        } while (wiggle > 0) {
            alpha = Approach(alpha, 1f, Time.deltaTime * 5f);
            SetBarAlpha(alpha);
            wiggle = Approach(wiggle, 0, Time.deltaTime * 30f);
            transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(Time.time * 25f) * wiggle);
            transform.localScale = Vector3.one * (1 + wiggle / 50f);
            yield return null;
        } while (alpha > opacity) {
            alpha = Approach(alpha, opacity, Time.deltaTime * 5f);
            SetBarAlpha(alpha);
            yield return null;
        }
    }

    // Update line renderer positions over time;
    private void UpdateLinePositions() {
        for (int i = 0; i < maxHP; i++) {
            var spriteIndex = 1;
            if (i == 0) { spriteIndex = 0; } else if (i == maxHP - 1) { spriteIndex = 2; }
            if (spriteIndex == 0) nodePositions[0] = new Vector2(nodes[0].transform.position.x + endOffset * pixelUnit * scale - pixelUnit, nodes[0].transform.position.y);
            else nodePositions[i] = (Vector2) nodes[i].transform.position + new Vector2(pixelUnit / 2f, 0);
            if (spriteIndex == 2) nodePositions[i + 1] = new Vector2(nodes[i].transform.position.x
                                                                        + (nodes[i].transform.position.x - nodes[i-1].transform.position.x) + pixelUnit / 2f,
                                                                        nodes[0].transform.position.y);
        }

        var points = new Vector3[3]; 
        points[0] = nodePositions[0];
        points[1] = Vector3.Lerp(nodePositions[0], nodePositions[hp], keyValue);
        points[2] = nodePositions[hp];
        keyValue = Approach(keyValue, 1, swipeSpeed/4f);
        if (keyValue == 1f) keyValue = 0;

        lineRenderers[0].SetPositions(points);
        lineRenderers[1].SetPosition(0, points[0]);
        if ((Vector2)lineRenderers[1].GetPosition(1) != nodePositions[hp]) {
            whitelineOffset = new Vector2(Approach(whitelineOffset.x, 0, swipeSpeed), 0);
        } lineRenderers[1].SetPosition(1, nodePositions[hp] + whitelineOffset);
    }

    private void SetBarAlpha(float alpha) {
        foreach (SpriteRenderer sprRenderer in nodes) sprRenderer.color = new Color(255, 255, 255, alpha);
        foreach (LineRenderer lineRenderer in lineRenderers) {
            var gradient = lineRenderer.colorGradient;
            gradient.SetKeys(gradient.colorKeys, new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0f), new GradientAlphaKey(alpha, 1f) });
            lineRenderer.colorGradient = gradient;
        }
    }
}