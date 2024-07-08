using CesiumForUnity;
using NetTopologySuite.Geometries;
using Npgsql;
using Photon.Pun.Demo.PunBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject panel_RightMouse;
    [SerializeField] GameObject panel_fillform;
    public CesiumGlobeAnchor CesiumGlobeAnchor;
    public TouchRaycaster touchraycaster;
    public void OnConfirm()
    {
        //Code by Timo
        Debug.Log("button works");
        panel_fillform.SetActive(true);
        var rubbishType = GameObject.FindGameObjectWithTag("dropdown_rubbish_type").GetComponent<TMP_Dropdown>();
        var fillLevel = GameObject.FindGameObjectWithTag("scrollbar_fill_level").GetComponent<Scrollbar>();
        var pick_up_day = GameObject.FindGameObjectWithTag("inpfield_pick_up_day").GetComponent<TMP_InputField>();
        var feedback = GameObject.FindGameObjectWithTag("inpfield_feedback").GetComponent<TMP_InputField>();

        string sql_timestring = "yyyy-MM-dd";
        var string_pud = pick_up_day.text;

        var day = string_pud.Substring(8,2);
        var month = string_pud.Substring(5, 2);
        var year = string_pud.Substring(0,4);

        sql_timestring = year + "-" + month + "-" + day;
        

        //sql_pud_string = dateValue.ToString(sql_pud_string);
        //Debug.Log(sql_timestring);

        var query = $"INSERT INTO group2_smartbin " +
            $"(geom, waste, fill_level, pud_day, feedback)" +
            $" VALUES(ST_GeomFromText('POINTZ({CesiumGlobeAnchor.longitude.ToString(CultureInfo.InvariantCulture)}" +
            $" {CesiumGlobeAnchor.latitude.ToString(CultureInfo.InvariantCulture)} {CesiumGlobeAnchor.height.ToString(CultureInfo.InvariantCulture)})'), '{rubbishType.options[rubbishType.value].text}', " +
            $"{fillLevel.value}, '{sql_timestring}', '{feedback.text}')";

        var connection = DbCommonFunctions.GetNpgsqlConnection();
        connection.Open();
        var cmd = new NpgsqlCommand(query, connection);
        cmd.ExecuteNonQuery();
        Debug.Log("Query finished");

        connection.Close();
        Debug.Log("panel anfangen zuzumachen");
        panel.SetActive(false);
        panel_fillform.SetActive(false);
        panel_RightMouse.SetActive(true);
        Debug.Log("panel zu");






        //var playerManagers = UnityEngine.Object.FindObjectsOfType<PlayerManagment1>();
        //var playerManagement = playerManagers[0];
        //var photonView = playerManagement.GetPhotonView();
        //if (!photonView.IsMine)
        //{
        //    for (int i = 1; i < playerManagers.Length; i++)
        //    {
        //        playerManagement = playerManagers[i];
        //        photonView = playerManagement.GetPhotonView();
        //        if (photonView.IsMine)
        //            break;
        //    }
        //}

        //var pickedPoint = playerManagement.GetPickedPoint();
        //var entranceType = GameObject.FindGameObjectWithTag("entrance").GetComponent<TMP_Dropdown>();
        //var comment = GameObject.FindGameObjectWithTag("comment").GetComponent<TMP_InputField>();
        //var pathGeoreferencedPoints = UnityEngine.Object.FindObjectOfType<DynamicNavMesh>().GetPathGeoreferencedPoints();

        // create tables PostgreSQL
        //var connection = DbCommonFunctions.GetNpgsqlConnection();
        //string fields = "(id int, geom GEOMETRY(POINTZ), type text, comment text)";
        //connection.Open();
        //DbCommonFunctions.CreateTableIfNotExistOrTruncate("entrances_new", connection, fields, false);
        //fields = "(id int, geom GEOMETRY(LINESTRINGZ))";
        //DbCommonFunctions.CreateTableIfNotExistOrTruncate("paths_new", connection, fields, false);

        //// Insert into point table
        //var query = $"INSERT INTO entrances_new (geom, type, comment) VALUES( ST_GeomFromText('POINTZ({pickedPoint.longitude} {pickedPoint.latitude} {pickedPoint.height})', 4326)," +
        //    $" '{entranceType.options[entranceType.value].text}', '{comment.text}')";
        //var cmd = new NpgsqlCommand(query, connection);
        //cmd.ExecuteNonQuery();

        // Insert into linestring table
        //var firstPoint = pathGeoreferencedPoints[0];
        //query = $"INSERT INTO paths_new (geom) VALUES( ST_MakeLine(ARRAY[ST_MakePoint({firstPoint.x},{firstPoint.y},{firstPoint.z})";
        //using (var conn = connection)
        //{
        //    cmd = new NpgsqlCommand();
        //    cmd.Connection = conn;
        //    var sql = new System.Text.StringBuilder(query);
        //    for (var i = 1; i < pathGeoreferencedPoints.Count; i++)
        //    {
        //        var point = pathGeoreferencedPoints[i];
        //        sql.Append($", ST_MakePoint({point.x},{point.y},{point.z})");
        //    }
        //    sql.Append("]))");

        //    cmd.CommandText = sql.ToString();
        //    cmd.ExecuteNonQuery();
        //}
        //connection.Close();

        // Turn on camera controller and playerManagement script
        UnityEngine.Object.FindObjectOfType<CesiumCameraController>().enabled = true;
       
        panel.SetActive(false);
    }

    public void OnClose()
    {
        // Turn on camera controller and playerManagement script
        //UnityEngine.Object.FindObjectOfType<CesiumCameraController>().enabled = true;
        //var playerManagers = UnityEngine.Object.FindObjectsOfType<PlayerManagment1>();
        //var playerManagement = playerManagers[0];
        //var photonView = playerManagement.GetPhotonView();
        //if (!photonView.IsMine)
        //{
        //    for (int i = 1; i < playerManagers.Length; i++)
        //    {
        //        playerManagement = playerManagers[i];
        //        photonView = playerManagement.GetPhotonView();
        //        if (photonView.IsMine)
        //            break;
        //    }
        //}
        //playerManagement.pointIsSuccesfullyAdded = false;
        //playerManagement.enabled = true;
        //Destroy(TouchRaycaster.);
        
        
        panel.SetActive(false);
        panel_fillform.SetActive(false);
        panel_RightMouse.SetActive(true);

        //// Remove lineObject if existant;
        //var dynamicNavMesh = UnityEngine.Object.FindObjectOfType<DynamicNavMesh>();
        //dynamicNavMesh.DestroyLineObject();
    }
}
