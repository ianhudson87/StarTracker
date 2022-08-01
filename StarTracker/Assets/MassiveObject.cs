using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassiveObject : MonoBehaviour
{
    [SerializeField] public float mass;
    [SerializeField] protected Vector3 velocity;
    [SerializeField] public float angularVelocity; // degrees per second

    void Update() {
        Gravity();
        Rotate();
        transform.position += velocity * Time.deltaTime;
    }

    protected void Gravity() {
        foreach(MassiveObject mo in FindObjectsOfType<MassiveObject>()) {
            if(mo == this) {
                continue;
            }

            float sqrDist = (mo.transform.position - transform.position).sqrMagnitude;
            float gravityMagnitude = mo.mass * this.mass * Constants.G / sqrDist;
            Vector3 gravityDirection = (mo.transform.position - transform.position).normalized;
            ApplyForce(gravityDirection * gravityMagnitude);
        }

    }

    void Rotate() {
        transform.Rotate(Vector3.up, angularVelocity * Time.deltaTime);
    }

    void ApplyForce(Vector3 force) {
        velocity += force * Time.deltaTime / this.mass;
    }
}
