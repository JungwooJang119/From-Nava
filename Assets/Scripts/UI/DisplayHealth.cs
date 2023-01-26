using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour
{
    private Text healthText;
    [SerializeField] private Transform player;
    private int health;
    private LogicScript logic;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<Text>();
        logic = GameObject.Find("Logic Manager").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        health = logic.playerHealth;
        healthText.text = "Health: " + health;
    }
}