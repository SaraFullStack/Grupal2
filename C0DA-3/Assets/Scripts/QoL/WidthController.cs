using UnityEngine;

public class WidthController : MonoBehaviour
{
    public Transform middlePiece;
    public Transform leftPiece;
    public Transform rightPiece;
    public BoxCollider coll;

    public float width;

    private void Awake()
    {
        UpdateWidth();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        UpdateWidth();
    }

#endif

    private void UpdateWidth()
    {
        middlePiece.localScale = new Vector3((width - 2f) * 100f, 100f, 100f);
        leftPiece.localPosition = Vector3.right * ((width - 1) / 2);
        rightPiece.localPosition = Vector3.left * ((width - 1) / 2);

        coll.size = new Vector3(width, 1, 1);
    }
}
