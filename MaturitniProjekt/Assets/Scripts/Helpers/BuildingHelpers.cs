using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public static class BuilidingHelpers
    {
        public static void SetTransparentMaterial(Renderer renderer, bool obstructed)
        {
            Material BrightRed = new Material(Shader.Find("Standard"));
            BrightRed.color = Color.red;
            Material material = renderer.material;

            material.SetFloat("_Mode", 3);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = 0.7f;
            if (obstructed)
            {
                renderer.material = BrightRed;
            }

            Debug.Log(color);
            material.color = color;
        }

    }
}