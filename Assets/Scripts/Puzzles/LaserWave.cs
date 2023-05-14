using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an aesthetic wave summoned by the caser,
// and the receiver objects of the mirror/laser system;
public class LaserWave : MonoBehaviour
{
    // Reference variables to control the visual behavior of the wave;
    [SerializeField] private float growthRate;
    [SerializeField] private float vanishRate;
    [SerializeField] private float maxGrowth;

	// Reference to the sprite renderer;
	private SpriteRenderer sprRenderer;

    // Active variables to control the visual behavior of the wave;
    private float currentGrowth = 0f;
    private float currentVanish = 0f;

    // Grab sprite render and begin at an infitesimal scale;
    void Start() {
		sprRenderer = gameObject.GetComponent<SpriteRenderer>();
		transform.localScale = new Vector3(0, 0, 0);
    }

    // Update the scale and alpha of the sprite overtime;
    void Update() {
        var growthRate = this.growthRate * Time.deltaTime;
        var vanishRate = this.vanishRate * Time.deltaTime;
        if (currentGrowth < maxGrowth) {
            currentGrowth += growthRate;
            currentVanish += vanishRate;
        } else {
            Destroy(gameObject);
        }
		transform.localScale = new Vector3(currentGrowth, currentGrowth, currentGrowth);
        sprRenderer.color = new Color(1f, 1f, 1f, 1f - currentVanish);
	}
}
