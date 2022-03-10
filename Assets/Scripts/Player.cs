using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer), typeof(Transform), typeof(CameraController))]
public class Player : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    
    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera playerCamera;
    private CameraController controller;
    
    public float cameraSpeed = 50f;
    public Tower currentTower;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        controller = playerCamera.GetComponent<CameraController>();
    }
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * cameraSpeed;
        controller.Move(moveVelocity);
    
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
        //TODO: See if the tower already is on this object
        Transform newTower = Instantiate(t.towerPrefab, obj.position + Vector3.up * obj.localScale.y/2f + Vector3.up*t.towerPrefab.localScale.y, Quaternion.identity) as Transform;
    }
}
