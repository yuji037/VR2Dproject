using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartColorController : MonoBehaviour {

  ParticleSystem myParticleSystem;
  ParticleSystem.EmissionModule RateModule;
  float Rate = 50;

  void Start()
  {
    myParticleSystem = GetComponent<ParticleSystem>();
    RateModule = myParticleSystem.emission;

  }

  void Update()
  {
    Rate += Time.deltaTime * 20;
    RateModule.rate = new ParticleSystem.MinMaxCurve(Rate);
  }
}
