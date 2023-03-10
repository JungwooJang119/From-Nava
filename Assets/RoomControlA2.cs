using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlA2 : MonoBehaviour
{
    public GameObject firewood1;
    public GameObject firewood2;
    public GameObject firewood3;
    public GameObject firewood4;
    public GameObject firewood5;
    public GameObject firewood6;
    public GameObject firewood7;

    public Firewood_Script p1;
    public Firewood_Script p2;
    public Firewood_Script p3;
    public Firewood_Script p4;
    public Firewood_Script p5;
    public Firewood_Script p6;
    public Firewood_Script p7;

    [SerializeField] private bool isClear = false;
    [SerializeField] private GameObject spellNotif;
    public GameObject A1Chest;
    

    // Start is called before the first frame update
    void Start()
    {
        p1 = firewood1.GetComponent<Firewood_Script>();
        p2 = firewood2.GetComponent<Firewood_Script>();
        p3 = firewood3.GetComponent<Firewood_Script>();
        p4 = firewood4.GetComponent<Firewood_Script>();
        p5 = firewood5.GetComponent<Firewood_Script>();
        p6 = firewood6.GetComponent<Firewood_Script>();
        p7 = firewood7.GetComponent<Firewood_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClear) {
            if (p1.isLit && p2.isLit && p3.isLit && p4.isLit && p5.isLit && p6.isLit && p7.isLit) {
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
}
