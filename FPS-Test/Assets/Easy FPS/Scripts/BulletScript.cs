using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000f;

    [Tooltip("Prefab of wall damage decal")]
    public GameObject decalHitWall;

    [Tooltip("Offset to prevent z-fighting with wall surface")]
    public float floatInfrontOfWall = 0.05f;

    [Tooltip("Blood effect prefab to instantiate on enemy hit")]
    public GameObject bloodEffect;

    [Tooltip("Layers to ignore in the raycast")]
    public LayerMask ignoreLayer;

    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            // Hit wall
            if (hit.transform.CompareTag("LevelPart") && decalHitWall != null)
            {
                Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
            }

            // Hit player
            if (hit.transform.CompareTag("Player"))
            {
                PlayerHealth health = hit.transform.GetComponent<PlayerHealth>();
                if (health != null && health.IsSpawned)
                {
                    // Solicita ao servidor que aplique o dano
                    health.TakeDamageServerRpc(25);
                }

                if (bloodEffect != null)
                {
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }

        // Destroi o projétil após 0.1s
        Destroy(gameObject, 0.1f);
    }
}
