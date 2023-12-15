using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Aarthificial.Safekeeper;
using Aarthificial.Safekeeper.Stores;
using Aarthificial.Safekeeper.Attributes;

public class SavedTransform : MonoBehaviour, ISaveStore {
    private class StoredData {
        public Vector3 position;
        public Quaternion rotation;
    }

    [ObjectLocation]
    [SerializeField]
    private SaveLocation _location;
    private StoredData _data = new();

    public void OnEnable() {
        SaveStoreRegistry.Register(this);
    }

    public void OnDisable() {
        SaveStoreRegistry.Unregister(this);
    }

    // OnLoad will be invoked right after the scene is loaded.
    // Before `Start` but after `OnEnable`.
    public void OnLoad(SaveControllerBase save) {
        Debug.Log("@Transform.OnLoad.1");
        if (save.Data.Read(_location, _data)) {
            Debug.Log($"@Transform.OnLoad.2: {_data.position.x}, {_data.position.y}, {_data.position.z}");
            transform.position = _data.position;
            transform.rotation = _data.rotation;
        }
    }

    public void Awake() {
        Debug.Log($"@Transform.Awake.1: {_data.position.x}, {_data.position.y}, {_data.position.z}");
        Debug.Log($"@Transform.Awake.2: {transform.position.x}, {transform.position.y}, {transform.position.z}");
    }

    public void Start() {
        Debug.Log($"@Transform.Start.1: {_data.position.x}, {_data.position.y}, {_data.position.z}");
        Debug.Log($"@Transform.Start.2: {transform.position.x}, {transform.position.y}, {transform.position.z}");
    }

    // OnSave will be invoked right before the scene in unloaded or whenever
    // the game is saved.
    public void OnSave(SaveControllerBase save) {
        _data.position = transform.position;
        _data.rotation = transform.rotation;
        Debug.Log($"@Transform.OnSave: {_data.position.x}, {_data.position.y}, {_data.position.z}");
        save.Data.Write(_location, _data);
    }
}

