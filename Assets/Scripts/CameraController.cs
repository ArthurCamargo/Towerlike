using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    
    public MapGenerator map;
    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 0.5f;
    public float maxZoom = 1.5f;
    public float pitch = 2f;
    public float currentZoom = 10f;
    public Vector3 velocity;
    void Start()
    {
        offset = transform.position - target.transform.position;
        //maxZoom = map.currentMap.mapSize.y * map.tileSize / (20f * 60f);
    }
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }
    void LateUpdate()
    {
        offset += velocity * Time.deltaTime;
        transform.position = target.transform.position + offset * currentZoom;
        if(tag != "MainCamera")
            transform.LookAt(target.position + Vector3.up * pitch);
    }
}
