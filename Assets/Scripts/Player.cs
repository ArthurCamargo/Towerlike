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


    //States
    public FreeState freeState = new FreeState();
    public BuildingState buildingState = new BuildingState();
    public ChangingCombatState changingCombatState = new ChangingCombatState();
    public EquippingItemState equippingItemState = new EquippingItemState();


    //Cameras
    public CameraController cameraController;

    //Viewing Objects
    public ObjectViewUI objectViewUI;
    public GameObject currentObject;
    public SpeedController speedController;
    public PlayerState currentState;
    public Item holdingItem;
    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;


    // Start is called before the first frame update
    void Start() {
        currentState = freeState;
    }


    void handleInput() {
        if(Input.GetButtonDown("TooglePause")) {
            speedController.TooglePause();
        }
        
        if(Input.GetButtonDown("ToogleSpeed")) {
            speedController.Toogle3Times();
        }

        if(Input.GetButtonDown("CameraSwap")) {
           cameraController.CameraSwap();
        }
    }

    void Update() {

        if(EventSystem.current.IsPointerOverGameObject())
            return;
        
        handleInput();

        currentState = currentState.doAction(this);
    }



    public void ChangeTowerCombat(Tower oldTower) {
        Tower newTower;
        List<SocketItem> oldTowerItems = oldTower.equipedItems;

        newTower = Instantiate((holdingItem as CombatItem).towerPrefab, oldTower.transform.position, Quaternion.identity).GetComponent<Tower>();
        newTower.TransferTowerStats(oldTower);

        oldTower.BeforeDestroy();
        Destroy(oldTower.gameObject);
        Inventory.instance.Remove(holdingItem);
    }

    public void PlaceTower(Tower t, Transform obj) {
        Transform newTower = Instantiate(t.towerPrefab, obj.position + Vector3.up * obj.localScale.y / 2f + Vector3.up * t.towerPrefab.localScale.y, Quaternion.identity) as Transform;
        Inventory.instance.Remove(holdingItem);
    }

    public Collider SelectObjectWithTag(string tag) {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag(tag)) {

            //if(hitObject != null)
            //hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;

            //hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            //initialColor = hitObjectMaterial.color;
            //hitObjectMaterial.color = highlightColor;

            if(Input.GetMouseButtonDown(0)) {
                if(hitObject != null) {
                    //hitObjectMaterial.color = initialColor;
                    return hitObject;
                }
            }
        }
        else if(hitObject != null) {
            //hitObjectMaterial.color = initialColor;
            //hitObject = null;
            //hitObjectMaterial = null;
        }

        return null;
    }
}