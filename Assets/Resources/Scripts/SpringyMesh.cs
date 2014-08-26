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

      positions_[i].x *= 2.0f;
      positions_[i].y /= 2.0f;
    }
  }

  void InitVelocities() {
    velocities_ = new Vector3[num_vertices_];
    for (int i = 0; i < num_vertices_; ++i) {
      velocities_[i] = Vector3.zero;
    }
  }

  void Update() {
    springy_mesh_.vertices = positions_;
    springy_mesh_.RecalculateBounds();
  }

  void FixedUpdate () {
    // Neighbor forces.
    int[] triangles = springy_mesh_.triangles;
    int triangle_length = triangles.Length;
    for (int i = 0; i < triangle_length;) {
      int one = triangles[i++];
      int two = triangles[i++];
      int three = triangles[i++];

      Vector3 delta_one_two = positions_[one] - positions_[two];
      Vector3 delta_two_three = positions_[two] - positions_[three];
      Vector3 delta_three_one = positions_[three] - positions_[one];

      float normal_distance1 = (resting_positions_[three] - resting_positions_[one]).magnitude;
      float normal_distance2 = (resting_positions_[one] - resting_positions_[two]).magnitude;
      float normal_distance3 = (resting_positions_[two] - resting_positions_[three]).magnitude;

      float distance1 = delta_three_one.magnitude - normal_distance1;
      float distance2 = delta_one_two.magnitude - normal_distance2;
      float distance3 = delta_two_three.magnitude - normal_distance3;

      velocities_[two] += neighborSpringForce * distance2 * delta_one_two.normalized;
      velocities_[three] += neighborSpringForce * distance3 * delta_two_three.normalized;
      velocities_[one] += neighborSpringForce * distance1 * delta_three_one.normalized;
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
