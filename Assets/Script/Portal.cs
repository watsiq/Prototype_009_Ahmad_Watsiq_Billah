using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.AddScore();

        Sock sock = other.GetComponent<Sock>();
        if (sock != null)
        {
            Debug.Log($"[Portal] Sock '{sock.materialID}' diproses di portal.");

            if (sock.spawner != null)
            {
                Debug.Log("[Portal] Memanggil DecreaseSockCount()");
                sock.spawner.DecreaseSockCount();

                if (sock.spawner.sockMatchers != null)
                {
                    Debug.Log("[Portal] Memanggil SockEnteredPortal()");
                    sock.spawner.sockMatchers.SockEnteredPortal(sock);
                }
                else
                {
                    Debug.LogWarning("[Portal] sockMatchers null!");
                }
            }
            else
            {
                Debug.LogWarning("[Portal] sock.spawner null!");
            }
        }
    }
}