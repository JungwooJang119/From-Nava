using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlA1 : MonoBehaviour
{
    public GameObject plate1;
    public GameObject plate2;
    public GameObject plate3;
    public GameObject plate4;
    public GameObject plate5;
    public GameObject plate6;

    public PressurePlate_Script p1;
    public PressurePlate_Script p2;
    public PressurePlate_Script p3;
    public PressurePlate_Script p4;
    public PressurePlate_Script p5;
    public PressurePlate_Script p6;

    public GameObject A1Chest;

    public bool check1;
    public bool check2;
    public bool check3;
    public bool check4;
    public bool check5;
    public bool check6;

    [SerializeField] private bool isClear = false;
    [SerializeField] private GameObject spellNotif;

    void Start() {
        p1 = plate1.GetComponent<PressurePlate_Script>();
        p2 = plate2.GetComponent<PressurePlate_Script>();
        p3 = plate3.GetComponent<PressurePlate_Script>();
        p4 = plate4.GetComponent<PressurePlate_Script>();
        p5 = plate5.GetComponent<PressurePlate_Script>();
        p6 = plate6.GetComponent<PressurePlate_Script>();
    }

    void Update() {
        checkPlates();
        if (!isClear) {
            if (check1 && check2 && check3 && check4 && check5 && check6) {
                A1Chest.SetActive(true);
                spellNotif.SetActive(true);
                StartCoroutine(DurationTime());
                isClear = true;
            }
        }
    }

    IEnumerator DurationTime() {
		yield return new WaitForSeconds(3f);
        spellNotif.SetActive(false);
	}

    void checkPlates() {
        if (p1.getIsPressed()) {
            check1 = true;
        } else {
            check1 = false;
        }
        if (p2.getIsPressed()) {
            check2 = true;
        }
        else {
            check2 = false;
        }
        if (p3.getIsPressed()) {
            check3 = true;
        }
        else {
            check3 = false;
        }
        if (p4.getIsPressed()) {
            check4 = true;
        }
        else {
            check4 = false;
        }
        if (p5.getIsPressed()) {
            check5 = true;
        }
        else {
            check5 = false;
        }
        if (p6.getIsPressed()) {
            check6 = true;
        }
        else {
            check6 = false;
        }

    }


}
