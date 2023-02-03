using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ItemInteractionTextPopup : MonoBehaviour
{
    [SerializeField] GameObject floatingTextPrefab;     // text popup
    [SerializeField] GameObject textPopUpLocation;      // empty text pop up location
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
            // checks if player is near the object
            playerPosition = player.GetComponent<Transform>().position;
            currentDistance = (playerPosition - (Vector2)transform.position).magnitude;
            // if the player is in range of the object
            if (currentDistance < range)
            {
                // if floatingTextPrefab has something assigned to it and text hasn't been spawned yet (makes only one clone spawn at a time);
                if (floatingTextPrefab && isTextInstantiated != true)
                {
                    isTextInstantiated = true;
                    // instantiate text
                    GameObject prefab = Instantiate(floatingTextPrefab, textPopUpLocation.transform.position, Quaternion.identity);
                    // destroy after x seconds
                    Destroy(prefab, secondsToDestroy);
                    // wait for x seconds before new text can be spawned
                    StartCoroutine(WaitForSeconds());
                }
            }
        }
    }
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(secondsToDestroy);
        // after x seconds, text is NOT instantiated
        isTextInstantiated = false;
    }
}


