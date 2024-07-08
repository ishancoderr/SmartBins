using CesiumForUnity;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TouchRaycaster : MonoBehaviour
{
    public GameObject binPrefab;  // Reference to the cube prefab
    private LineRenderer lineRenderer;
    private Vector3 hitPoint;
    private bool hitDetected;
    public float lineHeight = 10;
    public float lineWidth = 0.5f;
    public GameObject GeoRefFolder;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel_RightMouse;
    [SerializeField] GameObject panel_fillform;
    [SerializeField] GameObject panel_clicktocreatebin;

    public float correctionX = 90f;
    public float correctionY = 90f;
    public float correctionZ = 90f;

    public bool isPlacing = false;

    void Start()
    {
        // Initialize the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        panel_clicktocreatebin.SetActive(true);
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component missing from this GameObject. Please add a LineRenderer component.");
            return;
        }

        // Set some default properties for the line renderer
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        hitDetected = false;
        isPlacing = true;
        
    }

 
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    

    void HandleMouseInput()
    {
        
        if (Input.GetMouseButton(0))
        {
            VisualizeRay(Input.mousePosition);
        

        }

        //if (Input.GetMouseButton(0) && panel.activeSelf == false && isPlacing == false && panel_clicktocreatebin.activeSelf == true) {
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        // Überprüfe, ob das getroffene Objekt das gewünschte Objekt ist
        //        //if (hit.collider.gameObject == GameObject)
        //        //{
        //        //    // Das Objekt wurde ausgewählt
        //        //    Debug.Log("Das Objekt wurde ausgewählt!");

        //        //    // Füge hier deine Logik hinzu, was passieren soll, wenn das Objekt ausgewählt wurde
        //        //}
        //    }


        //}

        

        if (Input.GetMouseButtonUp(0) && hitDetected && panel.activeSelf == false && isPlacing == true) 
        {
            panel_clicktocreatebin.SetActive(false);
            panel_fillform.SetActive(true);
            isPlacing = false;
            var gameObject = Instantiate(binPrefab, hitPoint, Quaternion.identity);
            var cesiumGlobeAnchor = gameObject.AddComponent<CesiumGlobeAnchor>();
            gameObject.transform.parent = GeoRefFolder.transform;
            gameObject.transform.eulerAngles = new Vector3(-90f, -90f, -90f);

            panel.SetActive(true);
            var buttonActionScript = panel.GetComponent<ButtonActions>();
            buttonActionScript.CesiumGlobeAnchor = cesiumGlobeAnchor;

            




        }

        if (Input.GetMouseButton(1))
        {
            panel_clicktocreatebin.SetActive(true);
            isPlacing = true;

        }
    }

    
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                VisualizeRay(touch.position);
            }

            if (touch.phase == TouchPhase.Ended && hitDetected)
            {
                var gameobject = Instantiate(binPrefab, hitPoint, Quaternion.identity);
                gameObject.AddComponent<CesiumGlobeAnchor>();
                gameObject.transform.parent = GeoRefFolder.transform;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void VisualizeRay(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            hitPoint = hit.point;
            hitDetected = true;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector3(hit.point.x, hit.point.y + lineHeight, hit.point.z));
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, new Vector3(hit.point.x, hit.point.y + lineHeight, hit.point.z));
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
            hitDetected = false;
        }
    }
}
