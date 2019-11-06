using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodePopulator : MonoBehaviour {
    public static NodePopulator instance;
    
    [SerializeField]
    AbstractMap map;

    [SerializeField]
    [Geocode]
    string[] locationStrings;
    Vector2d[] locations;

    [SerializeField]
    float spawnScale = 100f;

    [SerializeField]
    Node nodePrefab;

    List<Node> spawnedNodes;

    private bool areNodesPopulated = false;
    
    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }

    public void PopulateNodes() {
        areNodesPopulated = true;
        locations = new Vector2d[locationStrings.Length];
        spawnedNodes = new List<Node>();
        for (int i = 0; i < locationStrings.Length; i++) {
            var locationString = locationStrings[i];
            locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Lean.Pool.LeanPool.Spawn(nodePrefab);
            //var instance = Instantiate(markerPrefab);
            instance.transform.localPosition = map.GeoToWorldPosition(locations[i], true);
            instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
            spawnedNodes.Add(instance);
        }
    }

    // Takes in a location string in the format "lat,lon" and returns a Vector3 position on the map
    public Vector3 GetWorldSpacePositionFromGPS(string locationString) {
        Vector2d location = Conversions.StringToLatLon(locationString);

        return map.GeoToWorldPosition(location, true);

    }

    public void SetNodesDatabase(Dictionary<string, List<string>> dataDictionary) {
        Debug.Log("Setting nodes database");

        int totalRows = dataDictionary[dataDictionary.Keys.First()].Count;

        for (int row = 0; row < totalRows; row++) {

            Node currentNode = Lean.Pool.LeanPool.Spawn(nodePrefab);
            currentNode.Initialize();

            foreach (KeyValuePair<string, List<string>> keyValuePair in dataDictionary) {
                string key = keyValuePair.Key;
                string value = dataDictionary[keyValuePair.Key][row];

                Debug.Log("Row: " + row + " Property: " + key + " Value: " + value);

                currentNode.AddToProperties(key, value);
            }

            currentNode.FinalizeProperties();
        }
    }

    void Start() {
        //PopulateNodes();
    }

    private void Update() {
        if (!areNodesPopulated) {
            return;
        }

        int count = spawnedNodes.Count;
        for (int i = 0; i < count; i++) {
            var spawnedObject = spawnedNodes[i];
            var location = locations[i];
            spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        }
    }

}
