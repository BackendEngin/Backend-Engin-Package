using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PackageInitializer
{
    // Static constructor that runs when the editor loads
    static PackageInitializer()
    {
        CreateFolderIfNotExists();
    }

    private static void CreateFolderIfNotExists()
    {
        string folderPath = "Assets/BackendEngin"; // Change this to your desired folder path

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            // Create the folder
            AssetDatabase.CreateFolder("Assets", "BackendEngin");

            // Now move the contents of the Editor folder into the new folder
            string editorSourcePath = "Packages/com.asoft.backendengine/Editor"; // Adjust this path to your Editor folder location
            string[] files = System.IO.Directory.GetFiles(editorSourcePath, "*.*", System.IO.SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                string destPath = System.IO.Path.Combine(folderPath, fileName);
                System.IO.File.Copy(file, destPath);
                Debug.Log($"Copied {fileName} to {folderPath}");
            }

            // Refresh the AssetDatabase to reflect the changes
            AssetDatabase.Refresh();
        }
    }
}