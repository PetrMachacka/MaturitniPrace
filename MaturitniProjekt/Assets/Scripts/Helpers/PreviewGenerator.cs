#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class PreviewGenerator : MonoBehaviour
{
    [ContextMenu("Generate Previews")]
    private void Start()
    {
        GeneratePreviews();
    }

    public void GeneratePreviews()
    {
        GeneratePreviewsForFolder("Prefabs/Building", "BuildingPictures");
        GeneratePreviewsForFolder("Prefabs/Tools", "ToolsPictures");
    }

    private void GeneratePreviewsForFolder(string folderPath, string outputFolder)
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>(folderPath);
        foreach (GameObject prefab in prefabs)
        {
            Debug.Log(prefab.name);
            Texture2D preview = AssetPreview.GetAssetPreview(prefab);
            if (preview != null)
            {
                byte[] bytes = preview.EncodeToPNG();
                string path = Application.dataPath + $"/Resources/Prefabs/{outputFolder}/{prefab.name}.png";
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log($"Saved preview for {prefab.name} at {path}");
            }
        }
    }
}
#endif