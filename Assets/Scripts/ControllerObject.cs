using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerObject : MonoBehaviour
{
    private BaseObject[] objList;
    void Awake() {
		objList = GetComponentsInChildren<BaseObject>();
	}
    public void StartReset() {
        foreach (BaseObject obj in objList) {
            if (obj != null) {
				obj.ObjectReset();
			}
        }
    }
}
