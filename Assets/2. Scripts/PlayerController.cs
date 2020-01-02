using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject holding;
    public Transform raycastPoint;
    void Start() {
        this.holding = null;    
    }

    void Update() {
        checkRelease();
    }

    public GameObject GetHoldedObject(){
        return this.holding;
    }
    
    public bool Hold(GameObject gameObject){
        if(this.holding == null){
            this.holding = gameObject;
            return true;
        }
        return false;
    }

    public bool Release(){
        if(this.holding != null){
            RaycastHit hit;
            if (Physics.Raycast(raycastPoint.position, raycastPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                this.holding.GetComponent<ObjectController>().Trigger(hit.point);
                this.holding = null;
                return true;
            }
            else
            {
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                // Debug.Log("Did not Hit");
                return false;   
            }
        }
        return false;
    }

    // private void GetShootPosition(){
    //     RaycastHit hitPoint;
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
    //     if(Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
    //     {
    //     if(hitPoint.collider.tag == "Ground")
    //     {
    //     Debug.Log("Hit ground"); 
    //     }

    //     if(hitPoint.collider.tag == "Object")
    //     {
    //     Debug.Log("Hit object"); 
    //     }
    //     }
    //     else
    //     {
    //     Debug.Log ("No collider hit"); 
    //     }
    // }

    private void checkRelease(){
        if (Input.GetMouseButtonDown(0))
        {
            Release();
        }
    }
    
}
