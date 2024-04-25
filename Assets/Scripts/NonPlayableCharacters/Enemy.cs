using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1.0f; 
    public List<Transform> waypoints; 
    private int waypointIndex = 0;
    private Boolean playerDetected = false;
    public Transform player;
    public float fieldOfView = 90f; 
    public float viewDistance = 10.0f;
    public LayerMask viewMask; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(player);
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[waypointIndex].position;
        }
    }

    void Update(){
    if (player == null || !player.gameObject.activeInHierarchy)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject potentialPlayer in players)
        {
            if (potentialPlayer.activeInHierarchy)
            {
                player = potentialPlayer.transform;
                break;
            }
        }
    }   
    playerDetected = PlayerInFieldOfView();
        if(!playerDetected){
            MoveEnemy();
        }
        else{
            ChasePlayer();
        }
    }
    bool PlayerInFieldOfView(){
        Vector3 directionToPlayer = transform.InverseTransformPoint(player.position);
        float angle = Vector3.Angle(-Vector3.right, directionToPlayer);

        Debug.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * -transform.right*viewDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * -transform.right*viewDistance, Color.red);

        if (angle < fieldOfView / 2  && directionToPlayer.magnitude < viewDistance)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(directionToPlayer), Color.green);
            return true;
        }
        return false;
    }

    void MoveEnemy(){
        if (waypoints.Count == 0) return;

        Vector3 direction = waypoints[waypointIndex].position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(0, 90, 0); 
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
        }
    }

    void ChasePlayer(){
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }
}