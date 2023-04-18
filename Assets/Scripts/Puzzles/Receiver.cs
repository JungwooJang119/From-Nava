using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Endpoint of the Laser/Mirror puzzle.
// The puzzle attempt is successful if,
// the laser beam collides with this object;

public class Receiver : MonoBehaviour {

	// Reference to the laserWave to be generated;
	[SerializeField] private GameObject laserWave;

	// Generate laser waves on collision with the laser beam;
	// Called by the laser beam OnCollision2D;
	public void Feedback(float life) {
		StartCoroutine(SoundDelay(life));
	}

	// Coroutine to repeat the wave and sound twice;
	IEnumerator SoundDelay(float life) {
		yield return new WaitForSeconds(life);
		AudioControl.Instance.PlayVoidSFX("Receiver Triggered");
		Instantiate(laserWave, transform.position, transform.rotation);
	}
}
