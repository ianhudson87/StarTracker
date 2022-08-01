using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereOfInfluence : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<PlayerController>() != null) {
            other.gameObject.GetComponent<PlayerController>().ghost.transform.parent = this.transform;
        }
        
        // if(other.gameObject.GetComponent<IStick)
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<PlayerController>() != null) {
            other.gameObject.GetComponent<PlayerController>().ghost.transform.parent = null;
        }
    }
}
