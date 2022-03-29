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
    public float cameraSpeed = 50f;
    public Tower defautTower;

    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera playerCamera;
    private CameraController controller;
    private bool isHoldingItem = false;
    private string itemTargetTag;
    private Item holdingItem;
    

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

        if(holdingItem) {
            ControlHoldingItem();
        }
    }

    void ControlHoldingItem() {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag(itemTargetTag)) {

            if(hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;

            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;

            if(Input.GetMouseButtonDown(0)) {
                if(holdingItem.GetType() == typeof(TowerItem)) {
                    PlaceTower(defautTower, hitObject.GetComponent<Transform>());
                }
                else if(holdingItem.GetType() == typeof(SocketItem)) {
                    EquipItemOnTower(hitObject.GetComponentInParent<Tower>());
                }
                else if(holdingItem.GetType() == typeof(CombatItem)) {
                    ChangeTowerClass(hitObject.GetComponentInParent<Tower>());
                }

                if(hitObject != null) {
                    hitObjectMaterial.color = initialColor;
                    hitObject = null;
                    hitObjectMaterial = null;
                }

                EndHoldingItem();
            }
        }
        else if(hitObject != null) {
            hitObjectMaterial.color = initialColor;
            hitObject = null;
            hitObjectMaterial = null;
        }
    }

    void EquipItemOnTower(Tower targetTower) {
        targetTower.EquipItem(holdingItem);
        Inventory.instance.Remove(holdingItem);
    }

    void ChangeTowerClass(Tower targetTower) {
        Instantiate((holdingItem as CombatItem).towerPrefab, targetTower.transform.position, Quaternion.identity);
        Destroy(targetTower.gameObject);
        Inventory.instance.Remove(holdingItem);
    }

    void PlaceTower(Tower t, Transform obj) {
        //TODO: See if the tower already is on this object
        Transform newTower = Instantiate(t.towerPrefab, obj.position + Vector3.up * obj.localScale.y/2f + Vector3.up*t.towerPrefab.localScale.y, Quaternion.identity) as Transform;
        Inventory.instance.Remove(holdingItem);
    }

    public void StartHoldingItem(Item selectedItem) {
        if (!isHoldingItem) {
            holdingItem = selectedItem;
            isHoldingItem = true;
            if(holdingItem.GetType() == typeof(TowerItem)) {
                itemTargetTag = "Obstacle";
            }
            else if(holdingItem.GetType() == typeof(SocketItem)) {
                itemTargetTag = "Tower";
            }
            else if(holdingItem.GetType() == typeof(CombatItem)) {
                itemTargetTag = "Tower";
            }
        }
    }

    public void EndHoldingItem() {
        if(isHoldingItem) {
            isHoldingItem = false;
        }
    }
}