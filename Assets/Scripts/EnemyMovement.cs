using System.Collections;
using System.Collections.Generic;
using System.IO;
using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target;

    public float moveSpeed = 200f,
    nextWaypointDistance = 0.2f,
    pathUpdateTime = 0.5f,
    maxDistanceToPlayer = 0,
    minDistanceToPlayer = 0;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateTime);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
    }

    void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        Vector2 direction;
        if(path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count || Vector2.Distance(target.transform.position, transform.position) <= maxDistanceToPlayer)
        {
            if (Vector2.Distance(target.transform.position, transform.position) <= minDistanceToPlayer)
                direction = -((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            else{
                reachedEndOfPath = true;
                return;
            }
        }else
        {
            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            reachedEndOfPath = false;
        }

        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
