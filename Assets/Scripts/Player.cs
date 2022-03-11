using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer), typeof(Transform), typeof(CameraController))]
public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Player found!");
            return;
        }
        instance = this;
    }

    #endregion

    public Color highlightColor = Color.yellow;
    
    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera playerCamera;
    private CameraController controller;

    private bool selectObstacleOn;
    private Item currentItem;
    
    public float cameraSpeed = 50f;
    public Tower currentTower;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        controller = playerCamera.GetComponent<CameraController>();
        selectObstacleOn = false;
    }
    
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * cameraSpeed;
        controller.Move(moveVelocity);
    
        if(selectObstacleOn)
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
                EndSelectObstacle();
                if (hitObject != null)
                {
                    hitObjectMaterial.color = initialColor;
                    hitObject = null;
                    hitObjectMaterial = null;
                }

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

    public void StartSelectObstacle(Item usedItem) {
        currentItem = usedItem;
        if (!selectObstacleOn)
            selectObstacleOn = true;

    }

    public void EndSelectObstacle()
    {
        Inventory.instance.Remove(currentItem);
        if (selectObstacleOn)
            selectObstacleOn = false;

    }
}
