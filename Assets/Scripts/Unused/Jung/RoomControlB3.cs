using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlB3 : MonoBehaviour
{
    [SerializeField] private GameObject fan1;
    [SerializeField] private GameObject fan2;
    [SerializeField] private GameObject fan3;
    [SerializeField] private GameObject fan4;
    [SerializeField] private GameObject fan5;
    [SerializeField] private GameObject fan6;

    public Fan p1;
    public Fan p2;
    public Fan p3;
    public Fan p4;
    public Fan p5;
    public Fan p6;

    [SerializeField] private GameObject chest;
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
            if (p1.IsBlowing() && p2.IsBlowing() && p3.IsBlowing() && p4.IsBlowing() && p5.IsBlowing() && p6.IsBlowing()) {
                chest.SetActive(true);
                isActive = false;
            }
        }
    }
}
