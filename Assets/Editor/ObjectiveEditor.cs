using System;
using UnityEditor;
[CustomEditor(typeof(ObjectiveBase))]
[CanEditMultipleObjects]
public class ObjectiveEditor : Editor
{
    SerializedProperty milestone;
    SerializedProperty itemKey;
    SerializedProperty sceneKey;
    SerializedProperty isTouched;
    SerializedProperty objectiveType;
    SerializedProperty onComplete;
    

    public void OnEnable()
    {
        milestone = serializedObject.FindProperty("milestone");
        itemKey = serializedObject.FindProperty("itemKey");
        sceneKey = serializedObject.FindProperty("sceneKey");
        isTouched = serializedObject.FindProperty("isTouched");
        objectiveType = serializedObject.FindProperty("objectiveType");
        onComplete = serializedObject.FindProperty("OnComplete");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(objectiveType);
        EditorGUILayout.PropertyField(onComplete);
        serializedObject.ApplyModifiedProperties();
        switch (objectiveType.enumValueIndex)
        {
            //location 
            case 0:
                EditorGUILayout.PropertyField(isTouched);
                serializedObject.ApplyModifiedProperties();
                return;
            case 1:
                EditorGUILayout.PropertyField(itemKey);
                serializedObject.ApplyModifiedProperties();
                return;
            case 2:
                EditorGUILayout.PropertyField(sceneKey);
                serializedObject.ApplyModifiedProperties();
                return;
            case 3:
                EditorGUILayout.PropertyField(milestone);
                serializedObject.ApplyModifiedProperties();
                return;
                
        }
        
    }
}