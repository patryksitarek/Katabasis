using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public GameObject map;
    public float offset = 9f;

    private float mapSize;

    private void Start()
    {
        mapSize = map.GetComponent<MeshCollider>().bounds.size.x / 2;
        transform.position = new Vector3(
            player.transform.position.x,
            transform.position.y,
            transform.position.z
        );
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        
        // Ensure the camera doesn't leave the map
        if (Math.Abs(newPosition.x) < mapSize - offset)
        {
            transform.position = newPosition;
        }
    }
}
