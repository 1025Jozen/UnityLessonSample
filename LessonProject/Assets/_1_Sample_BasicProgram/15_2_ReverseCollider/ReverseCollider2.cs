using System.Linq;
using UnityEngine;

namespace col_sample
{
    public class ReverseCollider2 : MonoBehaviour
    {
        private void Start() {         
            InvertMesh();
            gameObject.AddComponent<MeshCollider>();
        }

        private void InvertMesh() {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }
}