using UnityEngine;
using System.Collections;

public class SpringyMesh : MonoBehaviour {

  public float springForce = 0.1f;
  public float neighborSpringForce = 0.1f;
  public float damping = 0.1f;

  private Mesh springy_mesh_;
  private Vector3[] positions_;
  private Vector3[] resting_positions_;
  private Vector3[] velocities_;
  private int num_vertices_;

  void Start () {
    MeshFilter mesh_filter = GetComponent<MeshFilter>();
    springy_mesh_ = mesh_filter.mesh;
    springy_mesh_.MarkDynamic();
    num_vertices_ = springy_mesh_.vertexCount;

    InitPositions();
    InitVelocities();
  }

  void InitPositions() {
    positions_ = new Vector3[num_vertices_];
    resting_positions_ = new Vector3[num_vertices_];
    for (int i = 0; i < num_vertices_; ++i) {
      positions_[i] = springy_mesh_.vertices[i];
      resting_positions_[i] = positions_[i];
    }
  }

  void InitVelocities() {
    velocities_ = new Vector3[num_vertices_];
    for (int i = 0; i < num_vertices_; ++i)
      velocities_[i] = Vector3.zero;

    velocities_[0] = Vector3.up;
  }

  void Update() {
    springy_mesh_.vertices = positions_;
    springy_mesh_.RecalculateBounds();
  }

  void FixedUpdate () {
    // Neighbor forces.
    int triangle_length = springy_mesh_.triangles.Length;
    for (int i = 0; i < triangle_length; ++i) {
      
    }

    // Sphere shape forces.
    for (int i = 0; i < num_vertices_; ++i) {
      Vector3 delta = resting_positions_[i] - positions_[i];
      velocities_[i] += springForce * delta;
      velocities_[i] *= (1.0f - damping);
    }

    // Apply velocities.
    for (int i = 0; i < num_vertices_; ++i)
      positions_[i] += velocities_[i];
  }
}
