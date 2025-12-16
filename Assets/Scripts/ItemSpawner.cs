using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public int numberOfItems = 5;

    private BoxCollider spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
        SpawnItems();
    }

    void SpawnItems()
    {
        for (int i = 0; i < 5; i++)
            SpawnSingleItem();
    }

    void SpawnSingleItem()
    {
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 rayOrigin = new Vector3(randomX, bounds.max.y, randomZ);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, bounds.size.y))
        {
            GameObject randomItem = objects[Random.Range(0, objects.Length)];
            Vector3 spawnPos = hit.point + Vector3.up * 0.5f;
            Instantiate(randomItem, spawnPos, Quaternion.identity);
        }
    }
}
