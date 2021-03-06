﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject holding;
    public GameObject holdingEffect;
    public Transform raycastPoint;

    void Start() {
        holding = null;
    }

    void Update() {
        CheckThrow();
    }

    public GameObject GetHoldedObject(){
        return holding;
    }
    
    public bool Hold(GameObject gameObject){
        if(holding == null){
            holding = gameObject;
            CreateHoldingEffect();
            return true;
        }
        return false;
    }

    public bool Release(){
        if(holding != null){
            holding = null;
            return true;
        }
        return false;
    }

    public bool Throw(){
        if(holding != null){
            RaycastHit hit;
            if (Physics.Raycast(raycastPoint.position, raycastPoint.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                if(hit.transform == holding.transform){
                    return false;
                }
                if(hit.collider.tag == "Projectile"){
                    holding.GetComponentInChildren<ObjectController>().Release();
                    return false;
                }
                if(holding.GetComponentInChildren<ObjectController>().Trigger(hit.point)){
                    holding = null;
                    return true;
                }
                return false;
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

    private void CreateHoldingEffect(){
        GameObject effect = Instantiate(holdingEffect, holding.transform.position, Quaternion.identity);
        holding.GetComponentInChildren<ObjectController>().AssignVFXObject(effect);
        effect.transform.SetParent(holding.transform);
    }

    private void CheckThrow(){
        if (Input.GetMouseButtonDown(0))
        {
            Throw();
        }
    }
    
}
