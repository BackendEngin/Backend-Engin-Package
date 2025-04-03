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
        Debug.Log($"Checking if folder exists at: {folderPath}");

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.Log($"⚡ Creating folder: {folderPath}");
            AssetDatabase.CreateFolder("Assets", "BackendEngin");
            Debug.Log($"✅ Folder created: {folderPath}");
        }

        string editorSourcePath = "Packages/com.asoft.backendengine/Editor/BackendEngin";
        Debug.Log($"Source folder path: {editorSourcePath}");

        if (!Directory.Exists(editorSourcePath))
        {
            Debug.LogError($"❌ Source folder does NOT exist: {editorSourcePath}");
            return;
        }

        string[] files = Directory.GetFiles(editorSourcePath, "*.*", SearchOption.TopDirectoryOnly);
        Debug.Log($"📦 Found {files.Length} files in {editorSourcePath}");

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(folderPath, fileName);

            Debug.Log($"🔄 Processing file: {fileName}");
            Debug.Log($"Source file: {file}");
            Debug.Log($"Destination path: {destPath}");

            if (File.Exists(file)) // Ensure the source file exists
            {
                try
                {
                    bool copySuccess = AssetDatabase.CopyAsset(file, destPath);
                    if (copySuccess)
                    {
                        Debug.Log($"✅ Successfully copied {fileName} to {folderPath}");
                        AssetDatabase.ImportAsset(destPath);  // Make sure the asset is imported properly
                    }
                    else
                    {
                        Debug.LogError($"❌ Failed to copy {fileName} to {folderPath}");
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ Error copying {fileName}: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"❌ Source file does not exist: {file}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ PackageInitializer completed!");
    }
}
