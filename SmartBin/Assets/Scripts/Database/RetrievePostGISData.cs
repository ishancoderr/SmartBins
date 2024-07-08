using Npgsql;
using UnityEngine;
using NetTopologySuite.Geometries;
using System;
using CesiumForUnity;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

public class RetrievePostGISData : MonoBehaviour
{
    public GameObject binPrefab; // Reference to a cube prefab in Unity
    public GameObject GeoRefFolder;

    void Start()
    {
        // SQL query to select data from the database
        var sql = "SELECT id, geom, waste, fill_level, pud_day, feedback FROM public.group2_smartbin";

        // Get Npgsql connection
        var connection = DbCommonFunctions.GetNpgsqlConnection();

        try
        {
            connection.Open();

            // Enable NetTopologySuite for handling spatial data types
            connection.TypeMapper.UseNetTopologySuite();

            var cmd = new NpgsqlCommand(sql, connection);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Read id
                    var id = reader.GetInt32(0);
                    //Debug.Log($"ID: {id}");

                    // Read geom (assuming it's a Point)
                    var geom_point = (Point)reader[1];
                    Debug.Log($"Geom: {geom_point.ToString()}");
                    Debug.Log($"Geom X: {geom_point.X.ToString()}");
                    Debug.Log($"Geom Y: {geom_point.Y.ToString()}");
                    Debug.Log($"Geom Z: {geom_point.Z.ToString()}");



                    Vector3 geom = new Vector3((float)geom_point.X, (float)geom_point.Y, (float)geom_point.Z);

                    // Convert geom (Point) to Unity coordinates
                    var unityPosition = new Vector3(geom[0], geom[1], geom[2]);

                    Debug.Log($"unity postion Z: {unityPosition}");

                    //var buttonActionScript = panel.GetComponent<ButtonActions>();
                    //buttonActionScript.CesiumGlobeAnchor = cesiumGlobeAnchor;

                    // Instantiate a cube at the converted position
                    GameObject bin = Instantiate(binPrefab, unityPosition, Quaternion.identity);

                    var cesiumGlobeAnchor = bin.AddComponent<CesiumGlobeAnchor>();
                    cesiumGlobeAnchor.longitude = geom[0];
                    cesiumGlobeAnchor.latitude = geom[1];
                    cesiumGlobeAnchor.height = geom[2];

                    
                    bin.transform.parent = GeoRefFolder.transform;
                    bin.transform.eulerAngles = new Vector3(-90f, -90f, -90f);

                    //// Read other data fields (example: waste, fill_level, pud_day, feedback)
                    //var waste = reader.GetString(2);
                    //Debug.Log($"Waste: {waste}");

                    //var fillLevel = reader.GetInt32(3);
                    //Debug.Log($"Fill Level: {fillLevel}");

                    //var pudDay = reader.GetDateTime(4);
                    //Debug.Log($"Pick Up Day: {pudDay.ToShortDateString()}");

                    //var feedback = reader.GetString(5);
                    //Debug.Log($"Feedback: {feedback}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error retrieving data: {ex.Message}");
        }
        finally
        {
            connection.Close();
        }
    }

    void Update()
    {
        // Any update logic you may have
    }
}