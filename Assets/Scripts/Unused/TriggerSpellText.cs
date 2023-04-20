using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpellText : MonoBehaviour
{
    [SerializeField] private GameObject spellNotif;
    public bool isDirections = false;
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
            StartCoroutine(TextPopUp());
        }
    }

    IEnumerator DurationTime() {
		yield return new WaitForSeconds(3f);
        spellNotif.SetActive(false);
	}

    IEnumerator TextPopUp() {
        spellNotif.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        spellNotif.SetActive(false);
        if (isDirections) {
            Destroy(this.gameObject);
        }
    }
}
