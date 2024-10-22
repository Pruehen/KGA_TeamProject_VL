using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System; // Add this line

public static class LevelDesignTool
{
    private static GameObject objectToSpan;
    private static int numberOfObjects = 5;
    private static float spacing = 1f;
    private enum SpanAxis { X, Y, Z }
    private static SpanAxis selectedAxis = SpanAxis.X;
    private static SpanAxis selectedSubAxisForAutoSpan = SpanAxis.X;


    private static bool isPreparingMove = false;
    private static Vector3 originalParentPosition;
    private static List<Vector3> childrenOriginalPositions = new List<Vector3>();

    private static bool isSelectingVertex = false;
    private static Vector3 selectedVertexPosition;
    private static List<Vector3> cachedChildrenPositions = new List<Vector3>();

    private static Dictionary<GameObject, bool> originalSelectableStates = new Dictionary<GameObject, bool>();
    private static Dictionary<GameObject, bool> originalActiveStates = new Dictionary<GameObject, bool>();
    private static bool isOnlySelectedSelectable = false;
    private static bool isOnlySelectedActive = false;
    private static bool isCustomVertexSnappingEnabled = false;
    private static Vector3 lastSnappedPosition;

    private static bool isVKeyPressed = false;

    private static Vector3 vertexLocalOffsetFromSelected = Vector3.zero;

    private static Vector3 _initailPrefebOffset = new Vector3(0, 90, 0);

    private static Transform pivot = null;
    private static float radius = 5f;
    private static float wallHeight = 2f;
    private static int segments = 32;

