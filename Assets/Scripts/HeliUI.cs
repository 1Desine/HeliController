using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeliUI : MonoBehaviour {
    public static HeliUI Instance { get; private set; }

    [SerializeField] private Transform rollTransform;
    [SerializeField] private Transform heightTransform;


    private Transform heliTransform;

    private float heightMin;
    private float heightMax;

    private void Awake() {
        Instance = this;

        heightMin =  heightTransform.position.y;
        heightMax = -heightMin;
    }


    private void Update() {
        if(heliTransform == null) return;

        rollTransform.eulerAngles = Vector3.forward * heliTransform.right.y * -90;

        if(Physics.Raycast(heliTransform.position, Vector3.down, out RaycastHit hit, 100)) {
            heightTransform.position = Vector3.up * Mathf.Lerp(heightMin, heightMax, hit.distance / 100)
                + Vector3.right * 28f + Vector3.up * 340;
        }
    }

    public void SetHeliTransform(Transform heliTransform) {
        this.heliTransform = heliTransform;
    }




}
