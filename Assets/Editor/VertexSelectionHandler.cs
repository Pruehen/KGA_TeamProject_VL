using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class VertexSelectionHandler
{
    private static bool isSelectingVertex = false;
    private static Vector3 selectedVertexPosition;
    private static List<Vector3> cachedChildrenPositions = new List<Vector3>();

    public static bool IsSelectingVertex => isSelectingVertex;
    public static Vector3 SelectedVertexPosition => selectedVertexPosition;

    public static Vector3 GetSelectedVertexPosition()
    {
        return selectedVertexPosition;
    }

    public static void StartVertexSelection()
    {
        isSelectingVertex = true;
        Tools.current = Tool.None;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public static void CancelVertexSelection()
    {
        isSelectingVertex = false;
        SceneView.duringSceneGui -= OnSceneGUI;
        Tools.current = Tool.Move;
        SceneView.RepaintAll();
    }

    public static void SetParentPosition(GameObject parent)
    {
        if (parent != null)
        {
            CacheChildrenPositions(parent);
            Undo.RecordObject(parent.transform, "Set Parent Position");
            parent.transform.position = selectedVertexPosition;
            RepositionChildren(parent);
        }
        CancelVertexSelection();
    }

    private static void CacheChildrenPositions(GameObject parent)
    {
        cachedChildrenPositions.Clear();
        foreach (Transform child in parent.transform)
        {
            cachedChildrenPositions.Add(child.position);
        }
    }

    private static void RepositionChildren(GameObject parent)
    {
        int childIndex = 0;
        foreach (Transform child in parent.transform)
        {
            if (childIndex < cachedChildrenPositions.Count)
            {
                Undo.RecordObject(child, "Reposition Child");
                child.position = cachedChildrenPositions[childIndex];
                childIndex++;
            }
        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.V)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out MeshFilter meshFilter))
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    Vector3[] vertices = mesh.vertices;
                    int closestVertexIndex = GetClosestVertexIndex(vertices, hit);

                    if (closestVertexIndex != -1)
                    {
                        selectedVertexPosition = hit.transform.TransformPoint(vertices[closestVertexIndex]);
                        SceneView.RepaintAll();
                        e.Use();
                    }
                }
            }
        }

        Handles.color = Color.red;
        Handles.SphereHandleCap(0, selectedVertexPosition, Quaternion.identity, 0.1f, EventType.Repaint);
    }

    private static int GetClosestVertexIndex(Vector3[] vertices, RaycastHit hit)
    {
        float minDistance = float.MaxValue;
        int closestVertexIndex = -1;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldVertex = hit.transform.TransformPoint(vertices[i]);
            float distance = Vector3.Distance(worldVertex, hit.point);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestVertexIndex = i;
            }
        }

        return closestVertexIndex;
    }
}
