using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ScriptableObjectTable : EditorWindow
{
    private enum SortOrder
    {
        Original,
        Ascending,
        Descending
    }
    
    private Type selectedType;
    private List<ScriptableObject> instances = new List<ScriptableObject>();
    private Vector2 scrollPosition;
    private List<float> columnWidths = new List<float>();
    private const float minColumnWidth = 50f;
    private const float nameColumnWidth = 150f;
    private bool isResizing = false;
    private int resizingColumnIndex = -1;
    private int sortColumnIndex = -1;
    private SortOrder currentSortOrder = SortOrder.Original;
    private List<ScriptableObject> originalOrder;
    private List<string> propertyPaths = new List<string>();

    private Dictionary<Type, bool> typeCache = new Dictionary<Type, bool>();
    private HashSet<Type> processedTypes = new HashSet<Type>();

    [MenuItem("Tools/Scriptable Object Table")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectTable>("Scriptable Object Table");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scriptable Object Table", EditorStyles.boldLabel);

        // Dropdown to select ScriptableObject type
        if (EditorGUILayout.DropdownButton(new GUIContent(selectedType?.Name ?? "Select ScriptableObject Type"), FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();
            var types = GetAllScriptableObjectDerivedTypes();
            foreach (var type in types)
            {
                menu.AddItem(new GUIContent(type.Name), selectedType == type, OnTypeSelected, type);
            }
            menu.ShowAsContext();
        }

        if (selectedType != null)
        {
            // Refresh instances button
            if (GUILayout.Button("Refresh Instances"))
            {
                RefreshInstances();
            }

            if (instances.Count > 0)
            {
                // Get all property paths
                if (propertyPaths.Count == 0)
                {
                    GetAllPropertyPaths(instances[0]);
                }

                // Initialize column widths if needed
                if (columnWidths.Count != propertyPaths.Count + 1)
                {
                    columnWidths.Clear();
                    columnWidths.Add(nameColumnWidth);
                    for (int i = 0; i < propertyPaths.Count; i++)
                    {
                        columnWidths.Add(200f);
                    }
                }

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                // Draw header row
                DrawHeaderRow();

                // Sort instances if necessary
                List<ScriptableObject> sortedInstances = SortInstances(instances);

                // Draw instance rows
                foreach (var instance in sortedInstances)
                {
                    DrawInstanceRow(instance);
                }

                EditorGUILayout.EndScrollView();

                HandleColumnResize();
            }
            else
            {
                EditorGUILayout.LabelField($"No instances found for {selectedType.Name} or its derived types.");
            }
        }
    }

    private void DrawHeaderRow()
    {
        EditorGUILayout.BeginHorizontal();
        
        for (int i = 0; i < propertyPaths.Count + 1; i++)
        {
            Rect cellRect = EditorGUILayout.GetControlRect(GUILayout.Width(columnWidths[i]));
            
            if (DrawSortableHeader(cellRect, i == 0 ? "Name" : propertyPaths[i - 1], i))
            {
                SortByColumn(i);
            }

            if (i < propertyPaths.Count)
            {
                DrawResizeHandle(cellRect, i);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawInstanceRow(ScriptableObject instance)
    {
        EditorGUILayout.BeginHorizontal();

        // Instance name
        EditorGUILayout.ObjectField(instance, selectedType, false, GUILayout.Width(columnWidths[0]));

        // Fields
        SerializedObject serializedObject = new SerializedObject(instance);
        for (int i = 0; i < propertyPaths.Count; i++)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyPaths[i]);
            if (property != null && !IsHeaderProperty(property))
            {
                Rect cellRect = EditorGUILayout.GetControlRect(GUILayout.Width(columnWidths[i + 1]));
                DrawPropertyField(cellRect, property);
            }
            else
            {
                // Draw an empty space for headers
                GUILayout.Space(columnWidths[i + 1]);
            }
        }

        // Apply changes
        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(instance);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPropertyField(Rect position, SerializedProperty property)
    {
        // Calculate the indent level based on the property path
        int indentLevel = property.propertyPath.Split('.').Length - 1;

        // Store the original indent level
        int originalIndent = EditorGUI.indentLevel;

        try
        {
            // Set the indent level to 0 to align all fields
            EditorGUI.indentLevel = 0;

            // Adjust the position to remove the default indentation
            position.x -= indentLevel * 15;
            position.width += indentLevel * 15;

            if (property.propertyType == SerializedPropertyType.Generic && property.hasVisibleChildren)
            {
                DrawCustomClassInline(position, property);
            }
            else if (property.propertyType == SerializedPropertyType.Vector2 ||
                     property.propertyType == SerializedPropertyType.Vector3 ||
                     property.propertyType == SerializedPropertyType.Vector4)
            {
                DrawVectorHorizontally(position, property);
            }
            else
            {
                // Draw other property types normally
                EditorGUI.PropertyField(position, property, GUIContent.none, false);
            }
        }
        finally
        {
            // Restore the original indent level
            EditorGUI.indentLevel = originalIndent;
        }
    }

    private void DrawCustomClassInline(Rect position, SerializedProperty property)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        var childProperties = new List<SerializedProperty>();
        var childProperty = property.Copy();
        var endProperty = property.GetEndProperty();
        bool enterChildren = true;
        while (childProperty.NextVisible(enterChildren) && !SerializedProperty.EqualContents(childProperty, endProperty))
        {
            childProperties.Add(childProperty.Copy());
            enterChildren = false;
        }

        float labelWidth = 50f; // Adjust this value to fit your labels
        float fieldWidth = (position.width - labelWidth * childProperties.Count) / childProperties.Count;
        float totalWidth = labelWidth + fieldWidth;

        for (int i = 0; i < childProperties.Count; i++)
        {
            Rect labelRect = new Rect(position.x + i * totalWidth, position.y, labelWidth, position.height);
            Rect fieldRect = new Rect(labelRect.xMax, position.y, fieldWidth, position.height);

            // Draw label
            EditorGUI.LabelField(labelRect, childProperties[i].displayName + ":");

            // Draw field
            if (childProperties[i].propertyType == SerializedPropertyType.Vector2 ||
                childProperties[i].propertyType == SerializedPropertyType.Vector3 ||
                childProperties[i].propertyType == SerializedPropertyType.Vector4)
            {
                DrawVectorHorizontally(fieldRect, childProperties[i]);
            }
            else
            {
                EditorGUI.PropertyField(fieldRect, childProperties[i], GUIContent.none, false);
            }
        }

        EditorGUI.EndProperty();
    }

    private void DrawVectorHorizontally(Rect position, SerializedProperty property)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        int componentCount = property.propertyType == SerializedPropertyType.Vector4 ? 4 :
                             property.propertyType == SerializedPropertyType.Vector3 ? 3 : 2;

        float componentWidth = position.width / componentCount;
        float labelWidth = 15f; // Width for x, y, z, w labels
        float fieldWidth = componentWidth - labelWidth;

        for (int i = 0; i < componentCount; i++)
        {
            Rect labelRect = new Rect(position.x + i * componentWidth, position.y, labelWidth, position.height);
            Rect fieldRect = new Rect(labelRect.xMax, position.y, fieldWidth, position.height);

            // Draw component label (x, y, z, w)
            EditorGUI.LabelField(labelRect, GetVectorComponentName(i) + ":");

            // Draw component field
            EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative(GetVectorComponentName(i)), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    private string GetVectorComponentName(int index)
    {
        switch (index)
        {
            case 0: return "x";
            case 1: return "y";
            case 2: return "z";
            case 3: return "w";
            default: return "";
        }
    }

    private void DrawResizeHandle(Rect cellRect, int columnIndex)
    {
        Rect resizeHandleRect = new Rect(cellRect.xMax - 5f, cellRect.y, 10f, cellRect.height);
        EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);

        if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
        {
            isResizing = true;
            resizingColumnIndex = columnIndex;
        }
    }

    private void HandleColumnResize()
    {
        if (isResizing)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                isResizing = false;
                resizingColumnIndex = -1;
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                float mouseDelta = Event.current.delta.x;
                columnWidths[resizingColumnIndex] = Mathf.Max(columnWidths[resizingColumnIndex] + mouseDelta, minColumnWidth);
                columnWidths[resizingColumnIndex + 1] = Mathf.Max(columnWidths[resizingColumnIndex + 1] - mouseDelta, minColumnWidth);
                Repaint();
            }
        }
    }

    private void OnTypeSelected(object typeObj)
    {
        Type type = (Type)typeObj;
        if (typeof(ScriptableObject).IsAssignableFrom(type))
        {
            selectedType = type;
            RefreshInstances();
        }
        else
        {
            Debug.LogError($"Selected type {type.Name} is not derived from ScriptableObject.");
        }
    }

    private void RefreshInstances()
    {
        if (selectedType != null)
        {
            instances = AssetDatabase.FindAssets($"t:{selectedType.Name}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(asset => asset != null && selectedType.IsAssignableFrom(asset.GetType()))
                .ToList();
            
            propertyPaths.Clear();
        }
        else
        {
            instances.Clear();
            propertyPaths.Clear();
        }
    }

    private Type[] GetAllScriptableObjectDerivedTypes()
    {
        typeCache.Clear();
        processedTypes.Clear();

        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ScriptableObject).IsAssignableFrom(type) && !type.IsAbstract)
            .ToList();

        foreach (var type in allTypes)
        {
            HasInstantiableDescendants(type, allTypes);
        }

        return allTypes.Where(type => typeCache[type]).ToArray();
    }

    private bool HasInstantiableDescendants(Type type, List<Type> allTypes)
    {
        if (typeCache.TryGetValue(type, out bool result))
        {
            return result;
        }

        if (processedTypes.Contains(type))
        {
            return false;
        }

        processedTypes.Add(type);

        if (type.GetCustomAttribute<CreateAssetMenuAttribute>() != null)
        {
            typeCache[type] = true;
            return true;
        }

        bool hasInstantiableDescendants = allTypes
            .Where(t => t != type && type.IsAssignableFrom(t))
            .Any(t => HasInstantiableDescendants(t, allTypes));

        typeCache[type] = hasInstantiableDescendants;
        return hasInstantiableDescendants;
    }

    private List<ScriptableObject> SortInstances(List<ScriptableObject> instances)
    {
        if (instances.Count == 0 || sortColumnIndex == -1 || currentSortOrder == SortOrder.Original)
        {
            return instances;
        }

        return instances.OrderBy(instance =>
        {
            if (sortColumnIndex == 0)
            {
                return instance.name;
            }
            else if (sortColumnIndex - 1 < propertyPaths.Count)
            {
                SerializedObject serializedObject = new SerializedObject(instance);
                SerializedProperty property = serializedObject.FindProperty(propertyPaths[sortColumnIndex - 1]);
                return property != null ? GetPropertyValue(property) : null;
            }
            return null;
        }, currentSortOrder == SortOrder.Ascending ? Comparer<object>.Default : Comparer<object>.Create((a, b) => Comparer<object>.Default.Compare(b, a))).ToList();
    }

    private object GetPropertyValue(SerializedProperty property)
    {
        if (property == null) return null;

        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue ? property.objectReferenceValue.name : "";
            case SerializedPropertyType.Enum:
                return property.enumValueIndex;
            // Add more cases for other property types as needed
            default:
                return property.displayName;
        }
    }

    private bool DrawSortableHeader(Rect rect, string label, int columnIndex)
    {
        // Draw the header background
        EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.8f, 0.8f, 0.8f));

        // Create a button that covers the entire header cell
        bool clicked = GUI.Button(rect, "", GUIStyle.none);

        // Draw the label
        Rect labelRect = new Rect(rect.x + 4, rect.y, rect.width - 20, rect.height);
        GUI.Label(labelRect, label, EditorStyles.boldLabel);

        // Draw the sort indicator
        if (sortColumnIndex == columnIndex && currentSortOrder != SortOrder.Original)
        {
            Rect arrowRect = new Rect(rect.x + rect.width - 16, rect.y + 2, 16, rect.height);
            GUI.Label(arrowRect, currentSortOrder == SortOrder.Ascending ? "▲" : "▼", EditorStyles.boldLabel);
        }

        return clicked;
    }

    private void SortByColumn(int columnIndex)
    {
        if (sortColumnIndex == columnIndex)
        {
            // Cycle through sort orders: Ascending -> Descending -> Original
            currentSortOrder = (SortOrder)(((int)currentSortOrder + 1) % 3);
        }
        else
        {
            sortColumnIndex = columnIndex;
            currentSortOrder = SortOrder.Ascending;
        }

        if (currentSortOrder == SortOrder.Original && originalOrder == null)
        {
            originalOrder = new List<ScriptableObject>(instances);
        }

        Repaint();
    }

    private void GetAllPropertyPaths(ScriptableObject instance)
    {
        propertyPaths.Clear();
        SerializedObject serializedObject = new SerializedObject(instance);
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            if (property.propertyPath != "m_Script")
            {
                propertyPaths.Add(property.propertyPath);
            }
            enterChildren = false;
        }
    }

     private bool IsHeaderProperty(SerializedProperty property)
    {
        var field = GetFieldInfoFromProperty(property);
        return field != null && Attribute.IsDefined(field, typeof(HeaderAttribute));
    }

    private FieldInfo GetFieldInfoFromProperty(SerializedProperty property)
    {
        var parentType = property.serializedObject.targetObject.GetType();
        var fieldInfo = parentType.GetField(property.propertyPath, 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (fieldInfo == null && property.propertyPath.Contains("."))
        {
            var paths = property.propertyPath.Split('.');
            for (int i = 0; i < paths.Length - 1; i++)
            {
                var path = string.Join(".", paths.Take(i + 1));
                var parentField = parentType.GetField(path, 
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                
                if (parentField != null)
                {
                    parentType = parentField.FieldType;
                }
            }

            fieldInfo = parentType.GetField(paths.Last(), 
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        return fieldInfo;
    }
}