using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float panSpeed = 20f;
    public Vector2 panLimit;
    public float scrollSpeed = 20f;
    private Vector2 scrollLimit;
    public MapGenerator mapGen;
    public Vector2 MapSize;
    public float TileSize;


    public Canvas objectViewCanvas; 
    public Camera cameraTop;
    public Camera cameraIso;

    public void CameraSwap() {
        
        cameraTop.enabled = !cameraTop.enabled;
        cameraIso.enabled = !cameraIso.enabled;
        objectViewCanvas.worldCamera = Camera.main; 
    
    }

    void Start() {

        cameraIso.enabled = true;
        cameraTop.enabled = false;

        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

        MapSize.x = mapGen.maps[mapGen.mapIndex].mapSize.x;
        MapSize.y = mapGen.maps[mapGen.mapIndex].mapSize.y;

        TileSize = mapGen.tileSize;

        panLimit.x = MapSize.x * TileSize * 0.25f;
        panLimit.y = MapSize.y * TileSize * 0.25f;

        scrollLimit.x = 10f;
        scrollLimit.y = Mathf.Max(MapSize.x, MapSize.y) * TileSize * 1.5f;

        Camera.main.transform.position = new Vector3(0, Mathf.Max(MapSize.x, MapSize.y) * TileSize * 0.75f, 0);
    }


    void Update() {

        Vector3 pos = Camera.main.transform.position;

        if(Input.GetKey("w")) {
            pos.z += panSpeed * Time.unscaledDeltaTime;
        }

        if(Input.GetKey("s")) {
            pos.z -= panSpeed * Time.unscaledDeltaTime;
        }

        if(Input.GetKey("d")) {
            pos.x += panSpeed * Time.unscaledDeltaTime;
        }

        if(Input.GetKey("a")) {
            pos.x -= panSpeed * Time.unscaledDeltaTime;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * scrollSpeed * 100f * Time.unscaledDeltaTime;


       // pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
       // pos.y = Mathf.Clamp(pos.y, scrollLimit.x, scrollLimit.y);
       /// pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        Camera.main.transform.position = pos;
    }







}
