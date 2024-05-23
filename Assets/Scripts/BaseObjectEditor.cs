using UnityEditor;

[CustomEditor(typeof(BaseObject))]
public class BaseObjectEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}