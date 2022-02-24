using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Transform))]
public class Player : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public Tower[] towers;
    public int towerIndex;
    
    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera playerCamera;
    
    Tower currentTower;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        currentTower = towers[towerIndex];
    }
    
    void Update()
    {
        SelectObstacle();
    }
    
    void SelectObstacle() {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Obstacle")) {

            if( hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;


            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;
            
            if(Input.GetMouseButtonDown(0))
            {
                PlaceTower(currentTower, hitObject.GetComponent<Transform>());
            }
        }
        else if(hitObject != null)
        {
            hitObjectMaterial.color = initialColor;
            hitObject = null;
            hitObjectMaterial = null;
        }

    }
    
    void PlaceTower(Tower t, Transform obj) {
        Transform newTower = Instantiate(t.towerPrefab, obj.position + Vector3.up * obj.localScale.y/2f + Vector3.up*t.towerPrefab.localScale.y, Quaternion.identity) as Transform;
    }
}
