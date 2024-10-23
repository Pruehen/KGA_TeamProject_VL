using UnityEngine;
using UnityEditor;

public class Exchanger : EditorWindow
{
    public GameObject[] AObjects; // A 오브젝트 배열
    public GameObject prefab;     // 교체할 프리팹

    [MenuItem("Tools/Transform Sync and Replace")]
    public static void ShowWindow()
    {
        GetWindow<Exchanger>("Transform Sync and Replace");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sync and Replace A Objects with Prefab", EditorStyles.boldLabel);

        // AObjects 배열을 입력받음
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty aObjectsProperty = serializedObject.FindProperty("AObjects");
        EditorGUILayout.PropertyField(aObjectsProperty, true);

        // Prefab을 입력받음
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("Sync and Replace"))
        {
            SyncAndReplaceWithPrefab();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SyncAndReplaceWithPrefab()
    {
        if (AObjects == null || AObjects.Length == 0)
        {
            Debug.LogError("AObjects 배열이 비어 있습니다.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("프리팹이 지정되지 않았습니다.");
            return;
        }

        for (int i = 0; i < AObjects.Length; i++)
        {
            if (AObjects[i] != null)
            {
                // Record the A object to allow undo
                Undo.RecordObject(AObjects[i], "Disable A Object");

                // Instantiate the prefab at the same location and under the same parent as the A object
                GameObject newBObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newBObject.transform.SetParent(AObjects[i].transform.parent);  // AObjects의 부모를 그대로 가져옴

                // Copy transform values from A to the new prefab instance
                newBObject.transform.position = AObjects[i].transform.position;
                newBObject.transform.rotation = AObjects[i].transform.rotation;
                newBObject.transform.localScale = AObjects[i].transform.localScale;

                // Record the newly instantiated prefab for undo
                Undo.RegisterCreatedObjectUndo(newBObject, "Instantiate Prefab");

                // Disable the A object
                AObjects[i].SetActive(false);

                // Mark objects as dirty to ensure changes are saved
                EditorUtility.SetDirty(newBObject);
                EditorUtility.SetDirty(AObjects[i]);
            }
            else
            {
                Debug.LogWarning($"Element at index {i} is null in AObjects.");
            }
        }

        // Clear the AObjects array
        for (int i = 0; i < AObjects.Length; i++)
        {
            AObjects[i] = null;
        }

        // Ensure changes are saved in the editor
        EditorUtility.SetDirty(this);
    }
}

public class TransformSyncWindow : EditorWindow
{
    public GameObject[] AObjects; // A 오브젝트 배열
    public GameObject prefab;     // 교체할 프리팹
    public float randomYOffset = 0.5f; // Y축에 적용할 랜덤 오프셋 범위

    [MenuItem("Tools/Replacey")]
    public static void ShowWindow()
    {
        GetWindow<TransformSyncWindow>("Replacey");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sync and Replace A Objects with Prefab", EditorStyles.boldLabel);

        // AObjects 배열을 입력받음
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty aObjectsProperty = serializedObject.FindProperty("AObjects");
        EditorGUILayout.PropertyField(aObjectsProperty, true);

        // Prefab을 입력받음
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        // Random Y offset을 입력받음
        randomYOffset = EditorGUILayout.FloatField("Random Y Offset", randomYOffset);

        if (GUILayout.Button("Sync and Replace"))
        {
            SyncAndReplaceWithPrefab();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SyncAndReplaceWithPrefab()
    {
        if (AObjects == null || AObjects.Length == 0)
        {
            Debug.LogError("AObjects 배열이 비어 있습니다.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("프리팹이 지정되지 않았습니다.");
            return;
        }

        for (int i = 0; i < AObjects.Length; i++)
        {
            if (AObjects[i] != null)
            {
                // Record the A object to allow undo
                Undo.RecordObject(AObjects[i], "Disable A Object");

                // Instantiate the prefab at the same location and under the same parent as the A object
                GameObject newBObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newBObject.transform.SetParent(AObjects[i].transform.parent);  // AObjects의 부모를 그대로 가져옴

                // Copy transform values from A to the new prefab instance
                newBObject.transform.position = AObjects[i].transform.position;
                newBObject.transform.rotation = AObjects[i].transform.rotation;
                newBObject.transform.localScale = AObjects[i].transform.localScale;

                // 첫 번째 자식 오브젝트의 Y 위치에 랜덤 오프셋 추가
                if (newBObject.transform.childCount > 0)
                {
                    Transform firstChild = newBObject.transform.GetChild(0);
                    Vector3 childPosition = firstChild.localPosition;

                    // Y축에 -randomYOffset ~ +randomYOffset 범위의 랜덤값 적용
                    childPosition.y += Random.Range(-randomYOffset, randomYOffset);
                    firstChild.localPosition = childPosition;
                }

                // Record the newly instantiated prefab for undo
                Undo.RegisterCreatedObjectUndo(newBObject, "Instantiate Prefab");

                // Disable the A object
                AObjects[i].SetActive(false);

                // Mark objects as dirty to ensure changes are saved
                EditorUtility.SetDirty(newBObject);
                EditorUtility.SetDirty(AObjects[i]);
            }
            else
            {
                Debug.LogWarning($"Element at index {i} is null in AObjects.");
            }
        }

        // Clear the AObjects array
        for (int i = 0; i < AObjects.Length; i++)
        {
            AObjects[i] = null;
        }

        // Ensure changes are saved in the editor
        EditorUtility.SetDirty(this);
    }
}