    [MenuItem("Tools/Level Design/Object Spanning Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<LevelDesignToolWindow>("Object Spanning Tool");
    }

    public static void OnGUI()
    {
        GUILayout.Label("Object Spanning Tool", EditorStyles.boldLabel);

        // Use the currently selected object in the hierarchy
        objectToSpan = Selection.activeGameObject;
        EditorGUILayout.ObjectField("Selected Object", objectToSpan, typeof(GameObject), true);

        numberOfObjects = EditorGUILayout.IntField("Number of Objects", numberOfObjects);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        selectedAxis = (SpanAxis)EditorGUILayout.EnumPopup("Span Axis", selectedAxis);

        EditorGUI.BeginDisabledGroup(objectToSpan == null);
        if (GUILayout.Button("Span Objects"))
        {
            SpanObjects();
        }
        EditorGUI.EndDisabledGroup();

        if (objectToSpan == null)
        {
            EditorGUILayout.HelpBox("Please select an object in the hierarchy to use the tools.", MessageType.Info);
        }

        GUILayout.Space(10);
        GUILayout.Label("Vertex-Based Parent Positioning", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(objectToSpan == null);
        if (!VertexSelectionHandler.IsSelectingVertex)
        {
            if (GUILayout.Button("Select Vertex for Parent Position"))
            {
                VertexSelectionHandler.StartVertexSelection();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Press V and click on a vertex in the Scene view. Then click 'Set Parent Position'.", MessageType.Info);
            if (GUILayout.Button("Set Parent Position"))
            {
                VertexSelectionHandler.SetParentPosition(objectToSpan);
            }
            if (GUILayout.Button("Cancel Vertex Selection"))
            {
                VertexSelectionHandler.CancelVertexSelection();
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);
        isCustomVertexSnappingEnabled = GUILayout.Toggle(isCustomVertexSnappingEnabled, "Enable Custom Vertex Snapping");

        if (isCustomVertexSnappingEnabled)
        {
            EditorGUILayout.HelpBox("Hold V and click to snap to vertices of any object, regardless of selectability.", MessageType.Info);
        }

        if (GUILayout.Button(isOnlySelectedSelectable ? "Enable All Selections" : "Make Only Selected Selectable"))
        {
            ToggleOnlySelectedSelectable();
        }
        if (GUILayout.Button(isOnlySelectedActive ? "Enable All Activations" : "Make Only Selected Active"))
        {
            ToggleOnlySelectedActive();
        }

        GUILayout.Space(10);
        GUILayout.Label("Rotation Tool", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rotate 90° CCW"))
        {
            Rotate90Degrees(false);
        }
        if (GUILayout.Button("Rotate 90° CW"))
        {
            Rotate90Degrees(true);
        }
        EditorGUILayout.EndHorizontal();


        GUILayout.Space(10);
        GUILayout.Label("Automatic Spanning Tool", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(objectToSpan == null);
        if (!VertexSelectionHandler.IsSelectingVertex)
        {
            if (GUILayout.Button("Select Vertex for Auto Span"))
            {
                VertexSelectionHandler.StartVertexSelection();
            }
            if (GUILayout.Button("Auto Span Next"))
            {
                AutoSpanNext();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Press V and click on a vertex in the Scene view. Then click 'Auto Span Next'.", MessageType.Info);

            if (GUILayout.Button("Cancel Vertex Selection"))
            {
                VertexSelectionHandler.CancelVertexSelection();
                selectedVertexPosition = VertexSelectionHandler.GetSelectedVertexPosition();
                vertexLocalOffsetFromSelected = objectToSpan.transform.InverseTransformVector(selectedVertexPosition - objectToSpan.transform.position);
            }
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10);
        GUILayout.Label("Radial Wall Creator", EditorStyles.boldLabel);

        pivot = (Transform)EditorGUILayout.ObjectField("Pivot", pivot, typeof(Transform), true);
        radius = EditorGUILayout.FloatField("Radius", radius);
        wallHeight = EditorGUILayout.FloatField("Wall Height", wallHeight);
        segments = EditorGUILayout.IntSlider("Segments", segments, 8, 2048);

        if (GUILayout.Button("Create Radial Wall"))
        {
            CreateRadialWall();
        }
    }

    private static void AutoSpanNext()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length != 1)
        {
            Debug.LogError("Please select one object to span.");
            return;
        }
        DeselectAll();

        objectToSpan = selectedObjects[0];

        Vector3 currentSpanDirection = objectToSpan.transform.TransformVector(vertexLocalOffsetFromSelected);
        currentSpanDirection.Normalize();

        Vector3 nextSpanPosition = objectToSpan.transform.position + currentSpanDirection * 4f;

        Vector3 currentSpanPosition = objectToSpan.transform.position + currentSpanDirection * 2f;
        Debug.DrawRay(currentSpanPosition, currentSpanDirection, Color.red, 10f);
        Vector3 initalRayRotationOffset = new Vector3(0, -170, 0);
        Quaternion initalRayRotation = Quaternion.LookRotation(currentSpanDirection, Vector3.up) * Quaternion.Euler(initalRayRotationOffset);
        
        Quaternion prefabSpawnRotation = objectToSpan.transform.rotation;
        prefabSpawnRotation *= Quaternion.Euler(_initailPrefebOffset);
        
        Vector3 hitPosition = Vector3.zero;
        if(!TryDetectWallPoints(out hitPosition, 36, 2f, nextSpanPosition, initalRayRotation, 1 << LayerMask.NameToLayer("Environment")))
        {
            Debug.Log("Wall not found");
            return;
        }

        // Check if the selected object is a prefab instance
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(objectToSpan);
        
        GameObject newObject;
        Vector3 targetDirection = nextSpanPosition - objectToSpan.transform.position;
        
        prefabSpawnRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        prefabSpawnRotation *= Quaternion.Euler(_initailPrefebOffset);
        newObject = InstantiatePrefabOrObject(prefab, currentSpanPosition, prefabSpawnRotation,
            Vector3.one, objectToSpan.transform.parent, $"{objectToSpan.name}_Span");

        Undo.RegisterCreatedObjectUndo(newObject, "Span Object");
        SelectObject(newObject);
        objectToSpan = newObject;

        SceneView.RepaintAll();
    }
    private static Vector3 GetAutoSpanDirection(GameObject obj)
    {
        Vector3 spanDirection = GetSpanDirection(obj);
        spanDirection.Normalize();
        return spanDirection;
    }
    private static bool TryDetectWallPoints(out Vector3 result,int rayCount, float maxDistance, 
    Vector3 position, Quaternion rotation, LayerMask wallLayer)
    {
        result = Vector3.zero;
        // Perform radial raycasting in local space
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the local angle for this ray
            float angle = (i / (float)rayCount) * 360.0f;
            Vector3 localDirection = AngleToVector3(angle); // Local space direction

            // Convert the local direction to world space using hinge's rotation
            Vector3 worldDirection = Quaternion.Euler(_initailPrefebOffset) * rotation * localDirection;
            Vector3 rayDirection = Quaternion.Euler(0, 90, 0) * worldDirection;
            Vector3 rayStart = position + worldDirection * 2f;
            // Cast a ray from the hinge center in the world direction
            Ray ray = new Ray(rayStart, rayDirection);
            RaycastHit hit;

            Debug.DrawRay(rayStart, rayDirection, new Color(1f - (float)i / (float)rayCount, 0, 0, 1f), maxDistance);
            if (Physics.Raycast(ray, out hit, maxDistance, wallLayer))
            {
                // Handle collision, e.g., stop rotation or adjust behavior
                Debug.Log("Hit wall at " + hit.point);
                Debug.DrawRay(rayStart, rayDirection, Color.blue, maxDistance);
                result = hit.point;
            }
            else
            {
            }
        }
        Debug.Log("No wall found");
        if(result != Vector3.zero)
        {
            return true;
        }
        return false;
    }
    // Helper function to convert angle to a local Vector3 direction
    private static Vector3 AngleToVector3(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian)); // Local space direction
    }

    private static void DeselectAll()
    {
        Selection.objects = new UnityEngine.Object[0];
    }
    private static void SelectObject(UnityEngine.Object obj)
    {
        Selection.objects = new UnityEngine.Object[] { obj };
    }
    private static void SelectObjects(UnityEngine.Object[] objs)
    {
        Selection.objects = objs;
    }

    private static void SpanObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0)
        {
            Debug.LogError("Please select at least one object to span.");
            return;
        }

        foreach (GameObject objectToSpan in selectedObjects)
        {
            Vector3 spanDirection = GetSpanDirection(objectToSpan);

            // Check if the selected object is a prefab instance
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(objectToSpan);
            
            for (int i = 1; i < numberOfObjects; i++) // Start from 1 to exclude the original object
            {
                Vector3 position = objectToSpan.transform.position + spanDirection * spacing * i;
                GameObject newObject;

                newObject = InstantiatePrefabOrObject(prefab, position, objectToSpan.transform.rotation,
                 Vector3.one, objectToSpan.transform.parent, $"{objectToSpan.name}_Span{i + 1}");

                Undo.RegisterCreatedObjectUndo(newObject, "Span Object");
            }
        }

        SceneView.RepaintAll();
    }

