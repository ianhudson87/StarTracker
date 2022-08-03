using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    [SerializeField] float minDist, maxDist;
    [SerializeField] float starRadius;
    [SerializeField] int numStars;
    [SerializeField] GameObject starPrefab;

    void Awake() {
        for(int i = 0; i < numStars; i++) {
            float theta = Random.Range(0f, 2*Mathf.PI);
            float phi = Random.Range(-Mathf.PI/2, Mathf.PI/2);

            Vector3 direction = new Vector3(Mathf.Cos(theta), Mathf.Tan(phi), Mathf.Sin(theta)).normalized;

            float distance = Random.Range(minDist, maxDist);

            GameObject newStar = GameObject.Instantiate(starPrefab, transform);
            newStar.transform.position += direction * distance;
            newStar.transform.localScale *= starRadius;
        }
    }
}
