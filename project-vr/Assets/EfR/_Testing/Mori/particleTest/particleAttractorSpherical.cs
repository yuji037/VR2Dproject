using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorSpherical : EffectBehaviour {
	ParticleSystem[] m_aryParticleSystems;
	ParticleSystem.Particle[] m_Particles;
	Renderer refRenderer;
	public Transform endPoint;
	public Vector3 endPosition = new Vector3(-1,-1,-1);
	public float speed = 5f;
	int numParticlesAlive;
	GameObject player;
	int playerNum;

    void Start()
    {
        m_aryParticleSystems = GetComponentsInChildren<ParticleSystem>();
        //if (!GetComponent<Transform>()){
        //	GetComponent<Transform>();
        //}
        // パーティクルの開始位置はキャラの頭を追いかけさせる
        if (refRenderer)
        {
            float posY = refRenderer.material.GetFloat("_Pos");
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }
    }
	void Update () {
		foreach ( var ps in m_aryParticleSystems )
		{
			m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
			numParticlesAlive = ps.GetParticles(m_Particles);
			float step = speed * Time.deltaTime;
			for ( int i = 0; i < numParticlesAlive; i++ )
			{
				var endPos = ( endPosition != new Vector3(-1, -1, -1) ) ? endPosition : endPoint.position;
				m_Particles[i].position = Vector3.SlerpUnclamped(m_Particles[i].position, endPos - transform.position, step);
			}
			ps.SetParticles(m_Particles, numParticlesAlive);

			//// パーティクルの開始位置はキャラの頭を追いかけさせる
			//float posY = refRenderer.material.GetFloat("_Pos");
			//transform.position = new Vector3(transform.position.x, posY, transform.position.z);
		}
	}

	public override void SetAttachTarget(int num, string targetName)
	{
		switch ( num )
		{
			case 1:
				if ( targetName == "0" || targetName == "1" )
				{
					playerNum = ( targetName == "0" ) ? 0 : 1;
					var player = PlayerManager.Players[playerNum];
					refRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
				}
				else
				{
					var vrChara = GameObject.Find(targetName);
					if ( vrChara )
						refRenderer = vrChara.GetComponentInChildren<SkinnedMeshRenderer>();
					else
						Debug.Log("見つからず");
				}
				break;
			case 2:
				endPoint = GameObject.Find(targetName).transform;
				break;
		}
	}

	public override void SetEndPosition(Vector3 endPos)
	{
		endPosition = endPos;
	}
}
