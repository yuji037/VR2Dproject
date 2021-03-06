﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPredictionActiveController: SingletonMonoBehaviour<FloorPredictionActiveController>
{
  
    [SerializeField]
    GameObject[] PredictionFloorsPrefab;

    FloorPredictionView[] floorPredictions; 
    private void Start()
    {
        floorPredictions = new FloorPredictionView[PredictionFloorsPrefab.Length];
        for (int i = 0; i < PredictionFloorsPrefab.Length; i++)
        {
            var obj = Instantiate(PredictionFloorsPrefab[i]);
            floorPredictions[i]=obj.GetComponent<FloorPredictionView>();
            DontDestroyOnLoad(obj);
        }
    }


    public void SetView(Vector3 hitPos, PointerHitScreen pointerHitScreen,bool canCreate)
    {
        FloorPredictionView activePredictionFloor=null;

        var floorForm = pointerHitScreen.GetFloorForm;

        for (int i=0;i< floorPredictions.Length;i++)
        {
            var active = (i == (int)floorForm);
            floorPredictions[i].gameObject.SetActive(active);
            if (active) activePredictionFloor = floorPredictions[i];
        }
        activePredictionFloor.ActiveView(hitPos,pointerHitScreen,canCreate);

    }

    public void AllInactive()
    {
        foreach (var i in floorPredictions)
        {
            i.gameObject.SetActive(false);
        }
    }
}
