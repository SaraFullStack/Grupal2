using UnityEngine;

public class ScrollUVs : MonoBehaviour
{

    [SerializeField] float scrollSpeedX = 0.5f;
    [SerializeField] float scrollSpeedY = 0.5f;
    Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float xOffset = Time.time * scrollSpeedX;
        float yOffset = Time.time * scrollSpeedY;
        rend.materials[1].SetTextureOffset("_BaseMap", new Vector2(xOffset, yOffset));
        rend.materials[1].SetTextureOffset("_EmissionMap", new Vector2(xOffset, yOffset));
    }
}
