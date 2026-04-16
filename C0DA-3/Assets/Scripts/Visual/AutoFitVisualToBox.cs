using UnityEngine;

[ExecuteAlways]
public class AutoFitVisualToBox : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    [SerializeField] private float padding = 0.9f; 
    [SerializeField] private float yOffset = 0f;   

    [ContextMenu("Auto Fit Visual")]
    public void AutoFitVisual()
    {
        if (visualRoot == null)
        {
            return;
        }

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null)
        {
            return;
        }

        Renderer[] renderers = visualRoot.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return;
        }

        visualRoot.localPosition = Vector3.zero;
        visualRoot.localRotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;

        Bounds visualBounds = GetBounds(renderers);
        Bounds targetBounds = box.bounds;

        if (visualBounds.size.x <= 0f || visualBounds.size.y <= 0f || visualBounds.size.z <= 0f)
        {
            return;
        }

        Vector3 ratio = new Vector3(
            targetBounds.size.x / visualBounds.size.x,
            targetBounds.size.y / visualBounds.size.y,
            targetBounds.size.z / visualBounds.size.z
        );

        float uniformScale = Mathf.Min(ratio.x, ratio.y, ratio.z) * padding;
        visualRoot.localScale = Vector3.one * uniformScale;

        renderers = visualRoot.GetComponentsInChildren<Renderer>();
        visualBounds = GetBounds(renderers);

        Vector3 pos = visualRoot.position;

        pos.x += targetBounds.center.x - visualBounds.center.x;
        pos.z += targetBounds.center.z - visualBounds.center.z;

        float visualBottom = visualBounds.min.y;
        float targetBottom = targetBounds.min.y;
        pos.y += (targetBottom - visualBottom) + yOffset;

        visualRoot.position = pos;
    }

    [ContextMenu("Reset Visual Transform")]
    public void ResetVisualTransform()
    {
        if (visualRoot == null) return;

        visualRoot.localPosition = Vector3.zero;
        visualRoot.localRotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;
    }

    private Bounds GetBounds(Renderer[] renderers)
    {
        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);
        return bounds;
    }
}