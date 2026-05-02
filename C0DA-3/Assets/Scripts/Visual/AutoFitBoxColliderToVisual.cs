using UnityEngine;

[ExecuteAlways]
public class AutoFitBoxColliderTight : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    [ContextMenu("Fit Collider Tight")]
    public void Fit()
    {
        if (visualRoot == null) return;

        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        MeshFilter[] meshes = visualRoot.GetComponentsInChildren<MeshFilter>();
        if (meshes.Length == 0) return;

        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        foreach (var mf in meshes)
        {
            if (mf.sharedMesh == null) continue;

            var mesh = mf.sharedMesh;
            var verts = mesh.vertices;

            foreach (var v in verts)
            {
                Vector3 world = mf.transform.TransformPoint(v);
                Vector3 local = transform.InverseTransformPoint(world);

                min = Vector3.Min(min, local);
                max = Vector3.Max(max, local);
            }
        }

        Vector3 size = max - min;
        Vector3 center = (max + min) * 0.5f;

        box.center = center;
        box.size = size;
    }

    private void OnValidate()
    {
        Fit();
    }
}