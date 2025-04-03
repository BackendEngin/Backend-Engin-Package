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

        // کپی تمام محتویات دایرکتوری
        CopyDirectoryContents(editorSourcePath, folderPath);

        AssetDatabase.Refresh();
        Debug.Log("✅ PackageInitializer completed!");
    }

    private static void CopyDirectoryContents(string sourceDir, string destDir)
    {
        // تمام فایل‌ها رو از sourceDir به destDir کپی می‌کنیم
        string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(destDir, fileName);

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
                        Debug.Log($"✅ Successfully copied {fileName} to {destDir}");
                        AssetDatabase.ImportAsset(destPath);  // Make sure the asset is imported properly
                    }
                    else
                    {
                        Debug.LogError($"❌ Failed to copy {fileName} to {destDir}");
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
    }
}
