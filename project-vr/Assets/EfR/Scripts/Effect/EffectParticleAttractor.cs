using System.Collections;
using UnityEngine;


public class EffectParticleAttractor : EffectBehaviour
{
    [SerializeField]
    ParticleSystem m_psAttract;
    ParticleSystem[] m_aryParticleSystems;
    ParticleSystem.Particle[] m_Particles;
    [SerializeField]
    Renderer refRenderer;
    public Transform endPoint;
    public Vector3 endPosition = new Vector3(-1, -1, -1);
    public float speed = 5f;
    int numParticlesAlive;
    GameObject player;
    int playerNum;

    Vector3 m_prevPos;

    [SerializeField]
    bool startPointSeekParent = false;

    void Start()
    {
        m_aryParticleSystems = m_psAttract.GetComponentsInChildren<ParticleSystem>();
        m_prevPos = transform.position;

        // パーティクルの開始位置はキャラの頭を追いかけさせる
        if (refRenderer)
        {
            float posY = refRenderer.material.GetFloat("_Pos");
            transform.position = new Vector3(refRenderer.transform.position.x, posY, refRenderer.transform.position.z);
        }
    }
    void Update()
    {
        var deltaObjPos = transform.position - m_prevPos;
        m_prevPos = transform.position;

        foreach (var ps in m_aryParticleSystems)
        {
            m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
            numParticlesAlive = ps.GetParticles(m_Particles);
            float step = speed * Time.deltaTime;
            for (int i = 0; i < numParticlesAlive; i++)
            {
                var endPos = (endPosition != new Vector3(-1, -1, -1)) ? endPosition : endPoint.position;
                if (startPointSeekParent)
                    m_Particles[i].position += deltaObjPos;
                m_Particles[i].position = Vector3.SlerpUnclamped(m_Particles[i].position, endPos - transform.position, step);
            }
            ps.SetParticles(m_Particles, numParticlesAlive);
        }

        // パーティクルの開始位置はキャラの頭を追いかけさせる
        if (refRenderer)
        {
            float posY = refRenderer.material.GetFloat("_Pos");
            transform.position = new Vector3(refRenderer.transform.position.x, posY, refRenderer.transform.position.z);
        }
    }

    public override void SetAttachTarget(int num, string targetName)
    {
        switch (num)
        {
            case 1:
                if (targetName == "0" || targetName == "1")
                {
                    playerNum = (targetName == "0") ? 0 : 1;
                    var player = PlayerManager.Players[playerNum];
                    refRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
                }
                else
                {
                    var vrChara = GameObject.Find(targetName);
                    if (vrChara)
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
