using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

using DG.Tweening;

public class DataNodePopulator : MonoBehaviour {
    public static DataNodePopulator instance;

    [SerializeField]
    AbstractMap map;

    [SerializeField]
    float spawnScale = 1.0f;

    [SerializeField]
    DataNode nodePrefab;

    List<DataNode> spawnedNodes;

    private bool areNodesPopulated = false;

    private Dictionary<string, List<string>> dataDictionary;

    private bool isMapInitialized = false;

    [SerializeField]
    private GameObject customBuildings;

    public struct Properties {
        public float minimum;
        public float maximum;
        public float average;
    }

    private Dictionary<string, Properties> columnPropertiesDictionary;

    private string dataTabId;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }

        // Hide map on awake
        HideMap();
    }

    private void HideMap()
    {
        map.transform.parent.DOScale(0.0f, Utilities.animationSpeed * 2).SetEase(Ease.InOutBack);
        map.transform.parent.DOLocalMoveY(0.5f, Utilities.animationSpeed * 2).SetEase(Ease.InOutBack);

        customBuildings.SetActive(false);
    }

    private void ShowMap()
    {
        map.transform.parent.DOScale(0.0075f, Utilities.animationSpeed * 2).SetEase(Ease.InOutBack);
        map.transform.parent.DOLocalMoveY(1.25f, Utilities.animationSpeed * 2).SetEase(Ease.InOutBack);
    }

    public void SetMapOrigin(string locationString, string tabId)
    {
        /*
        // Despawn all prefabs first
        Lean.Pool.LeanPool.DespawnAll();

        dataTabId = tabId;

        Vector2d origin = Mapbox.Unity.Utilities.Conversions.StringToLatLon(locationString);

        float mapZoom = 17.5f;
        
        // If map is not set to initialize on start and is not initialized
        if (!map.InitializeOnStart && !isMapInitialized)
        {
            Debug.Log("Initializing map");
            isMapInitialized = true;
            map.Initialize(origin, (int)mapZoom);
        } else
        {
            Debug.Log("Updating map");
            map.UpdateMap(origin, (int)mapZoom);
        }

        GoogleSheetsFetcher.instance.Initialize(dataTabId);

        map.SetZoom(mapZoom);
        
        ShowMap();
        */
        StartCoroutine(SetMapOriginCoroutine(locationString, tabId));
    }

    private IEnumerator SetMapOriginCoroutine(string locationString, string tabId)
    {
        // Hide map
        HideMap();

        // Despawn all prefabs
        Lean.Pool.LeanPool.DespawnAll();

        yield return new WaitForSeconds(Utilities.animationSpeed * 2);

        dataTabId = tabId;

        Vector2d origin = Mapbox.Unity.Utilities.Conversions.StringToLatLon(locationString);

        float mapZoom = 17.5f;

        if (locationString.Equals("1.311310, 103.879139"))
        {
            mapZoom = 18.5f;
            customBuildings.SetActive(true);
        } else
        {
            customBuildings.SetActive(false);
        }

        // If map is not set to initialize on start and is not initialized
        if (!map.InitializeOnStart && !isMapInitialized)
        {
            Debug.Log("Initializing map");
            isMapInitialized = true;
            map.Initialize(origin, (int)mapZoom);
        }
        else
        {
            Debug.Log("Updating map");
            map.UpdateMap(origin, (int)mapZoom);
        }

        GoogleSheetsFetcher.instance.Initialize(dataTabId);

        map.SetZoom(mapZoom);

        yield return new WaitForSeconds(Utilities.animationSpeed * 2);

        ShowMap();

        yield return new WaitForSeconds(Utilities.animationSpeed * 2);

        StartCoroutine(SpawnNodeCorutine());
    }
    
    // Takes in a location string in the format "lat,lon" and returns a Vector3 position on the map
    public Vector3 GetWorldSpacePositionFromGPS(string locationString) {
        Vector2d location = Conversions.StringToLatLon(locationString);

        return map.GeoToWorldPosition(location, true);

    }

    public void SetNodesDatabase(Dictionary<string, List<string>> _dataDictionary) {
        Debug.Log("Setting nodes database");

        // Initialize data dictionary
        dataDictionary = new Dictionary<string, List<string>>(_dataDictionary);

        List<string> dataKeys = new List<string>(dataDictionary.Keys);

        // For each data key
        for (int i = 0; i < dataKeys.Count; i++) {
            float outputValue;

            // Try parse the first value of each column
            bool success = float.TryParse(dataDictionary[dataKeys[i]][0], NumberStyles.Float, CultureInfo.InvariantCulture, out outputValue);

            // If this is not numerical data, move on to next parameter
            if (!success) {
                continue;
            }

            // Add new parameter on UI
            UIController.instance.AddNewParameter(dataKeys[i]);
        }
        //StartCoroutine(SpawnNodeCorutine());
    }

    private IEnumerator SpawnNodeCorutine() {
        int totalRows = dataDictionary[dataDictionary.Keys.First()].Count;
        spawnedNodes = new List<DataNode>();

        // Initialize heatmap
        Heatmap.instance.Initialize(totalRows);

        // For every row, create a node with properties from each column
        for (int row = 0; row < totalRows; row++) {
            // Spawn node
            DataNode currentNode = Lean.Pool.LeanPool.Spawn(nodePrefab);

            // Set spawn scale
            currentNode.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);

            currentNode.index = row;

            // Initialize
            currentNode.Initialize();
            
            // Set up properties for each data node
            foreach (KeyValuePair<string, List<string>> keyValuePair in dataDictionary) {
                string key = keyValuePair.Key;
                string value = dataDictionary[keyValuePair.Key][row];

                //Debug.Log("Row: " + row + " Property: " + key + " Value: " + value);

                currentNode.AddToProperties(key, value);
            }

            // Add to spawned nodes
            spawnedNodes.Add(currentNode);
            //Debug.Log("Current row: " + row);

            yield return new WaitForSeconds(0.005f);
        }

        areNodesPopulated = true;

        // Update properties for column
        UpdateProperties();
        
        // Finalize properties for each spawned node
        for (int i = 0; i < spawnedNodes.Count; i++) {
            spawnedNodes[i].FinalizeProperties();
        }
    }

    // Use this method to update row properties
    private void UpdateProperties() {
        columnPropertiesDictionary = new Dictionary<string, Properties>();

        // For each column in data dictionary
        foreach (KeyValuePair<string, List<string>> keyValuePair in dataDictionary) {
            float outputValue;
            bool success = float.TryParse(dataDictionary[keyValuePair.Key][0], NumberStyles.Float, CultureInfo.InvariantCulture, out outputValue);

            // If the first value is not a float, then skip this column
            if (!success) {
                continue;
            }

            List<string> columnValues = dataDictionary[keyValuePair.Key];

            Properties columnProperties = new Properties();

            float count = columnValues.Count;
            float sum = 0;
            float min = float.MaxValue;
            float max = float.MinValue;

            for (int row = 0; row < count; row++) {
                float value = float.Parse(columnValues[row]);

                // Get the sum to calculate average at the end
                sum += value;

                // Keep track of the minimum
                if (value < min) {
                    min = value;
                }

                // Keep track of the maximum
                if (value > max) {
                    max = value;
                }
            }

            columnProperties.minimum = min;
            columnProperties.maximum = max;
            columnProperties.average = sum / count;

            columnPropertiesDictionary.Add(keyValuePair.Key, columnProperties);
        }
    }

    // Returns a normalized value from 0.0 to 1.0 given the property and value
    public float GetNormalizedValue(string property, float value) {
        //Calculate the normalized float
        float normalizedValue = (value - columnPropertiesDictionary[property].minimum) /
            (columnPropertiesDictionary[property].maximum - columnPropertiesDictionary[property].minimum);

        normalizedValue = Mathf.Clamp(normalizedValue, 0, 1);

        return Mathf.Clamp(normalizedValue, 0, 1);
    }

    void Start() {
        //PopulateNodes();
    }

    private void Update() {
    }

}
