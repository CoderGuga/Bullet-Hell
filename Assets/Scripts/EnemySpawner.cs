using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.Mathematics;
using System;

public class EnemySpawner : MonoBehaviour
{
    public BuffSpawner buffSpawner;
    public GameObject player, spawnMarker, flyingBossPrefab;

    public List<GameObject> enemyList, enemyPrefabs, layoutList;
    public int spawnWeightPerWave, livingEnemyWeightUntilSpawn, iteration, currentSpawnWeight;
    public bool canSpawnEnemies;

    int currentSpawnNum;

    public void NextWave()
    { 
        iteration ++;
        if (iteration == 4)
            SpawnFlyingBoss();
        else
            SpawnWave();
        spawnWeightPerWave += iteration;
    }

    public void CheckWave()
    {
        if (currentSpawnNum < 3 && currentSpawnWeight <= livingEnemyWeightUntilSpawn)
            SpawnWave();
        else if (currentSpawnNum >= 3 && currentSpawnWeight == 0)
            EndWave();
    }

    public void EndWave()
    {
        if (iteration < layoutList.Count)
        {
            Debug.Log("EndWave");
            layoutList[iteration-1].SetActive(false);
            layoutList[iteration].SetActive(true);
            buffSpawner.SpawnBuffBubbles();
        }
        else
            Debug.Log("Victoryyyyyyyyyyyyyyyyyyyyy!!!");
    }

    public void SpawnWave()
    {
            while (currentSpawnWeight < spawnWeightPerWave)
            {
                StartCoroutine(SpawnEnemy());
            }
            currentSpawnNum ++;
    }

    public void SpawnFlyingBoss()
    {
        Vector2 bossSpawn = new Vector2(transform.position.x, transform.position.y + 5);
        GameObject flyingBoss = Instantiate(flyingBossPrefab, bossSpawn, Quaternion.identity); 
        flyingBoss.GetComponent<EnemyMovement>().target = player;
        flyingBoss.GetComponent<EnemyHealth>().enemySpawner = GetComponent<EnemySpawner>();
        flyingBoss.GetComponent<FlyingBoss>().player = player;
        flyingBoss.GetComponent<FlyingBoss>().enemySpawner = GetComponent<EnemySpawner>();
    }

    public IEnumerator SpawnEnemy()
    {
        //Debug.Log("Enemy spawned");
        // Access the grid graph
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        // Find a random walkable node
        GraphNode randomNode = GetRandomWalkableNode(gridGraph);

        if (randomNode != null)
        {
            // Convert the node's position to world coordinates
            Vector3 spawnPosition = (Vector3)randomNode.position;
            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            currentSpawnWeight += enemyPrefab.GetComponent<EnemyHealth>().spawnWeight;
            
            GameObject marker = Instantiate(spawnMarker, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(marker);

            // Spawn the enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            ApplyGoodies(enemy);
        }
        else
        {
            Debug.LogWarning("No walkable node found!");
        }
    }

    private GraphNode GetRandomWalkableNode(GridGraph gridGraph)
    {
        // Get all nodes in the grid graph
        List<GraphNode> walkableNodes = new List<GraphNode>();

        gridGraph.GetNodes(node =>
        {
            if (node.Walkable)
            {
                walkableNodes.Add(node);
            }
        });

        // Select a random walkable node
        if (walkableNodes.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, walkableNodes.Count);
            return walkableNodes[randomIndex];
        }

        return null; // No walkable nodes found
    }


    public GameObject GetClosestEnemy(Transform trans)
    {
        float minDistance = math.INFINITY;
        GameObject minEnemy = player;
        foreach(GameObject enemy in enemyList)
        {
            float distance = Vector2.Distance(enemy.transform.position, trans.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                minEnemy = enemy;
            }
        }

        return minEnemy;
    }

    void ApplyGoodies(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyMovement>())
            enemy.GetComponent<EnemyMovement>().target = player;
        if (enemy.GetComponent<EnemyShooting>())
            enemy.GetComponent<EnemyShooting>().target = player;
        if (enemy.GetComponent<EnemyHealth>())
            enemy.GetComponent<EnemyHealth>().enemySpawner = GetComponent<EnemySpawner>();

        enemyList.Add(enemy);
    }
}