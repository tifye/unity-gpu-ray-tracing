using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public Vector2 SphereRadius = new Vector2(3.0f, 8.0f);
    public uint SphereMax = 100;
    public float SpherePlacementRadius = 100.0f;

    [HideInInspector]
    public List<Sphere> spheres = new List<Sphere>();
    public void SetUpScene() {
        spheres.Clear(); // TODO: Add Pooling

        // Add random shperes
        for (int i = 0; i < SphereMax; i++) {
            Sphere sphere = new Sphere();

            Vector2 randomPos = Random.insideUnitCircle * SpherePlacementRadius;
            sphere.radius = SphereRadius.x + Mathf.PerlinNoise(randomPos.x, randomPos.y) * (SphereRadius.y - SphereRadius.x);
            sphere.position = new Vector3(randomPos.x, sphere.radius, randomPos.y);

            // Reject spheres that are intersection others
            if (IsSphereIntersectingOthers(sphere, spheres)) continue;

            // Albedo and specular colors
            Color color = Random.ColorHSV();
            bool metal = Random.value < 0.5f;
            sphere.albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b);
            sphere.specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f;

            spheres.Add(sphere);
        }
    }

    private bool IsSphereIntersectingOthers(Sphere sphere, List<Sphere> spheres) {
        foreach (Sphere other in spheres) {
            float minDist = sphere.radius + other.radius;
            if (Vector3.Distance(sphere.position, other.position) < minDist) {
                return true;
            }
        }
        return false;
    }
}
