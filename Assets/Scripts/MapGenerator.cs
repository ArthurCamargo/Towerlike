using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator: MonoBehaviour
{
    public Map[] maps;
    public int mapIndex;
    public Vector2 maxMapSize;

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform basePrefab;
    public Transform enemySpawnerPrefab;
    public Transform navmeshMaskPrefab;
    public Transform navmeshFloor;
    


    [Range(0, 1)]
    public float outlinePercent;
    
    public float tileSize;
    
    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    
    Map currentMap;
    
    private void Start() {
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];

        System.Random prng = new System.Random (currentMap.seed);

        allTileCoords = new List<Coord> ();
        
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), currentMap.seed));


        
        string holderName = "Generated Map";
        if (transform.Find (holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        
        Transform mapHolder = new GameObject (holderName).transform;
        mapHolder.parent = transform;
        

        

        //Spawning Tiles
        for (int x = 0; x < currentMap.mapSize.x; x++) {
            for (int y = 0; y < currentMap.mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right *90)) as Transform;
                newTile.localScale = Vector3.one  * (1 - outlinePercent) * tileSize; 
                newTile.parent = mapHolder; 
            }
        }
        

        //Spawning the Base
        currentMap.baseCoord = GetRandomCoord();
        Vector3 basePosition = CoordToPosition(currentMap.baseCoord.x, currentMap.baseCoord.y);
        Transform newBase = Instantiate(basePrefab, basePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
        newBase.parent = mapHolder;
        

        //Spawning the EnemySpawner
        currentMap.enemySpawnerCoord = GetRandomCoord();
        Vector3 enemySpawnerPosition = CoordToPosition(currentMap.enemySpawnerCoord.x, currentMap.enemySpawnerCoord.y);
        Transform newEnemySpawner = Instantiate(enemySpawnerPrefab, enemySpawnerPosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
        newEnemySpawner.parent = mapHolder; 
        

        //Map of bool to see if there is a path from the spawner to the base
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
        
        int obstacleCount = (int) (currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
        int currentObstacleCount = 0;
        
        Dictionary<Coord, Vector2> vectorField = generateVectorField();
        printVectorField(vectorField);
        bool[,] viablePath = generateViablePath(vectorField);

        for (int i = 0; i < obstacleCount; i ++) {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount ++;
            
            if (randomCoord != currentMap.baseCoord
                && MapisFullyAccessible(obstacleMap, currentObstacleCount)
                //&& !isInViablePath(randomCoord, viablePath)
                && randomCoord != currentMap.enemySpawnerCoord) {

                float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight, currentMap.maxObstacleHeight, (float) prng.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight/2, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize); 
                
                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
                float colorPercent = randomCoord.y / (float)currentMap.mapSize.y;
                obstacleMaterial.color = Color.Lerp(currentMap.foregroundColor, currentMap.backgroundColor, colorPercent);
                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        Transform maskLeft = Instantiate(navmeshMaskPrefab, Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        Transform maskRight = Instantiate(navmeshMaskPrefab, Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4f * tileSize, Quaternion.identity) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale = new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2f, 1, currentMap.mapSize.y) * tileSize;

        Transform maskTop = Instantiate(navmeshMaskPrefab, Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2) * tileSize;

        Transform maskBottom = Instantiate(navmeshMaskPrefab, Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4f * tileSize, Quaternion.identity) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale = new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2) * tileSize;

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }
    
    //Generate a random path from the base to the spawner
    Dictionary<Coord,Vector2> generateVectorField()
    {
        bool[,] mapFlags = new bool[currentMap.mapSize.x, currentMap.mapSize.y];
        Queue<Coord> queue = new Queue<Coord> ();
        Dictionary<Coord,Vector2> vectorField = new Dictionary<Coord, Vector2>();

        Coord currentLocation = currentMap.baseCoord;
        vectorField.Add(currentLocation, Vector2.zero);
        mapFlags[currentLocation.x, currentLocation.y] = true;
        
        queue.Enqueue(currentLocation);
        
        Vector2[] movePossible = new Vector2[4];

        movePossible[0] = Vector2.up;
        movePossible[1] = Vector2.down;
        movePossible[2] = Vector2.left;
        movePossible[3] = Vector2.right;

        while (queue.Count > 0) {
            currentLocation = queue.Dequeue();
            for(int i = 0; i < movePossible.Length; i ++)
            {
                Coord nextNeighbor = currentLocation;
                nextNeighbor.AddVector(movePossible[i]);
                /*
                Debug.Log(currentLocation.x);
                Debug.Log(currentLocation.y);
                Debug.Log(movePossible[i]);
                Debug.Log(nextNeighbor.x);
                Debug.Log(nextNeighbor.y);
                */
                if(nextNeighbor.x >= 0 && nextNeighbor.x < currentMap.mapSize.x
                    && nextNeighbor.y >= 0 && nextNeighbor.y < currentMap.mapSize.y) {
                    if(!mapFlags[nextNeighbor.x, nextNeighbor.y]) {
                        mapFlags[nextNeighbor.x, nextNeighbor.y] = true;
                        queue.Enqueue(nextNeighbor);
                        vectorField.Add(nextNeighbor, -movePossible[i]);
                    }
                }
            }
        }
        return vectorField;
    }
    
    void printVectorField(Dictionary<Coord, Vector2> vectorField)
    {
        foreach(var item in vectorField)
        {
            Debug.Log("Coord x:");
            Debug.Log(item.Key.x);
            Debug.Log("Coord y:");
            Debug.Log(item.Key.y);
            Debug.Log(item.Value);
        }
    }
    
    bool[,] generateViablePath(Dictionary<Coord, Vector2>vectorField)
    {
        bool[,] path = new bool[currentMap.mapSize.x, currentMap.mapSize.y];

        Coord currentLocation = currentMap.enemySpawnerCoord;
        path[currentLocation.x, currentLocation.y] = true;
        while(currentLocation != currentMap.baseCoord)
        {
            Debug.Log(currentLocation.x);
            Debug.Log(currentLocation.y);

            currentLocation.AddVector(vectorField[currentLocation]);
            path[currentLocation.x, currentLocation.y] = true;
        }
        
        return path;
    }
    
    bool isInViablePath(Coord _coord, bool[,] path)
    {
        return path[_coord.x, _coord.y];
    }

    bool MapisFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord> ();
        queue.Enqueue(currentMap.baseCoord);
        mapFlags [currentMap.baseCoord.x, currentMap.baseCoord.y] = true;
        
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
        
        int targetAccessibleTileCount = (int) (currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }
    

    
    
    Vector3 CoordToPosition (int x, int y) {
        return new Vector3(-currentMap.mapSize.x/2f + 0.5f + x, 0, -currentMap.mapSize.y/2f + 0.5f + y) * tileSize;
    }
    
    public Coord GetRandomCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue ();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }
    
    [System.Serializable]
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

        public void AddVector(Vector2 v1) {
            this.x +=  (int) v1.x;
            this.y +=  (int) v1.y;
        }
        
        public override int GetHashCode() {
            unchecked
            {
                int hash = (int) 2166136261;
                // Suitable nullity checks etc, of course :)
                hash = (hash * 16777619) ^ x.GetHashCode();
                hash = (hash * 16777619) ^ y.GetHashCode();
                return hash;
            }       
        }
    }
    
    [System.Serializable]
    public class Map {
        public Coord mapSize;

        [Range(0,1)]
        public float obstaclePercent;
        public int seed;

        public float minObstacleHeight;
        public float maxObstacleHeight;

        public Color foregroundColor;
        public Color backgroundColor;

        public Coord baseCoord;
        public Coord enemySpawnerCoord;
    }
}
