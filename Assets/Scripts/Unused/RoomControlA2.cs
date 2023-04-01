using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlA2 : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood_Script[] _firewoods;
    private bool _canClear = true;

    [SerializeField] private bool isClear = false;
    //[SerializeField] private GameObject spellNotif;
    public GameObject A2Chest;
    public bool cheat = false;
    
    // Start is called before the first frame update
    void Start() {
		_firewoods = firewoodController.GetComponentsInChildren<Firewood_Script>();
	}

    // Update is called once per frame
    void Update() {
        if (!isClear) {
            foreach (Firewood_Script _firewood in _firewoods) {
                if (!_firewood.isLit) { _canClear = false; break; }
            }

			if (_canClear) {
				A2Chest.SetActive(true);
				//spellNotif.SetActive(true);
				StartCoroutine(DurationTime());
				isClear = true;
				Destroy(this.gameObject);
			}
			_canClear = true;

			if (cheat) {
                A2Chest.SetActive(true);
                //spellNotif.SetActive(true);
                //StartCoroutine(DurationTime());
                isClear = true;
                Destroy(this.gameObject);
            }
        }
    }

	IEnumerator DurationTime() {
		yield return new WaitForSeconds(5f);
		//spellNotif.SetActive(false);
	}
}
