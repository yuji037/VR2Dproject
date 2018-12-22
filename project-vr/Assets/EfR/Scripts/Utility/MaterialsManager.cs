using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsManager : SingletonMonoBehaviour<MaterialsManager>
{
    private List<GameObject> objects = new List<GameObject>();

	// オブジェクトが自身をListに登録する関数
	public bool Add(GameObject _object)
    {
        objects.Add(_object);
        return true;
    }

    // リストに登録されているオブジェクトのマテリアルを書き換える
    public void Change()
    {
        // このPCのPlayerが今どの状態か取得
        PlayerMove.MoveType localPlayersMoveType = PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType;

        foreach (GameObject _object in objects)
        {
            _object.GetComponent<ChangeMaterial>().Change(localPlayersMoveType);
        }
    }
}
