using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileDefinition))]
[CanEditMultipleObjects]
public class TileDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Set Name From File"))
        {
            foreach (var obj in targets)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

                SerializedObject so = new SerializedObject(obj);
                so.FindProperty("tileDefinitionName").stringValue = fileName;
                so.ApplyModifiedProperties();
            }
        }
    }
}