using UnityEditor;
using UnityEngine;
using System.IO;

public static class PackageInitializer
{
    [MenuItem("Tools/Run Package Initializer")]
    public static void Run()
    {
        Debug.Log("🚀 Running Package Initializer...");
        CreateFolderIfNotExists();
    }

    private static void CreateFolderIfNotExists()
    {
        string folderPath = "Assets/BackendEngin";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.Log($"⚡ Creating folder: {folderPath}");
            AssetDatabase.CreateFolder("Assets", "BackendEngin");
            Debug.Log($"✅ Folder created: {folderPath}");
        }

        string editorSourcePath = "Packages/com.asoft.backendengine/Editor/BackendEngin";

        if (!Directory.Exists(editorSourcePath))
        {
            Debug.LogError($"❌ Source folder does NOT exist: {editorSourcePath}");
            return;
        }

        string[] files = Directory.GetFiles(editorSourcePath, "*.*", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(folderPath, fileName);

            if (AssetDatabase.CopyAsset(file, destPath))
            {
                Debug.Log($"✅ Copied {fileName} to {folderPath}");
                AssetDatabase.ImportAsset(destPath);  // Make sure the asset is imported properly
            }
            else
            {
                Debug.LogError($"❌ Failed to copy {fileName} to {folderPath}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ PackageInitializer completed!");
    }
}