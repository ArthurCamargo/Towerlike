using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator: MonoBehaviour
{

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform basePrefab;
    public Vector2 mapSize;
    
    
    [Range(0, 1)]
    public float outlinePercent;
    [Range(0, 1)]
    public float obstaclePercent;
    
    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    
    public int seed = 123;
    
    Coord baseCoord;


    private void Start() {
        GenerateMap();
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord> ();
        
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));


        
        string holderName = "Generated Map";
        if (transform.Find (holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        
        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;
        

        

        //Spawning Tiles
        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right *90)) as Transform;
                newTile.localScale = Vector3.one  * (1 - outlinePercent); 
                newTile.parent = mapHolder; 
            }
        }
        

        //Spawning the Base
        baseCoord = GetRandomCoord();
        Vector3 basePosition = CoordToPosition(baseCoord.x, baseCoord.y);
        Transform newBase = Instantiate(basePrefab, basePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
        newBase.parent = mapHolder;
        
        

        //Map of bool to see if there is a path from the spawner to the base
        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];
        
        int obstacleCount = (int) (mapSize.x * mapSize.y * obstaclePercent);
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i ++) {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount ++;
            
            if (randomCoord != baseCoord && MapisFullyAccessible(obstacleMap, currentObstacleCount)) {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }
    }
    
    bool MapisFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord> ();
        queue.Enqueue(baseCoord);
        mapFlags [baseCoord.x, baseCoord.y] = true;
        
        int accessibleTileCount = 1;
        
        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();
            
            for(int x = -1; x <= 1; x ++) {
                for(int y = -1; y <= 1; y ++) {
                    int neightbourX = tile.x + x;
                    int neightbourY = tile.y + y;
                    if(x == 0 || y == 0) {
                        if(neightbourX >= 0 && neightbourX < obstacleMap.GetLength(0)
                            && neightbourY >=0 && neightbourY < obstacleMap.GetLength(1))
                        {
                            if(!mapFlags[neightbourX,neightbourY] && !obstacleMap[neightbourX,neightbourY]) {
                                mapFlags[neightbourX, neightbourY] = true;
                                queue.Enqueue(new Coord(neightbourX, neightbourY));
                                accessibleTileCount ++;
                            }

                        }
                    }
                }
            }
        }
        
        int targetAccessibleTileCount = (int) (mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }
    

    
    
    Vector3 CoordToPosition (int x, int y) {
        return new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
    }
    
    public Coord GetRandomCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue ();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }
    

    public struct Coord {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        
        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }
        
    }
}
