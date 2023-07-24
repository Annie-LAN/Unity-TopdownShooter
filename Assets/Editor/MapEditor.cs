using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        MapGenerator map = target as MapGenerator;
        if (DrawDefaultInspector()) // only true if there are changes
        {
            map.GenerateMap();
        }

        if(GUILayout.Button("Generate Map")) // make a button so we can manually generate the map
        {
            map.GenerateMap();
        }
        
    }
}
