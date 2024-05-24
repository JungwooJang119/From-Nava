using UnityEngine;

public abstract class ObjectModule : MonoBehaviour {
    protected BaseObject baseObject;
    protected virtual void Awake() => baseObject = GetComponent<BaseObject>();
}
