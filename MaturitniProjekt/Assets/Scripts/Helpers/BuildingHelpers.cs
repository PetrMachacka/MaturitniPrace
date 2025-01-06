using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class BuilidingHelpers
    {
        public static void SetTransparentMaterial(Renderer renderer, bool obstructed)
        {
            Material BrightRed = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            BrightRed.color = Color.red;

            Material material = renderer.material;

            material.SetFloat("_Surface", 1); 
            material.SetFloat("_Blend", 0); 
            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            Color color = material.color;
            color.a = 0.5f; 
            material.color = color;

            if (obstructed)
            {
                renderer.material = BrightRed;
            }
        }
        public static GameObject GenerateLine(GameObject linePrefab, Vector3 start, Vector3 end)
        {
            GameObject line = GameObject.Instantiate(linePrefab);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
            return line;
        }
        public static GameObject EditingCube(GameObject parent)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(parent.transform);
            cube.transform.localPosition = Vector3.zero;
            cube.tag = "Edit";
            cube.layer = 9;
            cube.GetComponent<Renderer>().enabled = false;
            return cube;
        }
        public static bool IsHoldingTool(GameObject objectPrefab)
        {
            return objectPrefab.GetComponent<Item>().isTool;
        }
        public static Vector3 VectorRound(Vector3 vector)
        {
            return new Vector3((float)Math.Round(vector.x * 2) / 2, (float)Math.Round(vector.y * 2) / 2, (float)Math.Round(vector.z * 2) / 2);
        }
    }
}