    private static GameObject InstantiatePrefabOrObject(GameObject prefab, Vector3 position, Quaternion rotation,
     Vector3 scale, Transform parent, string name)
    {
        GameObject newObject;
        if (prefab != null)
        {
            // If it's a prefab instance, instantiate from the prefab
            newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;
            newObject.transform.localScale = scale;
        }
        else
        {
            // If it's not a prefab, instantiate normally
            newObject = UnityEngine.Object.Instantiate(objectToSpan, position, rotation);
        }
        // Set the parent of the new object
        newObject.transform.SetParent(parent);

        // Rename the new object
        newObject.name = name;

        return newObject;
    }

    private static Vector3 GetSpanDirection(GameObject obj)
    {
        switch (selectedAxis)
        {
            case SpanAxis.X:
                return obj.transform.right;
            case SpanAxis.Y:
                return obj.transform.up;
            case SpanAxis.Z:
                return obj.transform.forward;
            default:
                return Vector3.right;
        }
    }
    private static Vector3 GetSubSpanDirectionForAutoSpan(GameObject obj)
    {
        switch (selectedSubAxisForAutoSpan)
        {
            case SpanAxis.X:
                return obj.transform.right;
            case SpanAxis.Y:
                return obj.transform.up;
            case SpanAxis.Z:
                return obj.transform.forward;
            default:
                return Vector3.right;
        }
    }

