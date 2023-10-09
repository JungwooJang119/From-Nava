using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision) {
        ReferenceSingleton.Instance.transition.LoadEnding();
    }
}
