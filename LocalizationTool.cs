using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LocalizationTool : EditorWindow
{
    private Dictionary<string, string> localizationData = new Dictionary<string, string>();
    private string language = "en";
    private string filePath = "Assets/Localization.csv";

    [MenuItem("Tools/Localization Tool")]
    public static void OpenWindow()
    {
        GetWindow<LocalizationTool>("Localization Tool");
    }

    private void OnGUI()
    {
        language = EditorGUILayout.TextField("Language Code", language);
        filePath = EditorGUILayout.TextField("File Path", filePath);

        if (GUILayout.Button("Import CSV"))
        {
            ImportCSV();
        }

        if (GUILayout.Button("Export CSV"))
        {
            ExportCSV();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Localization Data", EditorStyles.boldLabel);

        foreach (var key in new List<string>(localizationData.Keys))
        {
            EditorGUILayout.BeginHorizontal();
            localizationData[key] = EditorGUILayout.TextField(key, localizationData[key]);
            if (GUILayout.Button("Remove"))
            {
                localizationData.Remove(key);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add New Entry"))
        {
            localizationData.Add("NewKey", "");
        }
    }

    private void ImportCSV()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        localizationData.Clear();
        string[] lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var split = line.Split(',');
            if (split.Length == 2)
            {
                localizationData[split[0]] = split[1];
            }
        }
    }

    private void ExportCSV()
    {
        var lines = new List<string>();
        foreach (var pair in localizationData)
        {
            lines.Add($"{pair.Key},{pair.Value}");
        }
        File.WriteAllLines(filePath, lines);
        AssetDatabase.Refresh();
    }
}
