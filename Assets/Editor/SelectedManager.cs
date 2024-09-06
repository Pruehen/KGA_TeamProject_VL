using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class SelectedManager
{
    public static object SelectedObject;
    static SelectedManager()
    {
        // Subscribe to the selection changed event
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void OnSelectionChanged()
    {
        // Get the currently selected objects
        Object[] selectedObjects = Selection.objects;

        // If no object is selected, exit
        if (selectedObjects.Length == 0)
            return;
        if(selectedObjects.Length == 1)
        {
            SelectedObject = selectedObjects[0];
        }

        // Iterate over the selected objects and print their names
        foreach (Object selectedObject in selectedObjects)
        {
            Debug.Log("Selected object: " + selectedObject.name);
        }
    }
}