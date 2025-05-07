using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Orbit))]
public class OrbitEditor : Editor {

    Orbit targetScript;

    private void Awake()
    {
        targetScript = (Orbit)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        Undo.RecordObject(target, "Quick Ads");
        {
            EditorGUILayout.Space();
            targetScript.speed = EditorGUILayout.FloatField("Speed", targetScript.speed);
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            targetScript.angle = EditorGUILayout.Slider("Angle",targetScript.angle, 0, 360);
            EditorGUI.EndDisabledGroup();
            if (!Application.isPlaying)
            {
                targetScript.transform.rotation = Quaternion.Euler(Vector3.forward * targetScript.angle);
                targetScript.transform.GetChild(0).rotation = Quaternion.identity;
            }
            targetScript.distance = EditorGUILayout.FloatField("Distance",targetScript.distance >= 0 ? targetScript.distance : 0);
            targetScript.transform.GetChild(0).localPosition = Vector3.right * targetScript.distance;
        }
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

}
