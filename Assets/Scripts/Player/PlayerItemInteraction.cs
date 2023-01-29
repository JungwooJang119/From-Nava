using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] GameObject textPopUpLocation;
    [SerializeField] float secondsToDestroy;
    [SerializeField] float range;
    float currentDistance;
    Vector2 playerPosition;
    GameObject player;
    bool isTextInstantiated = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerPosition = player.GetComponent<Transform>().position;
            currentDistance = (playerPosition - (Vector2)transform.position).magnitude;
            if (currentDistance < range)
            {
                // display UI text
                // if floatingTextPrefab has something assigned to it and text hasn't been spawned yet
                if (floatingTextPrefab && isTextInstantiated != true)
                {
                    isTextInstantiated = true;
                    GameObject prefab = Instantiate(floatingTextPrefab, textPopUpLocation.transform.position, Quaternion.identity);
                    Destroy(prefab, secondsToDestroy);
                }
            }
        }
    }
}
