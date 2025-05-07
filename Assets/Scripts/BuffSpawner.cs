using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject spawnMarker;
    public List<GameObject> buffBubbles;
    private List<GameObject> spawnedBubbles = new List<GameObject>();
    void Start()
    {
        SpawnBuffBubbles();
    }

    public void SpawnBuffBubbles()
    {
        //Debug.Log("Spawn Buff BubbleSSSSSS");
        Vector2 spawnLocation = new (transform.position.x, transform.position.y);
        StartCoroutine(SpawnBuffBubble(spawnLocation));
        spawnLocation.x += 5;
        StartCoroutine(SpawnBuffBubble(spawnLocation));
        spawnLocation.x -= 10;
        StartCoroutine(SpawnBuffBubble(spawnLocation));
    }

    IEnumerator SpawnBuffBubble(Vector2 spawnLocation)
    {

        GameObject marker = Instantiate(spawnMarker, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Destroy(marker);

        GameObject buffBubble = Instantiate(buffBubbles[UnityEngine.Random.Range(0, buffBubbles.Count)], spawnLocation, Quaternion.identity);
        buffBubble.GetComponent<BuffBubble>().buffSpawner = GetComponent<BuffSpawner>();
        spawnedBubbles.Add(buffBubble);
    }

    public void DestoyBuffBubbles()
    {
        foreach(GameObject bubble in spawnedBubbles)
        {
            if (bubble)
                Destroy(bubble);
        }
        spawnedBubbles.Clear();
        enemySpawner.NextWave();
    }
}
