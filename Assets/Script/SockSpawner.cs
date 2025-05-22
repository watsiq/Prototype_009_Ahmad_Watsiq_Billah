using UnityEngine;
using System.Collections.Generic;

public class SockSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SockMaterialData
    {
        public Material material;
        public string id;
    }

    public GameObject sockPrefab;
    public int numberOfPairs = 5;
    public float spawnRadius = 3f;
    public List<SockMaterialData> sockMaterials = new List<SockMaterialData>();

    void Start()
    {
        SpawnSockPairs();
    }

    void SpawnSockPairs()
    {
        if (sockMaterials.Count == 0)
        {
            Debug.LogWarning("No materials assigned!");
            return;
        }

        List<Transform> spawnedSocks = new List<Transform>();

        for (int i = 0; i < numberOfPairs; i++)
        {
            int matIndex = Random.Range(0, sockMaterials.Count);
            SockMaterialData chosenMaterial = sockMaterials[matIndex];

            for (int j = 0; j < 2; j++)
            {
                Vector3 randomPos = GetRandomPosition(spawnRadius);
                GameObject newSock = Instantiate(sockPrefab, transform.position + randomPos, Quaternion.identity);

                Sock sockScript = newSock.GetComponent<Sock>();
                if (sockScript != null)
                {
                    sockScript.SetMaterial(chosenMaterial.material, chosenMaterial.id);
                }

                spawnedSocks.Add(newSock.transform);
            }
        }
    }

    Vector3 GetRandomPosition(float radius)
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return new Vector3(randomCircle.x, 0f, randomCircle.y);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearExistingSocks();
            SpawnSockPairs();
        }
    }

    void ClearExistingSocks()
    {
        foreach (var sock in GameObject.FindGameObjectsWithTag("Sock"))
        {
            Destroy(sock);
        }
    }

}