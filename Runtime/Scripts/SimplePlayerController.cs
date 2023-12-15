using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour {
    public float speed = 5.0f;

    void Update() {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        transform.position += movement * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log("@SimplePlayerController.Save");
            StartCoroutine(SaveManager.instance.SaveGame());
        }
        else if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("@SimplePlayerController.Reset");
            StartCoroutine(SaveManager.instance.ResetGame());
        }
    }
}
