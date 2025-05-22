using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sock"))
        {
            ScoreManager.Instance.AddScore();
            Destroy(other.gameObject); // Hapus sock setelah masuk portal
        }
    }
}