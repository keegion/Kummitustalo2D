using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParallaxOptions))]
public class ParallaxOptionEditor : Editor {
    private ParallaxOptions po;
    void Awake() {
        po = (ParallaxOptions)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save Position")) {
            po.SaveCameraPosition();
            EditorUtility.SetDirty(po);
        }
        if (GUILayout.Button("Load Position")) {
            po.LoadCameraPosition();
        }
    }
}
