using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarTracker : MonoBehaviour
{
    [SerializeField] GameObject hinge, screw, cameraBase, screwBase;
    // private Vector3 pivotPoint; // pivot about the (local) y-axis wrt to the hinge
    private float leg; // distance from pivot to screw
    // private float height; // distance the screw has pushed through the base
    private float cameraBaseRot = 0; // in degrees
    CelestialBody closestCB = null;

    void Awake() {
        // pivotPoint = hinge.transform.position;
        
        // get leg length
        Vector3 screwBaseNormal = screwBase.transform.forward.normalized;
        float screwBaseToScrewDist = Vector3.Dot(screwBaseNormal, screw.transform.position - screwBase.transform.position);
        Vector3 screwProjOntoScrewBase = screw.transform.position - (screwBaseNormal * screwBaseToScrewDist);
        leg = (screwProjOntoScrewBase - hinge.transform.position).magnitude;
        Debug.Log("leg" + leg);
        // leg = 

        PolarAlign();
    }

    void Update() {
        // Debug.Log("height" + GetScrewHeight());
        float screwHeight = GetScrewHeight();
        // Debug.Log("height" + height);
        float desiredcameraBaseRot = Mathf.Atan(screwHeight / leg) / Mathf.PI * 180f;
        cameraBase.transform.RotateAround(hinge.transform.position, -transform.up, desiredcameraBaseRot - cameraBaseRot);
        cameraBaseRot = desiredcameraBaseRot;
        // Debug.Log("desired angle" + cameraBaseRot);

        // Debug.Log("Screw Height update" + GetScrewHeight());

        if(Input.GetKeyDown(KeyCode.T)) {
            StartTrack();
        }
    }

    float GetScrewHeight() {
        // TODO: this is terrible, why did i do it this way?
        Vector3 screwWRtHinge = hinge.transform.InverseTransformPoint(screw.transform.position);
        float desiredHeight = screwWRtHinge.z + (screw.transform.lossyScale.y);
        return Mathf.Clamp(desiredHeight, 0, screw.transform.lossyScale.y*2);
    }
    
    void SetScrewHeight(float desiredHeight) {
        Vector3 screwWRtHinge = hinge.transform.InverseTransformPoint(screw.transform.position);
        screwWRtHinge.z = desiredHeight - (screw.transform.lossyScale.y);
        screw.transform.position = hinge.transform.TransformPoint(screwWRtHinge);
    }

    void PolarAlign() {
        float closestDist = -1f;
        foreach(CelestialBody cb in FindObjectsOfType<CelestialBody>()) {
            float dist = (cb.transform.position - transform.position).magnitude;
            if(dist < closestDist || closestDist == -1f) {
                closestCB = cb;
                closestDist = dist;
            }
        }

        Quaternion q = Quaternion.identity;
        if(closestCB.GetComponent<MassiveObject>().angularVelocity > 0) {
            q = Quaternion.FromToRotation(transform.up, closestCB.transform.up);
        }
        else{
            q = Quaternion.FromToRotation(transform.up, -closestCB.transform.up);
        }
        
        transform.rotation *= q;
        // Quaternion.FromToRotation()
    }

    void StartTrack() {
        StartCoroutine("StartTrackCoroutine");
    }

    IEnumerator StartTrackCoroutine() {
        float elapsedTime = 0;
        float rotateSpeed = Mathf.Abs(closestCB.GetComponent<MassiveObject>().angularVelocity / 180f * Mathf.PI);

        while(GetScrewHeight() < screw.transform.lossyScale.y*2) {
            SetScrewHeight(leg * Mathf.Tan(rotateSpeed * elapsedTime));

            elapsedTime += Time.deltaTime;
            Debug.Log("elapsed time" + elapsedTime);
            Debug.Log("screwn height" + leg * Mathf.Tan(rotateSpeed * elapsedTime));
            yield return null;
        }

        yield return null;
    }

}
