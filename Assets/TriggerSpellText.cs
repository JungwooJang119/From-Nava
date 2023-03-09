using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpellText : MonoBehaviour
{
    [SerializeField] private GameObject spellNotif;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            spellNotif.SetActive(true);
            StartCoroutine(DurationTime());
        }
    }

    IEnumerator DurationTime() {
		yield return new WaitForSeconds(3f);
        spellNotif.SetActive(false);
	}
}
