using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public static Checker Instance;

    public ObjectData objectData;

    public List<BoxPoint> boxPoints = new List<BoxPoint>();

    public Dictionary<int, GameObject> prefabDictionary = new Dictionary<int, GameObject>();

    public bool isActive;

    private void Awake()
    {
        CollectAreas();
    }

    private void Start()
    {
        Instance = this;
    }

    void CollectAreas()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("box");
        GameObject[] taggedRocks = GameObject.FindGameObjectsWithTag("rock");

        int id = -1;
        foreach (GameObject obj in taggedObjects)
        {
            boxPoints.Add(obj.gameObject.GetComponent<BoxPoint>());
        }
        
        foreach (GameObject obj in taggedRocks)
        {
            obj.GetComponent<Rock>().id = id;
            id--;   
        }

        objectData.Collect(prefabDictionary);
    }

    public void NotifyObjectStatusChange(Item obj)
    {
        BoxPoint point = boxPoints.Find(p => p.objectID == obj.id);

        if (point != null)
        {
            point.occupied = false;
            point.objectID = 0;
        }
    }
}
