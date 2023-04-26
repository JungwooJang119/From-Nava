using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerController playerControl;
    private int health;
    public Image[] healthBars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 10; i++) {
            healthBars[i].gameObject.SetActive(false);
        }
        health = playerControl.playerHealth;
        if (health >= 9) {
            healthBars[9].gameObject.SetActive(true);
        } else if (health >= 8) {
            healthBars[8].gameObject.SetActive(true);
        } else if (health >= 7) {
            healthBars[7].gameObject.SetActive(true);
        } else if (health >= 6) {
            healthBars[6].gameObject.SetActive(true);
        } else if (health >= 5) {
            healthBars[5].gameObject.SetActive(true);
        } else if (health >= 4) {
            healthBars[4].gameObject.SetActive(true);
        } else if (health >= 3) {
            healthBars[3].gameObject.SetActive(true);
        } else if (health >= 2) {
            healthBars[2].gameObject.SetActive(true);
        } else if (health >= 1) {
            healthBars[1].gameObject.SetActive(true);
        } else if (health < 1) {
            healthBars[0].gameObject.SetActive(true);
        }
    }
}
