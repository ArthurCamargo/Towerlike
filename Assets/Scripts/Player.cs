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

    private bool selectObstacleOn = false;
    private bool equipingItemOn = false;
    private bool changingClassOn = false;
    private Item currentItem;
    private CombatItem currentCombatItem;
    public float cameraSpeed = 50f;
    public Tower defautTower;

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
        if(equipingItemOn)
            SelectTowerAndEquip();
        if(changingClassOn)
            SelectTowerAndChangeClass();
    }
    void SelectTowerAndChangeClass() {
        Tower currentTower;
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Tower")) {

            if( hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;

            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;
            
            if(Input.GetMouseButtonDown(0))
            {
                currentTower = hitObject.GetComponentInParent<Tower>();
                InstantiateFromItem(currentCombatItem, currentTower.transform);
                EndChangingClass(currentCombatItem);
                Destroy(currentTower.gameObject);
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
    void InstantiateFromItem(CombatItem usedItem, Transform transform)
    {
        Instantiate(usedItem.towerPrefab, transform.position, Quaternion.identity);
    }
    
    void SelectTowerAndEquip() {
        Tower currentTower;
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Tower")) {

            if( hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;

            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;
            
            if(Input.GetMouseButtonDown(0))
            {
                currentTower = hitObject.GetComponentInParent<Tower>();
                currentTower.EquipItem(currentItem);
                EndEquipingItem(currentItem);
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
                PlaceTower(defautTower, hitObject.GetComponent<Transform>());
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

    public void EndSelectObstacle() {
        Inventory.instance.Remove(currentItem);
        if (selectObstacleOn)
            selectObstacleOn = false;
    }
    public void StartEquipingItem(Item usedItem) {
        currentItem = usedItem;
        if (!equipingItemOn)
            equipingItemOn = true;
    }

    public void EndEquipingItem(Item usedItem) {
        Inventory.instance.Remove(currentItem);
        if (equipingItemOn)
            equipingItemOn = false;
    }
    public void StartChangingClass(CombatItem usedItem) {
        currentCombatItem = usedItem;
        if (!changingClassOn)
            changingClassOn = true;
    }

    public void EndChangingClass(Item usedItem) {
        Inventory.instance.Remove(currentItem);
        if (changingClassOn)
            changingClassOn = false;
    }
}