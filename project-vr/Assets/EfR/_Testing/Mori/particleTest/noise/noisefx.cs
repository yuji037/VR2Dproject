using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class noisefx : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    float _intensity = 1.0f;

    public float intensity {
        get { return _intensity; }
        set { _intensity = value; }
    }

    [SerializeField] Shader shader;

    Material material;

    Texture2D noiseTexture;

    RenderTexture oldFrame1;
    RenderTexture oldFrame2;

    int frameCount;

    static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    void SetUpObjects()
    {
        if (material != null) return;

        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;

        noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
        noiseTexture.hideFlags = HideFlags.DontSave;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;
        noiseTexture.filterMode = FilterMode.Point;

        oldFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
        oldFrame2 = new RenderTexture(Screen.width, Screen.height, 0);

        UpdateNoise();
    }

    void UpdateNoise()
    {
        var color = RandomColor();

        for (var y = 0; y < noiseTexture.height; y++)
        {
            for (var x = 0; x < noiseTexture.width; x++)
            {
                if (Random.value > 0.85f) color = RandomColor();
                noiseTexture.SetPixel(x, y, color);
            }
        }

        noiseTexture.Apply();
    }

    void Start()
    {
        SetUpObjects();
    }

    void Update()
    {
        if (Random.value > 0.85f) UpdateNoise();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpObjects();

        if ((frameCount % 8) == 0) Graphics.Blit(source, oldFrame1);
        if ((frameCount % 13) == 0) Graphics.Blit(source, oldFrame2);

        material.SetFloat("_Intensity", _intensity);
        material.SetTexture("_GlitchTex", noiseTexture);
        material.SetTexture("_BufferTex", Random.value > 0.5f ? oldFrame1 : oldFrame2);
    
        Graphics.Blit(source, destination, material);

        frameCount++;
    }
}
