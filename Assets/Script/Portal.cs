// using UnityEngine;
//
// public class Portal : MonoBehaviour
// {
//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Sock"))
//         {
//             ScoreManager.Instance.AddScore();
//             Destroy(other.gameObject); // Hapus sock setelah masuk portal
//         }
//     }
// }

using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ScoreManager.Instance.AddScore();

        Sock sock = other.GetComponent<Sock>();
        if (sock != null)
        {
            if (sock.spawner != null)
            {
                sock.spawner.DecreaseSockCount();
            }

            // ➕ Tambahkan ini:
            if (sock.spawner != null && sock.spawner.sockMatchers != null)
            {
                Debug.Log("[Portal] SockEnteredPortal() dipanggil");
                sock.spawner.sockMatchers.SockEnteredPortal(sock);
            }
        }

        Destroy(other.gameObject);

    }
}