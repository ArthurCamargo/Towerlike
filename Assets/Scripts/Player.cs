using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Renderer), typeof(Transform), typeof(CameraController))]
public class Player : MonoBehaviour {
    #region Singleton
    public static Player instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Player found!");
            return;
        }
        instance = this;
    }

    #endregion

    public Color highlightColor = Color.yellow;
    public float cameraSpeed = 50f;
    public Tower defautTower;

    public FreeState freeState = new FreeState();
    public BuildingState buildingState = new BuildingState();
    public ChangingCombatState changingCombatState = new ChangingCombatState();
    public EquippingItemState equippingItemState = new EquippingItemState();

    public PlayerState currentState;
    public Item holdingItem;

    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera playerCamera;
    private CameraController controller;



    // Start is called before the first frame update
    void Start() {
        playerCamera = Camera.main;
        controller = playerCamera.GetComponent<CameraController>();
        currentState = freeState;
    }

    void Update() {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * cameraSpeed;
        controller.Move(moveVelocity);

        if(EventSystem.current.IsPointerOverGameObject())
            return;

        currentState = currentState.doAction(this);

    }



    public void ChangeTowerCombat(Tower oldTower) {
        Tower newTower;
        List<SocketItem> oldTowerItems = oldTower.equipedItems;

        newTower = Instantiate((holdingItem as CombatItem).towerPrefab, oldTower.transform.position, Quaternion.identity).GetComponent<Tower>();
        newTower.TransferTowerStats(oldTower);

        Destroy(oldTower.gameObject);
        Inventory.instance.Remove(holdingItem);
    }

    public void PlaceTower(Tower t, Transform obj) {
        //TODO: See if the tower already is on this object
        Transform newTower = Instantiate(t.towerPrefab, obj.position + Vector3.up * obj.localScale.y / 2f + Vector3.up * t.towerPrefab.localScale.y, Quaternion.identity) as Transform;
        Inventory.instance.Remove(holdingItem);
    }

    public Collider SelectObjectWithTag(string tag) {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag(tag)) {

            if(hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;

            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;

            if(Input.GetMouseButtonDown(0)) {
                if(hitObject != null) {
                    hitObjectMaterial.color = initialColor;
                    return hitObject;
                }
            }
        }
        else if(hitObject != null) {
            hitObjectMaterial.color = initialColor;
            hitObject = null;
            hitObjectMaterial = null;
        }

        return null;
    }

    /*
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
                    ChangeTowerCombat(hitObject.GetComponentInParent<Tower>());
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

    */
}