using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlA2 : MonoBehaviour
{
    public GameObject firewood1, firewood2, firewood3, firewood4, firewood5;
    public GameObject firewood6, firewood7, firewood8, firewood9, firewood10;
    public GameObject firewood11, firewood12, firewood13,firewood14;
    public GameObject firewood15;
    public GameObject firewood16;
    public GameObject firewood17;
    public GameObject firewood18;
    public GameObject firewood19;
    public GameObject firewood20;
    public GameObject firewood21;
    public GameObject firewood22;
    public GameObject firewood23;
    public GameObject firewood24;
    public GameObject firewood25;

    private Firewood_Script p1, p2, p3, p4, p5;
    private Firewood_Script p6, p7, p8, p9, p10;
    private Firewood_Script p11, p12, p13, p14, p15;
    private Firewood_Script p16, p17, p18, p19, p20;
    private Firewood_Script p21, p22, p23, p24, p25;

    [SerializeField] private bool isClear = false;
    [SerializeField] private GameObject spellNotif;
    public GameObject A2Chest;
    public bool cheat = false;
    

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
        p8 = firewood8.GetComponent<Firewood_Script>();
        p9 = firewood9.GetComponent<Firewood_Script>();
        p10 = firewood10.GetComponent<Firewood_Script>();
        p11 = firewood11.GetComponent<Firewood_Script>();
        p12 = firewood12.GetComponent<Firewood_Script>();
        p13 = firewood13.GetComponent<Firewood_Script>();
        p14 = firewood14.GetComponent<Firewood_Script>();
        p15 = firewood15.GetComponent<Firewood_Script>();
        p16 = firewood16.GetComponent<Firewood_Script>();
        p17 = firewood17.GetComponent<Firewood_Script>();
        p18 = firewood18.GetComponent<Firewood_Script>();
        p19 = firewood19.GetComponent<Firewood_Script>();
        p20 = firewood20.GetComponent<Firewood_Script>();
        p21 = firewood21.GetComponent<Firewood_Script>();
        p22 = firewood22.GetComponent<Firewood_Script>();
        p23 = firewood23.GetComponent<Firewood_Script>();
        p24 = firewood24.GetComponent<Firewood_Script>();
        p25 = firewood25.GetComponent<Firewood_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isClear) {
        if (p1.isLit && p2.isLit && p3.isLit && p4.isLit && p5.isLit && p6.isLit && p7.isLit && p8.isLit && p9.isLit) {
            if (p10.isLit && p11.isLit && p12.isLit && p13.isLit && p14.isLit && p15.isLit && p16.isLit && p17.isLit) {
                if (p18.isLit && p19.isLit && p20.isLit && p21.isLit && p22.isLit && p23.isLit && p24.isLit && p25.isLit) {
                    A2Chest.SetActive(true);
                    spellNotif.SetActive(true);
                    StartCoroutine(DurationTime());
                    isClear = true;
                    Destroy(this.gameObject);
                }
            }
        }
        }
        if (cheat) {
            A2Chest.SetActive(true);
                    spellNotif.SetActive(true);
                    StartCoroutine(DurationTime());
                    isClear = true;
                    Destroy(this.gameObject);
        }
    }

    IEnumerator DurationTime() {
		yield return new WaitForSeconds(5f);
        spellNotif.SetActive(false);
	}
}
