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
    public GameObject player, spawnMarker;

    public List<GameObject> enemyList, enemyPrefabs;
    public int spawnsPerWave, livingEnemiesUntilSpawn;
    public bool canSpawnEnemies;

    public void NextWave()
    { 
        SpawnWave();
        spawnsPerWave ++;
    }

    public void EndWave()
    {
        Debug.Log("EndWave");
        buffSpawner.SpawnBuffBubbles();
    }

    public void SpawnWave()
    {
            for (int i = 0; i < spawnsPerWave; i++)
            {
                StartCoroutine(SpawnEnemy());
            }
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
            
            GameObject marker = Instantiate(spawnMarker, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(marker);

            GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            // Spawn the enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            if (enemy.GetComponent<EnemyMovement>())
            enemy.GetComponent<EnemyMovement>().target = player;
            if (enemy.GetComponent<EnemyShooting>())
            enemy.GetComponent<EnemyShooting>().target = player;
            if (enemy.GetComponent<EnemyHealth>())
            enemy.GetComponent<EnemyHealth>().enemySpawner = GetComponent<EnemySpawner>();

            enemyList.Add(enemy);
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
}