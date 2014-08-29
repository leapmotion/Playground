using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetSphereUV : MonoBehaviour {

  void Start () {
    Mesh mesh = GetComponent<MeshFilter>().mesh;
    Vector2[] uv = mesh.uv;
    Vector3[] vertices = mesh.vertices;
    int num_vertices = mesh.vertexCount;

    for (int i = 0; i < num_vertices; ++i) {
      uv[i] = new Vector2((vertices[i].y + 1) / 2.0f, 0.0f);
    }
    mesh.uv = uv;
  }
}
