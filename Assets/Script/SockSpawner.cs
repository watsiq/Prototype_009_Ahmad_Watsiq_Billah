using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SockSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SockMaterialData
    {
        public Material material;
        public string id;
    }

    public TextMeshProUGUI sockCountText;
    public GameObject sockPrefab;
    public int numberOfPairs = 5;
    public float spawnRadius = 3f;
    public List<SockMaterialData> sockMaterials = new List<SockMaterialData>();

    private int currentSockCount = 0;
    private List<Sock> allSpawnedSocks = new List<Sock>();

    public SockMatchers sockMatchers;

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

        currentSockCount = 0;
        allSpawnedSocks.Clear();

        List<SockMaterialData> materialsToUse = new List<SockMaterialData>();

        for (int i = 0; i < Mathf.Min(numberOfPairs, sockMaterials.Count); i++)
        {
            materialsToUse.Add(sockMaterials[i]);
        }

        while (materialsToUse.Count < numberOfPairs)
        {
            int randomIndex = Random.Range(0, sockMaterials.Count);
            materialsToUse.Add(sockMaterials[randomIndex]);
        }

        Shuffle(materialsToUse);

        foreach (SockMaterialData materialData in materialsToUse)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 spawnPos = transform.position + GetRandomPosition(spawnRadius);
                GameObject newSock = Instantiate(sockPrefab, spawnPos, Quaternion.identity);

                Sock sockScript = newSock.GetComponent<Sock>();
                if (sockScript != null)
                {
                    sockScript.SetMaterial(materialData.material, materialData.id);
                    sockScript.spawner = this;
                    allSpawnedSocks.Add(sockScript);
                }

                currentSockCount++;
            }
        }

        UpdateSockCounterUI();
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void DecreaseSockCount()
    {
        currentSockCount--;
        UpdateSockCounterUI();

        if (currentSockCount <= 0)
        {
            sockMatchers?.DestroyMatchedSocks();
            numberOfPairs++;
            SpawnSockPairs();
        }
    }

    void UpdateSockCounterUI()
    {
        if (sockCountText != null)
        {
            sockCountText.text = currentSockCount.ToString();
        }
    }

    Vector3 GetRandomPosition(float radius)
    {
        Vector2 circle = Random.insideUnitCircle * radius;
        return new Vector3(circle.x, 0f, circle.y);
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
        allSpawnedSocks.Clear();
    }

    public void CheckIfAllSocksMatched(List<Sock> matchedSocks)
    {
        int activeCount = 0;
        foreach (var sock in allSpawnedSocks)
        {
            if (sock != null && sock.gameObject.activeSelf)
            {
                activeCount++;
            }
        }

        if (activeCount == 0)
        {
            StartCoroutine(DestroyAndRespawn(matchedSocks));
        }
    }

    private IEnumerator DestroyAndRespawn(List<Sock> matchedSocks)
    {
        yield return new WaitForSeconds(1f);

        foreach (var sock in matchedSocks)
        {
            if (sock != null)
                Destroy(sock.gameObject);
        }

        if (sockMatchers != null && sockMatchers.currentPortal != null)
        {
            Destroy(sockMatchers.currentPortal);
        }

        numberOfPairs++;
        SpawnSockPairs();
    }
}
