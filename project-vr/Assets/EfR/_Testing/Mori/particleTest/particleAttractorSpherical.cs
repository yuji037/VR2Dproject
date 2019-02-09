using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorSpherical : EffectBehaviour {
	ParticleSystem[] m_aryParticleSystems;
	ParticleSystem.Particle[] m_Particles;
	public Transform endPoint;
	public float speed = 5f;
	int numParticlesAlive;
	GameObject player;
	int playerNum;

	void Start () {
		m_aryParticleSystems = GetComponentsInChildren<ParticleSystem>();
		//if (!GetComponent<Transform>()){
		//	GetComponent<Transform>();
		//}
	}
	void Update () {
		foreach ( var ps in m_aryParticleSystems )
		{
			m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
			numParticlesAlive = ps.GetParticles(m_Particles);
			float step = speed * Time.deltaTime;
			for ( int i = 0; i < numParticlesAlive; i++ )
			{
				m_Particles[i].position = Vector3.SlerpUnclamped(m_Particles[i].position, endPoint.position - transform.position, step);
			}
			ps.SetParticles(m_Particles, numParticlesAlive);

			// パーティクルの開始位置はキャラの頭を追いかけさせる
			float posY = VRCharaHoloController.GetInstance().GetCharaBorderPos(playerNum);
			transform.position = new Vector3(transform.position.x, posY, transform.position.z);
		}
	}

	public override void SetAttachTarget(int num, string targetName)
	{
		switch ( num )
		{
			case 1:
				playerNum = ( targetName == "0" ) ? 0 : 1;
				//player = PlayerManager.Players[playerNum];
				break;

			case 2:
				endPoint = GameObject.Find(targetName).transform;
				break;
		}
	}
}
