using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float panBorderThickness = 400f;

    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    private Vector2 scrollLimit;

    public MapGenerator mapGen;

    public Vector2 MapSize;
    public float TileSize;


    void Start () { 
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();

        MapSize.x = mapGen.maps[mapGen.mapIndex].mapSize.x;
        MapSize.y = mapGen.maps[mapGen.mapIndex].mapSize.y;

        TileSize = mapGen.tileSize;


        panLimit.x = MapSize.x * TileSize * 0.25f;
        panLimit.y = MapSize.y * TileSize * 0.25f;


        scrollLimit.x =  10f;
        scrollLimit.y =  Mathf.Max(MapSize.x, MapSize.y) * TileSize * 1.5f;

       
        transform.position  = new Vector3(0, Mathf.Max(MapSize.x, MapSize.y) * TileSize, 0);
    }


    void Update() { 

        Vector3 pos = transform.position;
    
        if (Input.GetKey("w") ) { //|| Input.mousePosition.y >= Screen.height - panBorderThickness) {
            pos.z += panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) {
            pos.z -= panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) {
            pos.x += panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) {
            pos.x -= panSpeed * Time.unscaledDeltaTime;
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.unscaledDeltaTime;


        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, scrollLimit.x, scrollLimit.y);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }







}
