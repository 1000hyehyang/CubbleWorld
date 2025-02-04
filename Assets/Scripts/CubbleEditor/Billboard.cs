using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera cam;
    private void Start() {
        cam = Camera.main;
        transform.forward = cam.transform.forward;
    }

    private void Update() {
        transform.forward = cam.transform.forward;
    }
}
