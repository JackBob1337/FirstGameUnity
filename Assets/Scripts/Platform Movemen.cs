using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovemen : MonoBehaviour
{
   [SerializeField] private GameObject[] waypoints;
   private int currentWaypoinIndex = 0;
   [SerializeField] private float speed = 2f;

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypoinIndex].transform.position, transform.position) < .1f)
        {
            currentWaypoinIndex++;
            if (currentWaypoinIndex >= waypoints.Length)
            {
                currentWaypoinIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoinIndex].transform.position, Time.deltaTime * speed);
    }
}