    private static void PrepareParentMove()
    {
        if (objectToSpan == null || objectToSpan.transform.childCount == 0)
        {
            Debug.LogError("Please select a parent object with children.");
            return;
        }

        isPreparingMove = true;
        originalParentPosition = objectToSpan.transform.position;
        childrenOriginalPositions.Clear();

        foreach (Transform child in objectToSpan.transform)
        {
            childrenOriginalPositions.Add(child.position);
        }

        Debug.Log("Parent move prepared. Move the parent object in the Scene view.");
    }

    private static void CancelParentMove()
    {
        if (!isPreparingMove || objectToSpan == null)
        {
            Debug.LogError("No parent move in progress.");
            return;
        }

        isPreparingMove = false;
        childrenOriginalPositions.Clear();
        Debug.Log("Parent move cancelled.");
    }

    private static void ToggleOnlySelectedSelectable()
    {
        if (!isOnlySelectedSelectable)
        {
            MakeOnlySelectedSelectable();
        }
        else
        {
            RestoreSelectableStates();
        }
        isOnlySelectedSelectable = !isOnlySelectedSelectable;
    }
    private static void ToggleOnlySelectedActive()
    {
        if (!isOnlySelectedActive)
        {
            MakeOnlySelectedActive();
        }
        else
        {
            RestoreActiveStates();
        }
        isOnlySelectedActive = !isOnlySelectedActive;
    }

    private static void MakeOnlySelectedActive()
    {
        originalActiveStates.Clear();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        HashSet<GameObject> selectedObjects = new HashSet<GameObject>(Selection.gameObjects);
        HashSet<GameObject> selectedObjectsWithParentsAndChildren = new HashSet<GameObject>();
        foreach(GameObject obj in selectedObjects)
        {
            selectedObjectsWithParentsAndChildren.Add(obj);
            GameObject currentObj = obj;
            while(true)
            {
                if(currentObj.transform.parent == null)
                {
                    break;
                }
                selectedObjectsWithParentsAndChildren.Add(currentObj.transform.parent.gameObject);
                currentObj = currentObj.transform.parent.gameObject;
            }
            GetAllChildren(obj, selectedObjectsWithParentsAndChildren);
        }

        foreach(GameObject obj in allObjects)
        {
            originalActiveStates[obj] = obj.activeSelf;
        }
        foreach(GameObject obj in allObjects)
        {
            obj.SetActive(false);
        }
        foreach(GameObject obj in selectedObjectsWithParentsAndChildren)
        {
            obj.SetActive(true);
        }

        // Force UI update
        ForceUIUpdate();
    }
    private static void GetAllChildren(GameObject obj, HashSet<GameObject> result)
    {
        foreach (Transform child in obj.transform)
        {
            result.Add(child.gameObject);
            GetAllChildren(child.gameObject, result);
        }
    }

    private static void RestoreActiveStates()
    {
        foreach (var kvp in originalActiveStates)
        {
            if (kvp.Key != null)
            {
                kvp.Key.SetActive(kvp.Value);
            }
        }
        originalActiveStates.Clear();

        // Force UI update
        ForceUIUpdate();
    }

    private static void MakeOnlySelectedSelectable()
    {
        originalSelectableStates.Clear();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        HashSet<GameObject> selectedObjects = new HashSet<GameObject>(Selection.gameObjects);

        foreach(GameObject obj in allObjects)
        {
            originalSelectableStates[obj] = !SceneVisibilityManager.instance.IsPickingDisabled(obj);
        }
        SceneVisibilityManager.instance.DisableAllPicking();
        foreach(GameObject obj in selectedObjects)
        {
            SceneVisibilityManager.instance.EnablePicking(obj, true);
        }

        // Force UI update
        ForceUIUpdate();
    }


