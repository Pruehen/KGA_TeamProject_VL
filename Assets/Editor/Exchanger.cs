using UnityEngine;
using UnityEditor;

public class Exchanger : EditorWindow
{
    public GameObject[] AObjects; // A ������Ʈ �迭
    public GameObject prefab;     // ��ü�� ������

    [MenuItem("Tools/Transform Sync and Replace")]
    public static void ShowWindow()
    {
        GetWindow<Exchanger>("Transform Sync and Replace");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sync and Replace A Objects with Prefab", EditorStyles.boldLabel);

        // AObjects �迭�� �Է¹���
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty aObjectsProperty = serializedObject.FindProperty("AObjects");
        EditorGUILayout.PropertyField(aObjectsProperty, true);

        // Prefab�� �Է¹���
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
            Debug.LogError("AObjects �迭�� ��� �ֽ��ϴ�.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("�������� �������� �ʾҽ��ϴ�.");
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
                newBObject.transform.SetParent(AObjects[i].transform.parent);  // AObjects�� �θ� �״�� ������

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
    public GameObject[] AObjects; // A ������Ʈ �迭
    public GameObject prefab;     // ��ü�� ������
    public float randomYOffset = 0.5f; // Y�࿡ ������ ���� ������ ����

    [MenuItem("Tools/Replacey")]
    public static void ShowWindow()
    {
        GetWindow<TransformSyncWindow>("Replacey");
    }

    private void OnGUI()
    {
        GUILayout.Label("Sync and Replace A Objects with Prefab", EditorStyles.boldLabel);

        // AObjects �迭�� �Է¹���
        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty aObjectsProperty = serializedObject.FindProperty("AObjects");
        EditorGUILayout.PropertyField(aObjectsProperty, true);

        // Prefab�� �Է¹���
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);

        // Random Y offset�� �Է¹���
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
            Debug.LogError("AObjects �迭�� ��� �ֽ��ϴ�.");
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("�������� �������� �ʾҽ��ϴ�.");
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
                newBObject.transform.SetParent(AObjects[i].transform.parent);  // AObjects�� �θ� �״�� ������

                // Copy transform values from A to the new prefab instance
                newBObject.transform.position = AObjects[i].transform.position;
                newBObject.transform.rotation = AObjects[i].transform.rotation;
                newBObject.transform.localScale = AObjects[i].transform.localScale;

                // ù ��° �ڽ� ������Ʈ�� Y ��ġ�� ���� ������ �߰�
                if (newBObject.transform.childCount > 0)
                {
                    Transform firstChild = newBObject.transform.GetChild(0);
                    Vector3 childPosition = firstChild.localPosition;

                    // Y�࿡ -randomYOffset ~ +randomYOffset ������ ������ ����
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