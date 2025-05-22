using UnityEngine;
using UnityEngine.Events;

public class SockMatchers : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onMatch;
    public UnityEvent onMismatch;

    [Header("Portal")]
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;

    public void TryMatch(Sock sock1, Sock sock2)
    {
        if (sock1.IsMatch(sock2))
        {
            Debug.Log("✅ Match!");

            onMatch?.Invoke();

            // 🔁 Spawn Portal
            if (portalPrefab != null && portalSpawnPoint != null)
            {
                Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
            }
        }
        else
        {
            Debug.Log("❌ Not a match.");
            onMismatch?.Invoke();
        }
    }
}