using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB1 : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;

    [SerializeField] private GameObject chest;
    //[SerializeField] private GameObject spellNotif;

    private bool isActive = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive = true) {
            if (enemy1 == null && enemy2 == null) {
                chest.SetActive(true);
                // spellNotif.SetActive(true);
                // StartCoroutine(DurationTime());
                isActive = false;
                Destroy(this.gameObject);
            }
        }
    }

}
