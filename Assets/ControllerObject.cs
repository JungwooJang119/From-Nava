using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour
{
    private ObjectClass[] objList;
    void Awake() {
		objList = GetComponentsInChildren<ObjectClass>();
	}
    public void StartReset() {
        foreach (ObjectClass obj in objList) {
            if (obj != null) {
				obj.Reset();
			}
        }
    }
}
