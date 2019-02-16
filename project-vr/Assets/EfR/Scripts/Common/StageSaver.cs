using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageSaver : NetworkBehaviour
{
    static StageSaver instance = null;

	float holoFadeHeight = 1.0f;
	float holoFadeSpeed = 1f;

	private void Awake()
    {
        instance = this;
    }
    public static StageSaver GetInstance()
    {
        return instance;
    }
  
    struct SaveParam
    {
        public SaveParam(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
        public Vector3 position;
        public Quaternion rotation;
    }
    Dictionary<SavableObject, SaveParam> savableParams = new Dictionary<SavableObject, SaveParam>();

    [Server]
    public void RegisterSavableObject(SavableObject savableObject)
    {
        savableParams.Add(savableObject, new SaveParam(savableObject.transform.position, savableObject.transform.rotation));
    }

    [Server]
    public void SaveAllSavableObjects()
    {
        foreach (var i in savableParams)
        {
            savableParams[i.Key] = new SaveParam(i.Key.transform.position, i.Key.transform.rotation);
        }
    }

    [Server]
    public void LoadAllSavableObjects()
    {
        foreach (var i in savableParams)
        {
            i.Key.transform.position = i.Value.position;
            i.Key.transform.rotation = i.Value.rotation;
        }
    }

	// 種類によってHoloFadeさせずに瞬間移動させる場合は
	// SavableObjectにフラグを持たせる
    [Server]
    public void Respawn(SavableObject savableObject)
    {
		StartCoroutine(RespawnCoroutine(savableObject));
    }



	IEnumerator RespawnCoroutine(SavableObject savableObject)
	{
		SwitchMovable(savableObject.gameObject, false);
		var prevShaderName = GetShaderName(savableObject.gameObject);
		var netId = savableObject.GetComponent<NetworkIdentity>();
		RpcFade(netId, false, savableObject.transform.position);
		yield return new WaitForSeconds(holoFadeHeight / holoFadeSpeed);

		savableObject.transform.position = savableParams[savableObject].position;
		savableObject.transform.rotation = savableParams[savableObject].rotation;

		RpcFade(netId, true, savableParams[savableObject].position);
		yield return new WaitForSeconds(holoFadeHeight / holoFadeSpeed);

		savableObject.ExcuteRespawnAction();

		SetShader(savableObject.gameObject, prevShaderName);
		SwitchMovable(savableObject.gameObject, true);
	}

	[ClientRpc]
	void RpcFade(NetworkIdentity netId, bool fadeIn, Vector3 startPosition)
	{
		StartCoroutine(HoloFadeCoroutine(netId.gameObject, fadeIn, startPosition));
	}

	string GetShaderName(GameObject obj)
	{
		var ren = obj.GetComponentInChildren<Renderer>();
		return ren.material.shader.name;
	}

	void SetShader(GameObject obj, string shaderName)
	{
		var ren = obj.GetComponentInChildren<Renderer>();
		ren.material.shader = Shader.Find(shaderName);
	}

	IEnumerator HoloFadeCoroutine(GameObject obj, bool fadeIn, Vector3 startPosition)
	{
		var renderer = obj.GetComponentInChildren<Renderer>();
		if ( renderer == null )
			yield break;

		var mat = renderer.material;
		var prevShader = mat.shader;
		mat.shader = MaterialsManager.GetHoloShader();

		Debug.Log("fadeIn : " + fadeIn + " startPosition : " + startPosition);

		yield return StartCoroutine(MaterialsManager.HoloFadeCoroutine(mat, fadeIn, startPosition, obj.transform, holoFadeHeight, holoFadeSpeed));
	}

	void SwitchMovable(GameObject obj, bool movable)
	{
		var rigdBody = obj.GetComponentInChildren<Rigidbody>();
		if ( rigdBody == null )
			return;

		if ( !movable )
		{
			rigdBody.velocity = Vector3.zero;
			rigdBody.angularVelocity = Vector3.zero;
			obj.isStatic = true;
		}
		else
			obj.isStatic = false;

		rigdBody.isKinematic = !movable;
	}
}
