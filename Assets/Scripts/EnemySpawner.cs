using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using JetBrains.Annotations;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab, player;

    public List<GameObject> enemyList;

    void Start()
    {
        SpawnWave(5);
    }

    public void SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Debug.Log("Enemy spawned");
        // Access the grid graph
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        // Find a random walkable node
        GraphNode randomNode = GetRandomWalkableNode(gridGraph);

        if (randomNode != null)
        {
            // Convert the node's position to world coordinates
            Vector3 spawnPosition = (Vector3)randomNode.position;

            // Spawn the enemy at the random position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            if (enemy.GetComponent<EnemyMovement>())
            enemy.GetComponent<EnemyMovement>().target = player;
            if (enemy.GetComponent<EnemyShooting>())
            enemy.GetComponent<EnemyShooting>().target = player;

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
            int randomIndex = Random.Range(0, walkableNodes.Count);
            return walkableNodes[randomIndex];
        }

        return null; // No walkable nodes found
    }
}