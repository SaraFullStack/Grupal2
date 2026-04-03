using UnityEngine;

public class PillarController : MonoBehaviour
{
    public Transform middlePiece;
    public Transform topPiece;
    public BoxCollider coll;

    public float height;

    private void Awake()
    {
        UpdatePillar();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        UpdatePillar();
    }

#endif

    private void UpdatePillar()
    {
        middlePiece.localScale = new Vector3(100f, 100f, (height - 2f) * 100f);
        topPiece.position = Vector3.up * (height - 1);

        coll.center = Vector3.up * (height / 2);
        coll.size = new Vector3(1, height, 1);
    }
}
