using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public Transform target;
    public float speed = 5;
    private Vector2[] path;
    private int targetIndex;

    private void Start()
    {
        InvokeRepeating("RefreshPath", 0f, 0.25f);
    }

    private void RefreshPath()
    {
        if (target != null)
        {
            Vector2 targetPositionOld = (Vector2)target.position + Vector2.up; // ensure != to target.position initially

            if (targetPositionOld != (Vector2)target.position)
            {
                targetPositionOld = target.position;
                path = Pathfinding.RequestPath(transform.position, target.position);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
    }

    private IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            targetIndex = 0;
            Vector2 currentWaypoint = path[0];

            while (true)
            {
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                if (target != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                }
                yield return null;

            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * .5f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}