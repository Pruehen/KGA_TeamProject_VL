using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// Replace 'YourScriptableObject' with the name of your ScriptableObject class
[InitializeOnLoad]
public class DrawAttackArea : Editor
{
    static SO_AttackModule _am;
    static EnemyBase _eb;

    static DrawAttackArea()
    {
        // Called when Unity loads or scripts recompile
        SceneView.duringSceneGui += DrawMyGizmos;
    }

    private static void DrawMyGizmos(SceneView sceneView)
    {
        // draw some Gizmos-like gizmo
        object selected = SelectedManager.SelectedObject;
        if(selected == null)
        {
            return;
        }
        if (selected is SO_AttackModule am)
        {
            _am = am;
        }
        if (selected is GameObject go )
        {
            if(go == null)
            {
                return;
            }
            if(go.TryGetComponent(out EnemyBase eb))
            {
                _eb = eb;
            }
        }

        if (_am == null || _eb == null)
            return;

        Vector3 scale = _am.DamageBox.Range;
        Vector3 offset = _am.DamageBox.Offset;
        offset.Scale(_eb.DamageBox.transform.lossyScale);

        scale.Scale(_eb.DamageBox.transform.lossyScale);

        Handles.DrawWireCube(_eb.DamageBox.transform.position + offset, scale);
    }
}