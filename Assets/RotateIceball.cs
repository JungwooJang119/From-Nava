using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIceball : MonoBehaviour
{

    private Transform[] sprTransforms;  // Array of references to the transforms of children;
    [SerializeField] private bool rotateInner;


    // Start is called before the first frame update
    void Start()
    {
        // Variables to manage sprite rotation;
        sprTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            sprTransforms[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Continously rotate sprites;
		RotateSprites(-3.75f, 2.5f, 25f);
    }

    private void RotateSprites(float rotationRateOuter1, float rotationRateOuter2, float rotationRateInner) {
        int tranNumber = 0;
		rotationRateOuter1 *= Time.deltaTime * 60;
		rotationRateOuter2 *= Time.deltaTime * 60;
		rotationRateInner *= Time.deltaTime * 60;
		foreach (Transform transform in sprTransforms) {
            if (tranNumber == 0) {
                if (rotateInner) {
                    transform.Rotate(0, 0, rotationRateInner);
                }
				tranNumber = 1;
            } else if (tranNumber == 1) {
				transform.Rotate(0, 0, rotationRateOuter1);
				tranNumber = 2;
			} else {
				transform.Rotate(0, 0, rotationRateOuter2);
                tranNumber = 0;
			}
        }
    }
}
