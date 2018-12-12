using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera2DController : MonoBehaviour
{
    public enum WorldDirection
    {
        Front,
        Back,
        Left,
        Right
    }
    GameObject _targetObj;
    GameObject targetObject
    {
        get
        {
            if (!_targetObj)
                _targetObj = PlayerManager.LocalPlayer;
            return _targetObj;
        }
    }

    [SerializeField]
    Transform virtualOffsetObject;

    float cameraAngle;

    Vector3 frontOffset;

    Vector3 currentOffset;
    private void Awake()
    {
        frontOffset = virtualOffsetObject.position - transform.position;
    }
    bool isInitialized=false;
    private void Init()
    {
        if (isInitialized) return;
        if (PlayerManager.CheckLocalPlayerNumber(PlayerNumber.Player1))
        {
            ChangeCameraDirection(WorldDirection.Back);
        }
        else
        {
            ChangeCameraDirection(WorldDirection.Front);
        }
        isInitialized = true;
    }

    void Update()
    {
        if (!targetObject) return;
        Init();
        transform.position = targetObject.transform.position - currentOffset;

    }

    void ChangeCurrentOffset()
    {
        currentOffset = Quaternion.Euler(0, cameraAngle, 0) * frontOffset;
    }

    public void ChangeCameraDirection(WorldDirection cameraDirection)
    {
        ChangeCameraDirection(ConvertToAngle(cameraDirection));
    }

    public void ChangeCameraDirection(float cameraAngle)
    {
        this.cameraAngle = cameraAngle;
        ChangeCurrentOffset();
        transform.position = targetObject.transform.position - currentOffset;
        transform.eulerAngles = new Vector3(0,cameraAngle,0);
    }

    float ConvertToAngle(WorldDirection direction)
    {
        switch (direction)
        {
            case WorldDirection.Front:
                return 0f;
            case WorldDirection.Back:
                return 180.0f;
            case WorldDirection.Left:
                return 90.0f;
            case WorldDirection.Right:
                return -90.0f;
        }
        return 0f;
    }
}
