using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor : EditorWindow
{
    private string newKey = "";
    private string newValue = "";
    private string selectedKey = "";
    private string selectedValue = "";

    [MenuItem("Tools/PlayerPrefs Editor")]
    public static void OpenWindow()
    {
        GetWindow<PlayerPrefsEditor>("PlayerPrefs Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("PlayerPrefs Editor", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Entry", EditorStyles.boldLabel);
        newKey = EditorGUILayout.TextField("Key", newKey);
        newValue = EditorGUILayout.TextField("Value", newValue);

        if (GUILayout.Button("Add/Update Entry"))
        {
            if (!string.IsNullOrEmpty(newKey))
            {
                PlayerPrefs.SetString(newKey, newValue);
                PlayerPrefs.Save();
                newKey = "";
                newValue = "";
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Existing Entries", EditorStyles.boldLabel);

        foreach (var key in PlayerPrefsKeys())
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key, GUILayout.Width(200));
            if (GUILayout.Button("Edit"))
            {
                selectedKey = key;
                selectedValue = PlayerPrefs.GetString(key);
            }
            if (GUILayout.Button("Delete"))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
            EditorGUILayout.EndHorizontal();
        }

        if (!string.IsNullOrEmpty(selectedKey))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Edit Entry", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Editing Key: {selectedKey}");
            selectedValue = EditorGUILayout.TextField("Value", selectedValue);

            if (GUILayout.Button("Save Changes"))
            {
                PlayerPrefs.SetString(selectedKey, selectedValue);
                PlayerPrefs.Save();
                selectedKey = "";
                selectedValue = "";
            }

            if (GUILayout.Button("Cancel"))
            {
                selectedKey = "";
                selectedValue = "";
            }
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Clear All PlayerPrefs"))
        {
            if (EditorUtility.DisplayDialog("Confirm Clear", "Are you sure you want to clear all PlayerPrefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }
    }

    private string[] PlayerPrefsKeys()
    {
        var keys = new System.Collections.Generic.List<string>();
        var keysProperty = typeof(PlayerPrefs).GetMethod("GetAllKeys", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        if (keysProperty != null)
        {
            keys.AddRange((string[])keysProperty.Invoke(null, null));
        }
        return keys.ToArray();
    }
}
