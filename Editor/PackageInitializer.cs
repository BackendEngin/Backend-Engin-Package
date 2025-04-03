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

        // کپی کردن فولدرها و محتویات‌شان
        CopyDirectoryContents(editorSourcePath, folderPath);

        // رفرش کردن دیتابیس یونیتی
        AssetDatabase.Refresh();
        Debug.Log("✅ PackageInitializer completed!");
    }

    private static void CopyDirectoryContents(string sourceDir, string destDir)
    {
        // ابتدا بررسی می‌کنیم که دایرکتوری مقصد وجود داشته باشه یا نه
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
            Debug.Log($"✅ Created destination directory: {destDir}");
        }

        // گرفتن تمام فایل‌ها از دایرکتوری مبدا (شامل فایل‌ها و زیر دایرکتوری‌ها)
        string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string relativePath = file.Substring(sourceDir.Length + 1); // حذف مسیر پایه
            string destPath = Path.Combine(destDir, relativePath);

            string destFolder = Path.GetDirectoryName(destPath);
            // اطمینان از ایجاد پوشه‌های مورد نیاز در مقصد
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
                Debug.Log($"✅ Created folder: {destFolder}");
            }

            Debug.Log($"🔄 Copying {file} to {destPath}");

            // کپی کردن فایل‌ها به مقصد
            try
            {
                if (File.Exists(file))
                {
                    bool copySuccess = AssetDatabase.CopyAsset(file, destPath);
                    if (copySuccess)
                    {
                        Debug.Log($"✅ Successfully copied {file} to {destPath}");
                        AssetDatabase.ImportAsset(destPath);  // Make sure the asset is imported properly
                    }
                    else
                    {
                        Debug.LogError($"❌ Failed to copy {file} to {destPath}");
                    }
                }
                else
                {
                    Debug.LogError($"❌ Source file does not exist: {file}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Error copying {file}: {ex.Message}");
            }
        }
    }
}
