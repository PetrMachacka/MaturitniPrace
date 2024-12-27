using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class FileHelpers
    {
        public static string GetFolderPathByGuid(string selectedGuid)
        {
            string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");

            if (!Directory.Exists(levelsPath))
            {
                throw new DirectoryNotFoundException($"The directory '{levelsPath}' does not exist.");
            }

            string[] directories = Directory.GetDirectories(levelsPath);

            foreach (string directory in directories)
            {
                
                string directoryName = Path.GetFileName(directory);
                Debug.Log(selectedGuid);
                Debug.Log(directoryName);
                if (directoryName == selectedGuid)
                {
                    return directory;
                }
            }

            throw new DirectoryNotFoundException($"No directory found with the GUID '{selectedGuid}'.");
        }
    }
}