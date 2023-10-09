using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlC1_3 : MonoBehaviour
{
    [SerializeField] private GameObject fan1;
    [SerializeField] private GameObject fan2;
    [SerializeField] private GameObject fan3;
    [SerializeField] private GameObject fan4;
    [SerializeField] private GameObject fan5;
    [SerializeField] private GameObject fan6;

    private Fan p1;
    private Fan p2;
    private Fan p3;
    private Fan p4;
    private Fan p5;
    private Fan p6;

    public GameObject door1;

    private bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        p1 = fan1.GetComponent<Fan>();
        p2 = fan2.GetComponent<Fan>();
        p3 = fan3.GetComponent<Fan>();
        p4 = fan4.GetComponent<Fan>();
        p5 = fan5.GetComponent<Fan>();
        p6 = fan6.GetComponent<Fan>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            if (!p1.IsBlowing() && !p2.IsBlowing() && !p3.IsBlowing() && !p4.IsBlowing() && !p5.IsBlowing() && !p6.IsBlowing()) {
                door1.GetComponent<Door>().OpenDoor();
                AudioControl.Instance.PlaySFX("PuzzleComplete", PlayerController.Instance.gameObject, 0f, 1f);
                isActive = false;
            }
        }
        if (!isActive) {
            if (p1.IsBlowing() || p2.IsBlowing() || p3.IsBlowing() || p4.IsBlowing() || p5.IsBlowing() || p6.IsBlowing()) {
                door1.GetComponent<Door>().CloseDoor();
                isActive = true;
            }
        }
    }
}
