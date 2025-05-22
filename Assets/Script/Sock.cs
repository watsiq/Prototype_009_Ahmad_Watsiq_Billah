using UnityEngine;

public class Sock : MonoBehaviour
{
    public Renderer BodyRenderer;
    public Material sockMaterial;
    public string materialID;
    public SockMatchers matcher;
    
    public SockSpawner spawner;

    private bool isMatched = false;

    public void SetMaterial(Material mat, string id)
    {
        sockMaterial = mat;
        materialID = id;
        BodyRenderer.material = mat;
    }

    public bool IsMatch(Sock otherSock)
    {
        return materialID == otherSock.materialID;
    }

    void Start()
    {
        if (matcher == null)
            matcher = FindFirstObjectByType<SockMatchers>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sock"))
        {
            Sock otherSock = other.GetComponent<Sock>();

            if (otherSock != null && otherSock != this)
            {
                // Cegah double match
                if (isMatched || otherSock.isMatched)
                    return;

                if (IsMatch(otherSock))
                {
                    isMatched = true;
                    otherSock.isMatched = true;

                    Debug.Log("Match Detected!");
                    matcher?.TryMatch(this, otherSock);
                }
            }
        }
    }
}