﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingRing : MonoBehaviour
{
    void Update()
    {
        ParticleSystem.EmissionModule ringEmission = this.GetComponent<ParticleSystem>().emission;
        ringEmission.enabled = true;

        ParticleSystem.ShapeModule ring = this.GetComponent<ParticleSystem>().shape;
        ring.radius += 7.0f * Time.deltaTime;

        Destroy(this.gameObject, this.GetComponent<ParticleSystem>().main.duration);
    }

    private void OnParticleCollision(GameObject other)
    {
    }
}