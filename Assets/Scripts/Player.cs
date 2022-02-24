using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public Transform[] towers;
    
    private Collider hitObject;
    private Color initialColor;
    private Material hitObjectMaterial;
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }
    
    void Update()
    {
        SelectObstacle();
    }
    
    void SelectObstacle() {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Obstacle")) {

            if( hitObject != null)
                hitObjectMaterial.color = initialColor;
            hitObject = hit.collider;


            hitObjectMaterial = hitObject.GetComponent<Renderer>().material;
            initialColor = hitObjectMaterial.color;
            hitObjectMaterial.color = highlightColor;
        }
        else if(hitObject != null)
        {
            hitObjectMaterial.color = initialColor;
            hitObject = null;
            hitObjectMaterial = null;
        }

    }

}
