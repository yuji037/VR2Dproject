using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPingPong : MonoBehaviour {

	public enum ParamType {
		OutlineWidth,
		ColorAlpha,
	}

	
	public ParamType changeParamType = ParamType.OutlineWidth;

	public float min = 0f;
	public float max = 0.5f;
	public float speed = 1f;

	Material targetMaterial;

	// Use this for initialization
	void Start () {
		targetMaterial = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		var value = Mathf.PingPong(Time.time * speed, max - min) + min;

		switch ( changeParamType )
		{
			case ParamType.OutlineWidth:
				targetMaterial.SetFloat("_Outline", value);
				break;
			case ParamType.ColorAlpha:
				var c = targetMaterial.color;
				targetMaterial.color = new Color(c.r, c.g, c.b, value);
				break;
		}
	}
}
