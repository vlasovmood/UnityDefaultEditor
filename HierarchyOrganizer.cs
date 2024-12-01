using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class HierarchyOrganizer
{
    static HierarchyOrganizer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        Rect rect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
        if (obj.name.StartsWith("---")) // Group separator
        {
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUI.LabelField(rect, obj.name, EditorStyles.boldLabel);
        }
        else if (obj.CompareTag("Player")) // Tag-based coloring
        {
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.8f, 1f, 0.2f));
            EditorGUI.LabelField(rect, obj.name, EditorStyles.label);
        }
    }
}
