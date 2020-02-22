//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Pathfinding;
//using DG.Tweening;

//public class EnemyAI : MonoBehaviour
//{    
//    public Transform target;

//    public float speed = 10f;
//    [SerializeField]
//    float nextWaypointDistance = 3f;

//    Path currentPath;
//    int currentWaypoint = 0;
//    bool reachedEndOfPath = false;

//    Seeker seeker;
//    Rigidbody2D enemyRB;

//    // Start is called before the first frame update
//    void Start()
//    {
//        seeker = GetComponent<Seeker>();
//        enemyRB = GetComponent<Rigidbody2D>();
//        InvokeRepeating("UpdatePath", 0f, .5f);       
//    }

//    void UpdatePath()
//    {
//        if (seeker.IsDone())
//        {
//            seeker.StartPath(enemyRB.position, target.position, OnPathComplete);
//        }        
//    }

//    void OnPathComplete(Path path)
//    {
//        if (!path.error)
//        {
//            currentPath = path;
//            currentWaypoint = 0;
//        }
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        if (currentPath == null)
//        {
//            return;
//        }

//        if (currentWaypoint >= currentPath.vectorPath.Count)
//        {
//            reachedEndOfPath = true;
//            return;
//        }
//        else
//        {
//            reachedEndOfPath = false;
//        }

//        Vector2 directionToNextWaypoint = ((Vector2)currentPath.vectorPath[currentWaypoint] - enemyRB.position).normalized;
//        Vector2 moveDirection = directionToNextWaypoint * speed * Time.deltaTime;

//        enemyRB.DOMove(directionToNextWaypoint, speed).SetSpeedBased(true).SetEase(Ease.Linear);
//        float distanceToNextWaypoint = Vector2.Distance(enemyRB.position, currentPath.vectorPath[currentWaypoint]);

//        if (distanceToNextWaypoint < nextWaypointDistance)
//        {
//            currentWaypoint++;
//        }
//    }
//}
