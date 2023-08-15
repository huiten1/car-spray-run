
using PaintIn3D;
using UnityEngine;


public class Glitter : MonoBehaviour
{
    [SerializeField] private P3dPaintableTexture paintableTexture;
    private bool started;
    [SerializeField] private ParticleSystem particleSystem;
    // Start is called before the first frame update
    public void Setup()
    {
        started = true;
        var shape = particleSystem.shape;
        shape.textureClipThreshold = 0.0f;
        UpdateTexture();
    }

    private void UpdateTexture()
    {
        var shape = particleSystem.shape;
        shape.texture = toTexture2D(paintableTexture.Current);
        particleSystem.Play();
    }

    private float t = 0;
    private void Update()
    {
        if (!started) return;
        if(Time.time - t <1f) return;
        t = Time.time;
        UpdateTexture();
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
