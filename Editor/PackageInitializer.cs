using UnityEditor;
using UnityEngine;
using System.IO;

public static class PackageInitializer
{
    [MenuItem("Tools/Run Package Initializer")]
    public static void Run()
    {
        Debug.Log("🚀 Manual Run: PackageInitializer started");
        CreateFolderIfNotExists();
    }

    private static void CreateFolderIfNotExists()
    {
        string folderPath = "Assets/BackendEngin";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.Log($"⚡ Creating folder: {folderPath}");
            string guid = AssetDatabase.CreateFolder("Assets", "BackendEngin");
            if (!string.IsNullOrEmpty(guid))
                Debug.Log($"✅ Folder created: {folderPath}");
            else
            {
                Debug.LogError($"❌ Failed to create folder: {folderPath}");
                return;
            }
        }

        string editorSourcePath = "Packages/com.asoft.backendengine/Editor/BackendEngin";

        if (!Directory.Exists(editorSourcePath))
        {
            Debug.LogError($"❌ Source folder does NOT exist: {editorSourcePath}");
            return;
        }
        else
        {
            Debug.Log($"📂 Source folder found: {editorSourcePath}");
        }

        string[] files = Directory.GetFiles(editorSourcePath, "*.*", SearchOption.TopDirectoryOnly);
        Debug.Log($"📦 Found {files.Length} files in {editorSourcePath}");

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(folderPath, fileName);

            Debug.Log($"🔄 Processing file: {fileName}");

            if (File.Exists(destPath))
            {
                Debug.Log($"⚠️ File already exists: {destPath}, skipping...");
                continue;
            }

            try
            {
                FileUtil.CopyFileOrDirectory(file, destPath);
                Debug.Log($"✅ Copied {fileName} to {folderPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Failed to copy {fileName}: {ex.Message}");
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("✅ PackageInitializer completed!");
    }
}
