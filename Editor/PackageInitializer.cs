using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class PackageInitializer
{
    static PackageInitializer()
    {
        Debug.Log("🚀 PackageInitializer started");
        CreateFolderIfNotExists();
    }

    private static void CreateFolderIfNotExists()
    {
        string folderPath = "Assets/BackendEngin";

        // بررسی وجود فولدر
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.Log($"✅ Folder already exists: {folderPath}");
        }
        else
        {
            Debug.Log($"⚡ Folder does NOT exist, creating: {folderPath}");
            string guid = AssetDatabase.CreateFolder("Assets", "BackendEngin");
            if (!string.IsNullOrEmpty(guid))
            {
                Debug.Log($"✅ Folder created successfully: {folderPath}");
            }
            else
            {
                Debug.LogError($"❌ Failed to create folder: {folderPath}");
                return;
            }
        }

        string editorSourcePath = "Packages/com.asoft.backendengine/Editor/BackendEngin"; 

        // بررسی اینکه مسیر سورس وجود دارد یا نه
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
            string unitySourcePath = editorSourcePath + "/" + fileName;
            string unityDestPath = folderPath + "/" + fileName;

            Debug.Log($"🔄 Processing file: {fileName}");

            // بررسی اینکه فایل مقصد از قبل وجود دارد یا نه
            if (File.Exists(destPath))
            {
                Debug.Log($"⚠️ File already exists: {destPath}, skipping...");
                continue;
            }

            // استفاده از CopyAsset
            bool success = AssetDatabase.CopyAsset(unitySourcePath, unityDestPath);
            if (success)
            {
                Debug.Log($"✅ Copied {fileName} to {folderPath}");
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
