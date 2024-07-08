using System.Collections;
using System.Collections.Generic;
using CesiumForUnity;
using Npgsql;
using UnityEngine;

public class InsertPostGISData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //var globalAnchor_cesium = GetComponent<CesiumGlobeAnchor>();
        //var query = $"INSERT INTO group2_smartbin(geom, metadata) VALUES( ST_GeomFromText('POINTZ({globalAnchor_cesium.longitude}" + $" {globalAnchor_cesium.latitude} {globalAnchor_cesium.height})', 4326), 'tree')";
        ////var query = $"INSERT INTO group2_smartbin(geom, metadata) VALUES( ST_GeomFromText('POINTZ({longitudeLatitudeHeight.x}" +
        // // $" {globalAnchor_cesium.latitude} {globalAnchor_cesium.height})', 4326), 'tree')";
        //var connection = DbCommonFunctions.GetNpgsqlConnection();
        //connection.Open();
        //var cmd = new NpgsqlCommand(query, connection);
        //cmd.ExecuteNonQuery();
        //connection.Close();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
