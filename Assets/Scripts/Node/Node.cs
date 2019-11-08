using Mapbox.Geocoding;
using Mapbox.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// The node class is purely for the visual representation of a data point
public class Node : MonoBehaviour {

    // These are the different dimensions that the node is affected by
    #region Dimensions

    public enum DimensionType {
        Position,
        Scale,
        ColorScale,
        Shape
    }

    // Need to attach a dimension type to the property
    public DimensionType dimensionType;

    // Private properties
    private Vector3 position;
    private float scale;
    private Color color;
    private float colorScale;
    private MeshRenderer shape;

    // Position (X, Y, Z) 3-Dimensions
    public Vector3 Position {
        get {
            return position;
        }
        set {
            position = value;

            UpdatePosition();
        }
    }

    // Scale 1-Dimension
    public float Scale {
        get {
            return scale;
        }
        set {
            scale = value;

            UpdateScale();
        }
    }

    // Color 1-Dimension
    public Color Color {
        get {
            return color;
        }
        set {
            color = value;

            UpdateColor();
        }
    }

    // Color Scale 1-Dimension
    public float ColorScale {
        get {
            return colorScale;
        }
        set {
            colorScale = value;

            UpdateColorScale();
        }
    }

    // Shape 1-Dimension
    public MeshRenderer Shape {
        get {
            return shape;
        }
        set {
            shape = value;

            UpdateShape();
        }
    }

    #endregion

    #region Dimension References

    [Header("Dimension References")]

    // Color Gradient
    [SerializeField]
    private Gradient colorGradient;

    // Shape Mesh Renderer
    [SerializeField]
    private MeshRenderer shapeMeshRenderer;

    #endregion

    // Other node properties
    #region Properties
    private struct Property {

    }

    // Properties dictionary
    private Dictionary<string, string> propertiesDictionary;

    // Name
    private string labelName;

    public string LabelName {
        get {
            return labelName;
        }
        set {
            labelName = value;

            UpdateLabelName();
        }
    }

    // Geocode
    private ForwardGeocodeResource resource;
    public ForwardGeocodeResponse Response { get; private set; }
    public event Action<ForwardGeocodeResponse> OnGeocoderResponseDelegate = delegate { };

    public string LocationString { get; private set; }

    // The property dimension dictionary attaches a dimension type to a data property
    public Dictionary<string, DimensionType> PropertyDimensionDictionary;

    #endregion

    #region Properties References

    [Header("Properties References")]
    [SerializeField]
    private TextMeshPro labelTextMesh;

    #endregion

    #region Dimension Updating
    private void UpdatePosition() {
        transform.position = position;
    }

    private void UpdateScale() {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * scale, transform.localScale.z);
    }

    private void UpdateColor() {
        // TODO: Add mesh renderer or sprite
    }

    private void UpdateColorScale() {
        shapeMeshRenderer.material.color = colorGradient.Evaluate(ColorScale);
    }

    private void UpdateShape() {
        // TODO: Add shape, preferably enum
    }

    #endregion

    #region Monobehaviors

    private void Awake() {
        //Initialize();
    }

    private void Update() {
        /*
        if (LocationString != null) {
            transform.position = NodePopulator.instance.GetWorldSpacePositionFromGPS(LocationString);
        }
        */
        //UpdateColorScale();
    }

    public void Initialize() {
        // Initialize properties dictionary
        propertiesDictionary = new Dictionary<string, string>();

        // Initialize geocode resource
        resource = new ForwardGeocodeResource("");

        // Subscribe to geocoder response
        OnGeocoderResponseDelegate += OnGeocoderResponse;
    }

    #endregion

    #region Properties Methods 

    private void UpdateLabelName() {
        labelTextMesh.text = labelName;
    }


    public void AddToProperties(string key, string value) {

        if (propertiesDictionary.ContainsKey(key)) {
            Debug.LogError("Key: " + key + " already exists in dictionary!");
        }

        propertiesDictionary.Add(key, value);
    }

    public void FinalizeProperties() {
        Debug.Log("Finalizing properties");

        // Set label name
        LabelName = propertiesDictionary["Author_Name"];

        // Convert address to GPS
        ConvertAddressToGPS();
        
        // Set color scale based on sentiment value
        string property = "Sentiment";
        ColorScale = NodePopulator.instance.GetNormalizedValue(property, float.Parse(propertiesDictionary[property]));
        Debug.Log("Setting color scale as: " + ColorScale);
        
        // Set length based on reach
        property = "Joy";
        Scale = NodePopulator.instance.GetNormalizedValue(property, float.Parse(propertiesDictionary[property]));
        Debug.Log("Setting scale as: " + Scale);
    }

    private void ConvertAddressToGPS() {
        resource.Query = propertiesDictionary["Address"];
        MapboxAccess.Instance.Geocoder.Geocode(resource, HandleGeocoderResponse);
    }

    void HandleGeocoderResponse(ForwardGeocodeResponse res) {
        if (null == res) {
            Debug.LogWarning("No geocode response");
        } else if (null != res.Features && res.Features.Count > 0) {
            var center = res.Features[0].Center;
            var coordinate = res.Features[0].Center;
        }
        Response = res;

        //OnGeocoderResponseDelegate(res);
        OnGeocoderResponseDelegate?.Invoke(res);
    }

    void OnGeocoderResponse(ForwardGeocodeResponse response) {
        //_resultsText.text = JsonConvert.SerializeObject(_searchLocation.Response, Formatting.Indented, JsonConverters.Converters);

        LocationString = response.Features[0].Geometry.Coordinates.x + "," + response.Features[0].Geometry.Coordinates.y;

        /*
        Debug.Log(transform.name + " Address: " + propertiesDictionary["Address"] +
            " GPS: " + LocationString);
        */

        transform.position = NodePopulator.instance.GetWorldSpacePositionFromGPS(LocationString);
    }

    #endregion

    #region Interactive Functions

    public void OnTouch() {
        Debug.Log(transform.name + " touched");
    }

    #endregion
}
