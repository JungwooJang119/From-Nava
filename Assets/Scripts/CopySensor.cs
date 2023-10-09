using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySensor : MonoBehaviour
{
    [SerializeField] private string objectTagName;
    private bool isCorrect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == objectTagName) {
            isCorrect = true;
        }
    }

    public bool GetStatus() {
        return isCorrect;
    }
}
