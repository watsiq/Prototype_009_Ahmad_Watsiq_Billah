using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SockMatchers : MonoBehaviour
{
    private List<Sock> matchedSockObjects = new List<Sock>();
    public GameObject currentPortal { get; private set; }
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
            Debug.Log("Match!");
            onMatch?.Invoke();

            if (currentPortal == null && portalPrefab != null && portalSpawnPoint != null)
            {
                currentPortal = Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
            }

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
            Debug.Log($"Menonaktifkan sock: {sock.gameObject.name}");
            
            sock.gameObject.SetActive(false); // Nonaktifkan sock
            matchedSockObjects.Add(sock);

            if (matchedSockEntered >= 2)
            {
                matchedSockEntered = 0;
                activePairID = "";
                Debug.Log("[SockMatchers] Kedua sock masuk portal, sembunyikan portal");

                if (currentPortal != null)
                {
                    currentPortal.SetActive(false);
                    currentPortal = null;
                }

                FindFirstObjectByType<SockSpawner>()?.CheckIfAllSocksMatched(matchedSockObjects);
            }
        }
        else
        {
            Debug.LogWarning("[SockMatchers] Sock materialID tidak cocok dengan activePairID");
        }
    }

    public void DestroyMatchedSocks()
    {
        foreach (var sock in matchedSockObjects)
        {
            if (sock != null)
                Destroy(sock.gameObject);
        }

        matchedSockObjects.Clear();

        if (currentPortal != null)
        {
            Destroy(currentPortal);
            currentPortal = null;
        }
    }

    public void HidePortal()
    {
        if (currentPortal != null)
        {
            currentPortal.SetActive(false);
        }

        activePairID = "";
        matchedSockEntered = 0;
    }
}
