using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] float range;
    float currentDistance;
    Vector2 playerPosition;
    GameObject player;

    /*UI element*/
    [SerializeField] TMP_Text text;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerPosition = player.GetComponent<Transform>().position;
            currentDistance = (playerPosition - (Vector2)transform.position).magnitude;
            if (currentDistance < range)
            {
                // display UI text
            }
        }
    }
}
