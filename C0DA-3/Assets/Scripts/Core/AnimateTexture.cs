using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextureAnimator : MonoBehaviour
{
    public List<Texture2D> Textures;
    public Renderer TargetRenderer;
    [Range(0, 24)] public int Speed;
    public float currentIndex;

    private void Update()
    {
        currentIndex += Speed * Time.deltaTime;

        var i = (int)currentIndex;

        if (i > Textures.Count - 1)
        {
            currentIndex = 0;
            i = 0;
        }

        // _BaseMap for URP Lit  _MainTex for built in RP
        TargetRenderer.material.SetTexture("_BaseMap", Textures[i]);
        TargetRenderer.material.SetTexture("_EmissionMap", Textures[i]);
    }
}