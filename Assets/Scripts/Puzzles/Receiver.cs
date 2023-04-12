using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Endpoint of the Laser/Mirror puzzle.
// The puzzle attempt is successful if,
// the laser beam collides with this object;

public class Receiver : MonoBehaviour {

	// Reference to the laserWave to be generated;
	public GameObject laserWave;

	// Generate laser waves on collision with the laser beam;
	// Called by the laser beam OnCollision2D;
	public void feedback(float life) {
		StartCoroutine(SoundDelay(life));
	}

	// Coroutine to repeat the wave and sound twice;
	IEnumerator SoundDelay(float life) {
		yield return new WaitForSeconds(life);
		var _dur = AudioControl.Instance.PlaySFX("Receiver Triggered");
		Instantiate(laserWave, transform.position, transform.rotation);
		yield return new WaitForSeconds(_dur);
		AudioControl.Instance.PlaySFX("Receiver Triggered");
		Instantiate(laserWave, transform.position, transform.rotation);
	}
}
