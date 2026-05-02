using UnityEngine;

[ExecuteAlways]
public class AutoFitVisualToBox : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float padding = 1f;
    [SerializeField] private bool autoFitOnValidate = true;

    [ContextMenu("Auto Fit Visual")]
    public void AutoFitVisual()
    {
        if (visualRoot == null) return;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        Renderer[] renderers = visualRoot.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0) return;

        visualRoot.localPosition = Vector3.zero;
        visualRoot.localRotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;

        Bounds visualBounds = GetRendererBounds(renderers);
        Bounds targetBounds = box.bounds;

        if (visualBounds.size.x <= 0f ||
            visualBounds.size.y <= 0f ||
            visualBounds.size.z <= 0f)
            return;

        Vector3 scaleRatio = new Vector3(
            targetBounds.size.x / visualBounds.size.x,
            targetBounds.size.y / visualBounds.size.y,
            targetBounds.size.z / visualBounds.size.z
        );

        visualRoot.localScale = Vector3.Scale(visualRoot.localScale, scaleRatio * padding);

        renderers = visualRoot.GetComponentsInChildren<Renderer>(true);
        visualBounds = GetRendererBounds(renderers);

        Vector3 offset = targetBounds.center - visualBounds.center;
        visualRoot.position += offset;
    }

    [ContextMenu("Reset Visual")]
    public void ResetVisual()
    {
        if (visualRoot == null) return;

        visualRoot.localPosition = Vector3.zero;
        visualRoot.localRotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;
    }

    private Bounds GetRendererBounds(Renderer[] renderers)
    {
        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }

    private void OnValidate()
    {
        if (!autoFitOnValidate) return;
        AutoFitVisual();
    }
}