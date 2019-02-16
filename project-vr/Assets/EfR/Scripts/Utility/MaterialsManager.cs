using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsManager : SingletonMonoBehaviour<MaterialsManager>
{
    private List<GameObject> objects = new List<GameObject>();

	public static Shader HoloShader = null;

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

	public static Shader GetHoloShader()
	{
		if( HoloShader == null )
		{
			HoloShader = Shader.Find("EfRchan/Holo");
		}
		if ( HoloShader == null )
			Debug.LogError("Holoシェーダ見つからず");

		return HoloShader;
	}

	public static IEnumerator HoloFadeCoroutine(Material mat, bool fadeIn, Vector3 objPosition, Transform objTransform, 
		float fadeHeightMax, float speed = 1f)
	{
		mat.SetFloat("_App", 1f);
		for ( float t = 0; t <= fadeHeightMax; t += Time.deltaTime * speed )
		{
			if ( objTransform )
				objPosition = objTransform.position;

			float setPosY = objPosition.y + (fadeIn ? t :( fadeHeightMax - t ));
			mat.SetFloat("_Pos", setPosY);

			yield return null;
		}

		if ( fadeIn ) mat.SetFloat("_App", 0f);
	}

}
