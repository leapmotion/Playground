using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpringyMesh : MonoBehaviour {

  public float springForce = 0.1f;
  public float neighborSpringForce = 0.1f;
  public float damping = 0.1f;
  public float maxVertexVelocity = 0.2f;
  public float prodFrequency = 1.0f;
  public float mushStrength = 1.0f;
  public float mushStrengthVertical = 1.0f;

  private Mesh springy_mesh_;
  private Dictionary<int, int> vertex_duplicates_ = new Dictionary<int, int>();
  private Vector3[] positions_;
  private Vector3[] resting_positions_;
  private Vector3[] velocities_;
  private int num_vertices_;

  private float prod_progress_ = 0.0f;

  void Start () {
    MeshFilter mesh_filter = GetComponent<MeshFilter>();
    springy_mesh_ = mesh_filter.mesh;
    springy_mesh_.MarkDynamic();
    num_vertices_ = springy_mesh_.vertexCount;

    InitPositions();
    InitVelocities();
    DeduplicatePoints();
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

  void DeduplicatePoints() {
    for (int i = 0; i < num_vertices_; ++i) {
      int master_index = i;
      for (int j = 0; j < i; ++j) {
        if (positions_[i] == positions_[j]) {
          master_index = j;
          break;
        }
      }
      vertex_duplicates_[i] = master_index;
    }
  }

  void Update() {
    springy_mesh_.vertices = positions_;
    springy_mesh_.RecalculateBounds();
  }

  void ProdVertex(int vertex, Vector3 velocity_bump) {
    velocities_[vertex] += velocity_bump;
  }

  void MushMesh3() {
    for (int i = 0; i < num_vertices_; ++i) {
      float s = resting_positions_[i].x * resting_positions_[i].x +
                resting_positions_[i].z * resting_positions_[i].z;
      velocities_[i] += s * s * s * mushStrength * transform.TransformDirection(resting_positions_[i]);

      float y = resting_positions_[i].y;
      velocities_[i] += y * y * y * y * mushStrengthVertical * transform.TransformDirection(Vector3.up);
    }
  }

  void MushMesh2() {
    for (int i = 0; i < num_vertices_; ++i) {
      float s = resting_positions_[i].x * resting_positions_[i].x +
                resting_positions_[i].z * resting_positions_[i].z;
      velocities_[i] += s * s * s * mushStrength * transform.TransformDirection(resting_positions_[i]);
    }
  }

  void MushMesh() {
    for (int i = 0; i < num_vertices_; ++i) {
      float y = resting_positions_[i].y;
      velocities_[i] += y * mushStrength * transform.TransformDirection(Vector3.up);
    }
  }

  void ApplyNeighborForces() {
    int[] triangles = springy_mesh_.triangles;
    int triangle_length = triangles.Length;
    for (int i = 0; i < triangle_length;) {
      int one = vertex_duplicates_[triangles[i++]];
      int two = vertex_duplicates_[triangles[i++]];
      int three = vertex_duplicates_[triangles[i++]];

      Vector3 delta_one_two = positions_[one] - positions_[two];
      Vector3 delta_two_three = positions_[two] - positions_[three];
      Vector3 delta_three_one = positions_[three] - positions_[one];

      float normal_distance1 = (resting_positions_[three] - resting_positions_[one]).magnitude;
      float normal_distance2 = (resting_positions_[one] - resting_positions_[two]).magnitude;
      float normal_distance3 = (resting_positions_[two] - resting_positions_[three]).magnitude;

      float distance1 = delta_three_one.magnitude - normal_distance1;
      float distance2 = delta_one_two.magnitude - normal_distance2;
      float distance3 = delta_two_three.magnitude - normal_distance3;

      if (delta_one_two.magnitude != 0 && distance2 != 0)
        velocities_[two] += neighborSpringForce * distance2 * delta_one_two.normalized;
      if (delta_two_three.magnitude != 0 && distance3 != 0)
        velocities_[three] += neighborSpringForce * distance3 * delta_two_three.normalized;
      if (delta_three_one.magnitude != 0 && distance1 != 0)
        velocities_[one] += neighborSpringForce * distance1 * delta_three_one.normalized;
    }
  }

  void FixedUpdate () {
    MushMesh3();
    ApplyNeighborForces();

    // Sphere shape forces.
    for (int i = 0; i < num_vertices_; ++i) {
      if (vertex_duplicates_[i] == i) {
        Vector3 delta = resting_positions_[i] - positions_[i];
        velocities_[i] += springForce * delta;
        velocities_[i] *= (1.0f - damping);
        velocities_[i] = Vector3.ClampMagnitude(velocities_[i], maxVertexVelocity);
      }
    }

    // Apply velocities.
    for (int i = 0; i < num_vertices_; ++i) {
      if (vertex_duplicates_[i] == i)
        positions_[i] += velocities_[i];
    }

    // Update duplicate vertices.
    for (int i = 0; i < num_vertices_; ++i) {
      if (vertex_duplicates_[i] != i)
        positions_[i] = positions_[vertex_duplicates_[i]];
    }

    prod_progress_ += Time.deltaTime * prodFrequency;
    // ProdVertex(200, Mathf.Sin(prod_progress_) * 10 * Vector3.right);
  }
}
