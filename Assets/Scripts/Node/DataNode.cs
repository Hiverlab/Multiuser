using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using DG.Tweening;
using Mapbox.Geocoding;
using Mapbox.Unity;
using TMPro;

// The node class is purely for the visual representation of a data point
public class DataNode : MonoBehaviour {

    // These are the different dimensions that the node is affected by
    #region Dimensions

    public enum DimensionType {
        None,
        //Position,
        Scale,
        ColorScale,
        //Shape
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

    #region Properties
    private string selectedProperty;

    #endregion

    #region Properties References

    [Header("Properties References")]
    [SerializeField]
    private TextMeshPro labelTextMesh;

    [SerializeField]
    private Transform nodeCollider;

    #endregion

    #region Dimension Updating
    private void UpdatePosition() {
        transform.position = position;
    }

    private void UpdateScale() {
        // Update model scale on Y axis
        float modelYScale = 1.0f * scale;

        // Update model mesh renderer scale
        /*
        shapeMeshRenderer.transform.localScale = new Vector3(shapeMeshRenderer.transform.localScale.x,
            modelYScale,
            shapeMeshRenderer.transform.localScale.z);
            */

        shapeMeshRenderer.transform.DOScaleY(modelYScale, Utilities.animationSpeed);

        // Calculate offset position
        Vector3 offsetPosition = new Vector3(0, (modelYScale *  0.25f) + 0.025f, 0);
        
        // Update text mesh and collider position
        labelTextMesh.transform.localPosition = offsetPosition;
        nodeCollider.transform.localPosition = offsetPosition;
    }

    private void UpdateColor() {
        // TODO: Add mesh renderer or sprite
    }

    private void UpdateColorScale() {
        //shapeMeshRenderer.material.color = colorGradient.Evaluate(ColorScale);

        shapeMeshRenderer.material.DOColor(colorGradient.Evaluate(ColorScale), Utilities.animationSpeed);

        //DOTween.To(() => shapeMeshRenderer.material.GetColor("_EmissionColor"), x => shapeMeshRenderer.material.SetColor("_EmissionColor", x), colorGradient.Evaluate(ColorScale), Utilities.animationSpeed);
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
            transform.position = DataNodePopulator.instance.GetWorldSpacePositionFromGPS(LocationString);
        }
        */
        //UpdateColorScale();
    }

    public void Initialize() {
        HideRestaurantName();

        // Initialize properties dictionary
        propertiesDictionary = new Dictionary<string, string>();

        // Initialize geocode resource
        resource = new ForwardGeocodeResource("");

        // Subscribe to geocoder response
        OnGeocoderResponseDelegate += OnGeocoderResponse;

        // Subscribe to OnParameterSelected event
        //UIController.instance.OnTestParameterSelected += SetProperty;

        // Subscribe to OnStyleSelected event
        //UIController.instance.OnDimensionTypeSelected += SetStyle;

        // Subscribe to OnParameterSelected event
        UIController.instance.OnParameterSelected += SetPropertyAndStyle;
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
        //Debug.Log("Finalizing properties");

        // Set label name
        //LabelName = propertiesDictionary["Restaurant"];

        // Place node in world space
        PlaceInWorldSpace();

        // Set Scale as 0.0
        Scale = 0.0f;
    }

    public void SetProperty(string property) {
        Debug.Log("Setting property: " + property);

        selectedProperty = property;

        UpdateVisuals();
    }

    public void SetPropertyAndStyle(string property, DimensionType _dimensionType) {
        Debug.Log("Setting property: " + property + " Setting style: " + _dimensionType);
        
        selectedProperty = property;
        dimensionType = _dimensionType;

        UpdateVisuals();
    }

    public void SetStyle(DimensionType _dimensionType) {
        Debug.Log("Setting style: " + _dimensionType);

        dimensionType = _dimensionType;

        UpdateVisuals();
    }

    private void UpdateVisuals() {
        // If selected property is empty, then return
        if (string.IsNullOrEmpty(selectedProperty)) {
            return;
        }

        Debug.Log("Property key: " + selectedProperty);

        if (!propertiesDictionary.ContainsKey(selectedProperty))
        {
            Debug.Log("Does not contain: " + selectedProperty);
            return;
        }

        float outputValue;
        bool success = float.TryParse(propertiesDictionary[selectedProperty], NumberStyles.Float, CultureInfo.InvariantCulture, out outputValue);

        // If the first value is not a float, then skip this column
        if (!success) {
            return;
        }

        Debug.Log("Setting value: " + outputValue);

        switch (dimensionType) {
            case DimensionType.Scale:
                Scale = DataNodePopulator.instance.GetNormalizedValue(selectedProperty, outputValue);
                //ColorScale = 0.0f;
                break;
            case DimensionType.ColorScale:
                ColorScale = DataNodePopulator.instance.GetNormalizedValue(selectedProperty, outputValue);
                //Scale = 0.25f;
                break;
            default:
                Scale = 0.1f;
                ColorScale = 0.0f;
                break;
        }
    }

    private void PlaceInWorldSpace()
    {
        // If using Address, convert it to GPS
        if (propertiesDictionary.ContainsKey("Address"))
        {
            ConvertAddressToGPS();
        }

        // If has GPS, then just use GPS
        if (propertiesDictionary.ContainsKey("GPS"))
        {
            transform.position = DataNodePopulator.instance.GetWorldSpacePositionFromGPS(propertiesDictionary["GPS"]);
            
            // Set scale to be 0.1f
            Scale = 0.1f;
        }
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

        transform.position = DataNodePopulator.instance.GetWorldSpacePositionFromGPS(LocationString);
        
        // Set scale to be 0.1f
        Scale = 0.1f;
    }

    #endregion

    #region Interactive Functions

    public void OnTouchEnter() {
        Debug.Log(transform.name + " touched");
        ShowRestaurantName();
    }

    public void OnTouchExit() {
        Debug.Log(transform.name + " touched");
        HideRestaurantName();
    }

    private void ShowRestaurantName() {
        labelTextMesh.gameObject.SetActive(true);
    }

    private void HideRestaurantName() {
        labelTextMesh.gameObject.SetActive(false);
    }

    #endregion
}
