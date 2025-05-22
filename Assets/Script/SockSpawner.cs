// using UnityEngine;
// using System.Collections.Generic;
//
// public class SockSpawner : MonoBehaviour
// {
//     [System.Serializable]
//     public class SockMaterialData
//     {
//         public Material material;
//         public string id;
//     }
//
//     public GameObject sockPrefab;
//     public int numberOfPairs = 5;
//     public float spawnRadius = 3f;
//     public List<SockMaterialData> sockMaterials = new List<SockMaterialData>();
//
//     void Start()
//     {
//         SpawnSockPairs();
//     }
//
//     void SpawnSockPairs()
//     {
//         if (sockMaterials.Count == 0)
//         {
//             Debug.LogWarning("No materials assigned!");
//             return;
//         }
//
//         List<Transform> spawnedSocks = new List<Transform>();
//
//         for (int i = 0; i < numberOfPairs; i++)
//         {
//             int matIndex = Random.Range(0, sockMaterials.Count);
//             SockMaterialData chosenMaterial = sockMaterials[matIndex];
//
//             for (int j = 0; j < 2; j++)
//             {
//                 Vector3 randomPos = GetRandomPosition(spawnRadius);
//                 GameObject newSock = Instantiate(sockPrefab, transform.position + randomPos, Quaternion.identity);
//
//                 Sock sockScript = newSock.GetComponent<Sock>();
//                 if (sockScript != null)
//                 {
//                     sockScript.SetMaterial(chosenMaterial.material, chosenMaterial.id);
//                 }
//
//                 spawnedSocks.Add(newSock.transform);
//             }
//         }
//     }
//
//     Vector3 GetRandomPosition(float radius)
//     {
//         Vector2 randomCircle = Random.insideUnitCircle * radius;
//         return new Vector3(randomCircle.x, 0f, randomCircle.y);
//     }
//     
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.R))
//         {
//             ClearExistingSocks();
//             SpawnSockPairs();
//         }
//     }
//
//     void ClearExistingSocks()
//     {
//         foreach (var sock in GameObject.FindGameObjectsWithTag("Sock"))
//         {
//             Destroy(sock);
//         }
//     }
//
// }

using UnityEngine;
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
    private int currentPairCount;
    public SockMatchers sockMatchers;


    
    void Start()
    {
        currentPairCount = numberOfPairs;
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

        // 1. Buat list material yang akan digunakan
        List<SockMaterialData> materialsToUse = new List<SockMaterialData>();

        // Salin semua material unik dulu
        for (int i = 0; i < Mathf.Min(numberOfPairs, sockMaterials.Count); i++)
        {
            materialsToUse.Add(sockMaterials[i]);
        }

        // Jika numberOfPairs > jumlah material, ulangi dari awal secara acak
        while (materialsToUse.Count < numberOfPairs)
        {
            int randomIndex = Random.Range(0, sockMaterials.Count);
            materialsToUse.Add(sockMaterials[randomIndex]);
        }

        // Acak urutan material agar lebih bervariasi
        Shuffle(materialsToUse);

        // 2. Buat pasangan untuk setiap material
        foreach (SockMaterialData chosenMaterial in materialsToUse)
        {
            for (int j = 0; j < 2; j++)
            {
                Vector3 randomPos = GetRandomPosition(spawnRadius);
                GameObject newSock = Instantiate(sockPrefab, transform.position + randomPos, Quaternion.identity);

                Sock sockScript = newSock.GetComponent<Sock>();
                if (sockScript != null)
                {
                    sockScript.SetMaterial(chosenMaterial.material, chosenMaterial.id);
                    sockScript.spawner = this;
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
        
        //HILANGKAN PORTAL
        if (sockMatchers != null)
        {
            sockMatchers.HidePortal();
        }

        if (currentSockCount <= 0)
        {
            currentPairCount++; // tambah 1 pasang
            SpawnSockPairs();
        }
    }

    void UpdateSockCounterUI()
    {
        if (sockCountText != null)
        {
            sockCountText.text = $"{currentSockCount}";
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