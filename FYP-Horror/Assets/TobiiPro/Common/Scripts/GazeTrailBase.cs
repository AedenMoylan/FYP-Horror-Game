//-----------------------------------------------------------------------
// Copyright © 2019 Tobii Pro AB. All rights reserved.
//-----------------------------------------------------------------------

using UnityEngine;

namespace Tobii.Research.Unity
{
    public class GazeTrailBase : MonoBehaviour
    {

        //[SerializeField]
        //[Tooltip("Turn gaze trail on or off.")]
        //public bool _on = true;

        ///// <summary>
        ///// Turn gaze trail on or off.
        ///// </summary>
        //public bool On
        //{
        //    get
        //    {
        //        return _on;
        //    }

        //    set
        //    {
        //        //_on = value;
        //        //OnSwitch();
        //    }
        //}





        //protected virtual bool HasEyeTracker { get; set; }
        //protected virtual bool CalibrationInProgress { get; set; }

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        private void Update()
        {
            //if (_particlesDirty)
            //{
            //    _particleSystem.SetParticles(_particles, _particles.Length);
            //    _particlesDirty = false;
            //}

            //CheckCount();

            //OnSwitch();

            //if (CalibrationInProgress)
            //{
            //    // Don't do anything if we are calibrating.

            //    //if (_removeParticlesWhileCalibrating)
            //    //{
            //    //    RemoveParticles();
            //    //    _removeParticlesWhileCalibrating = false;
            //    //}

            //    return;
            //}

            //// Reset the flag when no longer calibrating.
            //_removeParticlesWhileCalibrating = true;


        }

        //private void CheckCount()
        //{
        //    if (_lastParticleCount != _particleCount)
        //    {
        //        RemoveParticles();
        //        _particleIndex = 0;
        //        _particles = new ParticleSystem.Particle[_particleCount];
        //        _lastParticleCount = _particleCount;
        //    }
        //}

        //private void OnSwitch()
        //{
        //    if (_lastOn && !_on)
        //    {
        //        //// Switch off.
        //        //RemoveParticles();
        //        _lastOn = false;
        //    }
        //    else if (!_lastOn && _on)
        //    {
        //        // Switch on.
        //        _lastOn = true;
        //    }
        //}

        //private void RemoveParticles()
        //{
        //    for (int i = 0; i < _particles.Length; i++)
        //    {
        //        PlaceParticle(Vector3.zero, Color.white, 0);
        //    }
        //}

        //private void PlaceParticle(Vector3 pos, Color color, float size)
        //{
        //    if (_particleCount < 1)
        //    {
        //        return;
        //    }

        //    var particle = _particles[_particleIndex];
        //    particle.position = pos;
        //    particle.startColor = color;
        //    particle.startSize = size;
        //    _particles[_particleIndex] = particle;
        //    _particleIndex = (_particleIndex + 1) % _particles.Length;
        //    _particlesDirty = true;
        //}


    }
}