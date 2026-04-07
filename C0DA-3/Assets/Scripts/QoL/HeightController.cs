using UnityEngine;

public class HeightController : MonoBehaviour
{
    public Transform middlePiece;
    public Transform topPiece;
    public BoxCollider coll;

    public float height;

    private void Awake()
    {
        UpdateHeight();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        UpdateHeight();
    }

#endif

    private void UpdateHeight()
    {
        middlePiece.localScale = new Vector3(100f, 100f, (height - 2f) * 100f);
        topPiece.localPosition = Vector3.up * (height - 1);

        coll.center = Vector3.up * (height / 2);
        coll.size = new Vector3(1, height, 1);
    }
}
