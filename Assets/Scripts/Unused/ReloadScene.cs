using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private GameObject spellNotif;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Spell") {
            return;
        }
        print("Reset");
        StartCoroutine(TextPopUp());
    }

    IEnumerator TextPopUp() {
        spellNotif.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        spellNotif.SetActive(false);
    }
}
