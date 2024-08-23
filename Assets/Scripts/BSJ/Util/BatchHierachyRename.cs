using UnityEditor;
using UnityEngine;

public class BatchHierachyRename : EditorWindow
{
    private string baseName = "NewName";
    private int startNumber = 1;
    private string findWord = "";
    private string replaceWord = "";

    [MenuItem("Tools/Batch Hierachy Rename")]
    public static void ShowWindow()
    {
        GetWindow<BatchHierachyRename>("Batch Rename");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Selected Objects", EditorStyles.boldLabel);

        baseName = EditorGUILayout.TextField("Base Name", baseName);
        startNumber = EditorGUILayout.IntField("Start Number", startNumber);

        GUILayout.Space(10);

        GUILayout.Label("Find and Replace", EditorStyles.boldLabel);
        findWord = EditorGUILayout.TextField("Find", findWord);
        replaceWord = EditorGUILayout.TextField("Replace With", replaceWord);

        GUILayout.Space(10);

        if (GUILayout.Button("Preview Rename"))
        {
            PreviewRename();
        }

        if (GUILayout.Button("Rename"))
        {
            if (EditorUtility.DisplayDialog("Confirm Rename", "Are you sure you want to rename the selected objects?", "Yes", "No"))
            {
                RenameSelectedObjects();
            }
        }

        if (GUILayout.Button("Find and Replace"))
        {
            if (EditorUtility.DisplayDialog("Confirm Find and Replace", "Are you sure you want to find and replace the text in the selected objects' names?", "Yes", "No"))
            {
                FindAndReplaceInSelectedObjects();
            }
        }
    }

    private void PreviewRename()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "No objects selected for renaming.", "OK");
            return;
        }

        string previewMessage = "The selected objects will be renamed as follows:\n";
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            previewMessage += $"{selectedObjects[i].name} -> {baseName}{startNumber + i}\n";
        }

        EditorUtility.DisplayDialog("Rename Preview", previewMessage, "OK");
    }

    private void RenameSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "No objects selected for renaming.", "OK");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Batch Rename");

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].name = $"{baseName}{startNumber + i}";
        }

        EditorUtility.DisplayDialog("Rename Complete", $"{selectedObjects.Length} objects renamed.", "OK");
    }

    private void FindAndReplaceInSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "No objects selected for find and replace.", "OK");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Batch Find and Replace");

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].name = selectedObjects[i].name.Replace(findWord, replaceWord);
        }

        EditorUtility.DisplayDialog("Find and Replace Complete", $"{selectedObjects.Length} objects processed.", "OK");
    }
}
