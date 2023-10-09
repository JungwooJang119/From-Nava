using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB1 : MonoBehaviour
{
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject enemy3;
    [SerializeField] private GameObject enemy4;
    [SerializeField] private GameObject enemy5;
    [SerializeField] private GameObject enemy6;

    [SerializeField] private GameObject chest;
    [SerializeField] private bool isC1;
    [SerializeField] private GameObject door;
    //[SerializeField] private GameObject spellNotif;

    private bool isActive = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == true) {
            if (enemy1 == null && enemy2 == null && enemy3 == null && enemy4 == null && enemy5 == null && enemy6 == null) {
                if (chest != null) {
                    chest.SetActive(true);
                    AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
                }
                // spellNotif.SetActive(true);
                // StartCoroutine(DurationTime());
                isActive = false;
                if (isC1) {
                    door.GetComponent<Door>().OpenDoor();
                }
                Destroy(this.gameObject);
            }
        }
    }

}
