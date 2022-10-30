using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SimplifySimple: MonoBehaviour
{
    [SerializeField, Range(0f, 1f), Tooltip("The desired quality of the simplified mesh.")]
    private float quality = 0.5f;

    private void Start()
    {
        Simplify();
    }

    private void Simplify()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) // verify that there is a mesh filter
            return;

        Mesh sourceMesh = meshFilter.sharedMesh;
        if (sourceMesh == null) // verify that the mesh filter actually has a mesh
            return;

        // Create our mesh simplifier and setup our vertices and indices from all sub meshes in it
        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Vertices = sourceMesh.vertices;
        // Debug.Log(meshSimplifier.Vertices);

        for (int i = 0; i < sourceMesh.subMeshCount; i++)
        {
            meshSimplifier.AddSubMeshTriangles(sourceMesh.GetTriangles(i));
        }

        // This is where the magic happens, lets simplify!
        meshSimplifier.SimplifyMesh(quality);

        // Create our new mesh and transfer vertices and indices from all sub meshes
        var newMesh = new Mesh();
        newMesh.subMeshCount = meshSimplifier.SubMeshCount;
        newMesh.vertices = meshSimplifier.Vertices;

        for (int i = 0; i < meshSimplifier.SubMeshCount; i++)
        {
            newMesh.SetTriangles(meshSimplifier.GetSubMeshTriangles(i), 0);
        }

        meshFilter.sharedMesh = newMesh;
    }
}