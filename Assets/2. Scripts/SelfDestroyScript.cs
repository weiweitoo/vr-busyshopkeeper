using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyScript : MonoBehaviour
{
    public void SelfDestroy(){
        Destroy(gameObject);
    }
}
