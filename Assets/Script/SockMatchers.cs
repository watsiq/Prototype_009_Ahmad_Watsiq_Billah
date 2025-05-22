using UnityEngine;
using UnityEngine.Events;

public class SockMatchers : MonoBehaviour
{
    private GameObject currentPortal;
    private string activePairID = "";
    private int matchedSockEntered = 0;

    
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

            // Spawn Portal jika belum ada
            if (currentPortal == null && portalPrefab != null && portalSpawnPoint != null)
            {
                currentPortal = Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
            }

            // Simpan ID pasangan aktif & reset counter
            activePairID = sock1.materialID;
            matchedSockEntered = 0;
        }
        else
        {
            Debug.Log("Not a match.");
            onMismatch?.Invoke();
        }
    }
    
    public void SockEnteredPortal(Sock sock)
    {
        Debug.Log($"[Portal] Sock masuk: {sock.materialID}, Pair aktif: {activePairID}");
        if (sock.materialID == activePairID)
        {
            matchedSockEntered++;
            Debug.Log($"[Portal] Match masuk ke-{matchedSockEntered}");
            if (matchedSockEntered >= 2)
            {
                Debug.Log("[Portal] Kedua sock match masuk, portal akan hilang");
                HidePortal(); // setelah 2 sock yang match masuk, portal hilang
            }
        }
        else
        {
            Debug.Log("[Portal] Sock ID tidak cocok, tidak dihitung sebagai match");
        }
    }


    public void HidePortal()
    {
        if (currentPortal != null)
        {
            Destroy(currentPortal);
            currentPortal = null;
            activePairID = "";
            matchedSockEntered = 0;
        }
    }
}