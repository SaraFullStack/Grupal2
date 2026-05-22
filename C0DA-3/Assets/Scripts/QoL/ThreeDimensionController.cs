using UnityEngine;

public class ThreeDimensionController : MonoBehaviour
{
    public Transform[] middleYPieces;
    public Transform[] topPieces;
    public Transform[] bottomPieces;

    public Transform[] middleXPieces;
    public Transform[] leftPieces;
    public Transform[] rightPieces;

    public Transform[] middleZPieces;
    public Transform[] backPieces;
    public Transform[] frontPieces;

    public Transform coll;

    public float height;
    public float width;
    public float length;

    private void Awake()
    {
        UpdateDimensions();
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        UpdateDimensions();
    }

#endif

    private void UpdateDimensions()
    {
        foreach (var piece in middleYPieces)
        {
            piece.localScale = new Vector3(piece.localScale.x, piece.localScale.y, height * 100f + 1f);
        }
        foreach (var piece in topPieces)
        {
            piece.localPosition = new Vector3(piece.localPosition.x, (height - 1) / 2, piece.localPosition.z);
        }
        foreach (var piece in bottomPieces)
        {
            piece.localPosition = new Vector3(piece.localPosition.x, -(height - 1) / 2, piece.localPosition.z);
        }
        foreach (var piece in middleXPieces)
        {
            piece.localScale = new Vector3(width * 100f + 1f, piece.localScale.y, piece.localScale.z);
        }
        foreach (var piece in rightPieces)
        {
            piece.localPosition = new Vector3((width - 1) / 2, piece.localPosition.y, piece.localPosition.z);
        }
        foreach (var piece in leftPieces)
        {
            piece.localPosition = new Vector3(-(width - 1) / 2, piece.localPosition.y, piece.localPosition.z);
        }
        foreach (var piece in middleZPieces)
        {
            piece.localScale = new Vector3(piece.localScale.x, length * 100f + 1f, piece.localScale.z);
        }
        foreach (var piece in frontPieces)
        {
            piece.localPosition = new Vector3(piece.localPosition.x, piece.localPosition.y, (length - 1) / 2);
        }
        foreach (var piece in backPieces)
        {
            piece.localPosition = new Vector3(piece.localPosition.x, piece.localPosition.y, -(length - 1) / 2);
        }

        coll.localScale = new Vector3(width + 0.6f, height + 0.6f, length + 0.6f);
    }
}
