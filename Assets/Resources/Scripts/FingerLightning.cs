using UnityEngine;
using System.Collections;

public class FingerLightning : MonoBehaviour {

  private const float Y_PERLIN_OFFSET = 10.0f;
  private const float Z_PERLIN_OFFSET = 20.0f;

  public float maxZapDistance = 2.0f;
  public float lightningPerUnit = 20.0f;
  public float lightningChangeFrequency = 1.0f;
  public float lightningJaggedness = 5.0f;
  public float lightningAmplitude = 1.0f;
  public float zapStrength = 0.0001f;
  public ParticleSystem sparks;

  private float lightning_noise_phase_;

  void Start() {
    sparks.Stop();
    lightning_noise_phase_ = Random.Range(0.0f, 10000.0f);
    RemoveLine();
  }

  void RemoveLine() {
    sparks.Stop();
    LineRenderer line_renderer = GetComponent<LineRenderer>();
    if (line_renderer != null)
      line_renderer.SetVertexCount(0);
  }

  void DrawLine(Vector3 start, Vector3 end) {
    LineRenderer line_renderer = GetComponent<LineRenderer>();
    if (line_renderer == null)
      return;

    float distance = (start - end).magnitude;
    int number_points = (int)(2 + distance * lightningPerUnit);
    line_renderer.SetVertexCount(number_points);

    for (int i = 0; i < number_points; ++i) {
      float phase = i / (number_points - 1.0f);
      Vector3 normal_position = start + phase * (end - start);

      float offset_x = Mathf.PerlinNoise(lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;
      float offset_y = Mathf.PerlinNoise(Y_PERLIN_OFFSET + lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;
      float offset_z = Mathf.PerlinNoise(Z_PERLIN_OFFSET + lightning_noise_phase_,
                                         phase * lightningJaggedness) - 0.5f;

      Vector3 offset = lightningAmplitude * phase * (1 - phase) *
                       new Vector3(offset_x, offset_y, offset_z);
      line_renderer.SetPosition(i, normal_position + offset);
    }

    lightning_noise_phase_ += Time.deltaTime * lightningChangeFrequency;
    /*
    sparks.transform.position = end;
    if (!sparks.isPlaying)
      sparks.Play();
      */
  }

  void ZapElectroSpheres() {
    Vector3 position = GetComponent<FingerModel>().GetTipPosition();
    SpringyMesh[] spheres = FindObjectsOfType<SpringyMesh>() as SpringyMesh[];
    float closest_distance = maxZapDistance;
    SpringyMesh target = null;
    Vector3 zap = Vector3.zero;

    foreach (SpringyMesh sphere in spheres) {
      Vector3 zap_point = sphere.GetZappingPoint(position);
      Vector3 delta = zap_point - position;
      if (delta.magnitude < closest_distance) {
        closest_distance = delta.magnitude;
        target = sphere;
        zap = zap_point;
      }
    }

    if (target == null)
      RemoveLine();
    else {
      DrawLine(position, zap);
      target.SuckMesh(position);
    }
  }

  void ZapEnergyGyros() {
    Vector3 position = GetComponent<FingerModel>().GetTipPosition();
    EnergyGyro[] gyros = FindObjectsOfType<EnergyGyro>() as EnergyGyro[];
    float closest_distance = maxZapDistance;
    EnergyGyro target = null;
    Vector3 zap = Vector3.zero;

    foreach (EnergyGyro gyro in gyros) {
      Vector3[] zap_points = gyro.GetZappingPoints();
      foreach (Vector3 zap_point in zap_points) {
        Vector3 delta = zap_point - position;
        if (delta.magnitude < closest_distance) {
          closest_distance = delta.magnitude;
          target = gyro;
          zap = zap_point;
        }
      }
    }

    if (target == null)
      RemoveLine();
    else {
      DrawLine(position, zap);
      target.Zap(zapStrength);
    }
  }

  void Update() {
    ZapEnergyGyros();
    // ZapElectroSpheres();
  }
}