    private static void RestoreSelectableStates()
    {
        SceneVisibilityManager.instance.DisableAllPicking();

        foreach (var kvp in originalSelectableStates)
        {
            if (kvp.Key != null)
            {
                if(kvp.Value)
                {
                    SceneVisibilityManager.instance.EnablePicking(kvp.Key, false);
                }
            }
        }
        originalSelectableStates.Clear();

        // Force UI update
        ForceUIUpdate();
    }

    private static void ForceUIUpdate()
    {
        // Repaint all views
        SceneView.RepaintAll();
        EditorApplication.RepaintHierarchyWindow();
        EditorApplication.RepaintProjectWindow();

        // Force a layout update
        EditorUtility.SetDirty(Selection.activeGameObject);

        // Schedule another repaint on the next editor update
        EditorApplication.delayCall += () =>
        {
            SceneView.RepaintAll();
            EditorApplication.RepaintHierarchyWindow();
            EditorApplication.RepaintProjectWindow();
        };
    }

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        if (!isCustomVertexSnappingEnabled) return;

        Event e = Event.current;

        // Check for V key press and release
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.V)
        {
            isVKeyPressed = true;
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else if (e.type == EventType.KeyUp && e.keyCode == KeyCode.V)
        {
            isVKeyPressed = false;
        }

        // Check for left mouse click while V is pressed
        if (e.type == EventType.MouseDown && e.button == 0 && isVKeyPressed)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MeshFilter meshFilter = hit.collider.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Vector3 closestVertex = FindClosestVertex(meshFilter, hit.point);
                    lastSnappedPosition = closestVertex;

                    // If an object is selected, move it to the snapped position
                    if (Selection.activeGameObject != null)
                    {
                        Undo.RecordObject(Selection.activeGameObject.transform, "Snap to Vertex");
                        Selection.activeGameObject.transform.position = lastSnappedPosition;
                    }

                    e.Use();
                    sceneView.Repaint();
                }
            }
        }

        // Draw a sphere at the last snapped position
        Handles.color = Color.green;
        Handles.SphereHandleCap(0, lastSnappedPosition, Quaternion.identity, 0.1f, EventType.Repaint);
    }

    private static Vector3 FindClosestVertex(MeshFilter meshFilter, Vector3 hitPoint)
    {
        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        Vector3 closestVertex = meshFilter.transform.TransformPoint(vertices[0]);
        float closestDistance = Vector3.Distance(hitPoint, closestVertex);

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 worldVertex = meshFilter.transform.TransformPoint(vertices[i]);
            float distance = Vector3.Distance(hitPoint, worldVertex);
            if (distance < closestDistance)
            {
                closestVertex = worldVertex;
                closestDistance = distance;
            }
        }

        return closestVertex;
    }

    private static void Rotate90Degrees(bool clockwise)
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length == 0) return;

        Undo.RecordObjects(selectedObjects.Select(obj => obj.transform).ToArray(), "Rotate 90 Degrees");

        float angle = clockwise ? -90f : 90f;
        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.Rotate(Vector3.up, angle, Space.World);
        }

        ForceUIUpdate();
    }

    private static void CreateRadialWall()
    {
        GameObject wallParent = pivot.gameObject;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * 360f / segments;
            CreateWallSegment(wallParent.transform, angle);
        }
    }

    private static void CreateWallSegment(Transform parent, float angle)
    {
        GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(objectToSpan);
        GameObject segment = InstantiatePrefabOrObject(prefab, parent.position, parent.rotation, Vector3.one, parent, $"Wall Segment {angle}");
        segment.transform.SetParent(parent);

        Vector3 position = Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
        segment.transform.position = position + parent.position;
        segment.transform.LookAt(parent.position);

        Undo.RegisterCreatedObjectUndo(segment, "Create Wall Segment");
    }
}

public class LevelDesignToolWindow : EditorWindow
{
    void OnGUI() => LevelDesignTool.OnGUI();
    void OnDestroy() => VertexSelectionHandler.CancelVertexSelection();
}
