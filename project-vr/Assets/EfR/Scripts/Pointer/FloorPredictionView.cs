using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPredictionView : MonoBehaviour
{
    private void Start()
    {
        scales = new Vector3[PredictionFloors.Length];
        for (int i = 0; i < PredictionFloors.Length; i++)
        {
            scales[i] = PredictionFloors[i].transform.lossyScale;
            PredictionFloors[i] = Instantiate(PredictionFloors[i]);
        }
    }
    [SerializeField]
    GameObject[] PredictionFloors;

    Vector3[] scales;
    public void View(Vector3 hitPos, PointerHitScreen pointerHitScreen)
    {
        var form = pointerHitScreen.GetFloorForm;
        PredictionFloors[(int)form].SetActive(true);
        var floorScale = scales[(int)form];

        float adjustPosY = 0f;
        float subY = 0f;
        if ((subY = GetUpperBase(hitPos, floorScale) - GetUpperBase(pointerHitScreen.transform)) > 0)
        {
            adjustPosY = subY * 0.5f;
        }
        else if ((subY = GetLowerBase(hitPos, floorScale) - GetLowerBase(pointerHitScreen.transform)) > 0)
        {
            adjustPosY = subY * -0.5f;
        }

        float adjustPosX = 0f;
        float subX = 0f;
        if ((subX = GetRightSide(hitPos, floorScale) - GetRightSide(pointerHitScreen.transform)) > 0)
        {
            adjustPosX = subX * 0.5f;
        }
        else if ((subX = GetLeftSide(hitPos, floorScale) - GetLeftSide(pointerHitScreen.transform)) > 0)
        {
            adjustPosX = subX * -0.5f;
        }
        Debug.Log(adjustPosX+":"+adjustPosY);
        var floor = PredictionFloors[(int)form].transform;
        floor.position = hitPos+new Vector3(0,0,-0.1f); /*- new Vector3(adjustPosX, adjustPosY, 1.0f);*/
        //floor.localScale = new Vector3(
        //    floorScale.x + adjustPosX/2,
        //    floorScale.y + adjustPosY/2,
        //    1);
    }

    float GetUpperBase(Transform target)
    {
        //上底
        return target.position.y + target.transform.lossyScale.y / 2;
    }
    float GetLowerBase(Transform target)
    {
        //下底
        return target.position.y - target.transform.lossyScale.y / 2;
    }
    float GetRightSide(Transform target)
    {
        //右辺
        return target.position.x + target.transform.lossyScale.x / 2;
    }
    float GetLeftSide(Transform target)
    {
        //左辺
        return target.position.x - target.transform.lossyScale.x / 2;
    }

    float GetUpperBase(Vector3 pos, Vector3 scale)
    {
        //上底
        return pos.y + scale.y / 2;
    }
    float GetLowerBase(Vector3 pos, Vector3 scale)
    {
        //下底
        return pos.y - scale.y / 2;
    }
    float GetRightSide(Vector3 pos, Vector3 scale)
    {
        //右辺
        return pos.x + scale.x / 2;
    }
    float GetLeftSide(Vector3 pos, Vector3 scale)
    {
        //左辺
        return pos.x - scale.x / 2;
    }
}
