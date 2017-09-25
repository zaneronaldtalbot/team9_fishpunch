using UnityEngine;
using System.Collections;

namespace Dreamteck.Splines
{
    [ExecuteInEditMode]
    [AddComponentMenu("Dreamteck/Splines/Particle Controller")]
    public class ParticleController : SplineUser
    {
        public class Particle
        {
            public Vector2 startOffset = Vector2.zero;
            public Vector2 endOffset = Vector2.zero;
            public float speed = 0f;
            public double splinePercent = 0f;
            public float lifePercent = 0f;
        }

        [HideInInspector]
        public ParticleSystem _particleSystem;
        public enum EmitPoint { Beginning, Ending, Random, Ordered }

        public enum MotionType { None, UseParticleSystem, FollowForward, FollowBackward, ByNormal, ByNormalRandomized }

        public enum Wrap { Default, Loop }

        [HideInInspector]
        public bool volumetric = false;
        [HideInInspector]
        public bool emitFromShell = false;
        [HideInInspector]
        public Vector2 scale = Vector2.one;
        [HideInInspector]
        public EmitPoint emitPoint = EmitPoint.Beginning;
        [HideInInspector]
        public MotionType motionType = MotionType.UseParticleSystem;
        [HideInInspector]
        public Wrap wrapMode = Wrap.Default;
        [HideInInspector]
        public float minCycles = 1f;
        [HideInInspector]
        public float maxCycles = 2f;

        private ParticleSystem.Particle[] particles = null;
        private ParticleController.Particle[] particleControllers = null;
        private int particleCount = 0;
        private int birthIndex = 0;
        SplineResult evaluateResult = new SplineResult();

        protected override void Awake() 
        {
            base.Awake();
        }

        protected override void LateRun()
        {
            if (_particleSystem == null) return;
#if UNITY_5_5_OR_NEWER
            int maxParticles = _particleSystem.main.maxParticles;
#else
            int maxParticles = _particleSystem.maxParticles;
#endif
            if (particles == null || particles.Length != maxParticles)
            {
                particles = new ParticleSystem.Particle[maxParticles];
                if (particleControllers == null) particleControllers = new ParticleController.Particle[maxParticles];
                else
                {
                    ParticleController.Particle[] newControllers = new ParticleController.Particle[maxParticles];
                    for (int i = 0; i < newControllers.Length; i++)
                    {
                        if (i >= particleControllers.Length) break;
                        newControllers[i] = particleControllers[i];
                    }
                    particleControllers = newControllers;
                }
            }
            particleCount = _particleSystem.GetParticles(particles);
#if UNITY_5_5_OR_NEWER
            bool isLocal = _particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local;
#else
            bool isLocal = _particleSystem.simulationSpace == ParticleSystemSimulationSpace.Local;
#endif

            Transform particleSystemTransform = _particleSystem.transform;
            for (int i = 0; i < particleCount; i++)
            {
#if UNITY_5_5_OR_NEWER
                if (particles[i].remainingLifetime <= 0f) continue;
#else
                if (particles[i].lifetime <= 0f) continue;
#endif
                if (isLocal)
                {
                    particles[i].position = particleSystemTransform.TransformPoint(particles[i].position);
                    particles[i].velocity = particleSystemTransform.TransformDirection(particles[i].velocity);
                }
                HandleParticle(i);
                if (isLocal)
                {
                    particles[i].position = particleSystemTransform.InverseTransformPoint(particles[i].position);
                    particles[i].velocity = particleSystemTransform.InverseTransformDirection(particles[i].velocity);
                }
            }
            _particleSystem.SetParticles(particles, particleCount);
        }

        protected override void Reset()
        {
            base.Reset();
            if (_particleSystem == null) _particleSystem = GetComponent<ParticleSystem>();
        }

        void HandleParticle(int index)
        {
#if UNITY_5_5_OR_NEWER
            float lifePercent = particles[index].remainingLifetime / particles[index].startLifetime;
#else
            float lifePercent = particles[index].lifetime / particles[index].startLifetime;
#endif
            if (particleControllers[index] == null)
            {
                particleControllers[index] = new ParticleController.Particle();
            }
            if (lifePercent > particleControllers[index].lifePercent) OnParticleBorn(index);

            float lifeDelta = particleControllers[index].lifePercent - lifePercent;

            switch (motionType)
            {
                case MotionType.FollowForward: particleControllers[index].splinePercent += lifeDelta * particleControllers[index].speed; break;
                case MotionType.FollowBackward: particleControllers[index].splinePercent -= lifeDelta * particleControllers[index].speed; break;
            }

            if (particleControllers[index].splinePercent < 0f)
            {
                if (wrapMode == Wrap.Loop) particleControllers[index].splinePercent += 1f;
                if (wrapMode == Wrap.Default) particleControllers[index].splinePercent = 0f;
            }
            else if (particleControllers[index].splinePercent > 1f)
            {
                if (wrapMode == Wrap.Loop) particleControllers[index].splinePercent -= 1f;
                if (wrapMode == Wrap.Default) particleControllers[index].splinePercent = 1f;
            }

            if (motionType == MotionType.FollowBackward || motionType == MotionType.FollowForward || motionType == MotionType.None)
            {
                Evaluate(evaluateResult, UnclipPercent(particleControllers[index].splinePercent));
                particles[index].position = evaluateResult.position;
                if (volumetric)
                {
                    Vector3 right = -Vector3.Cross(evaluateResult.direction, evaluateResult.normal);
                    Vector2 offset = particleControllers[index].startOffset;
                    if (motionType != MotionType.None) offset = Vector2.Lerp(particleControllers[index].startOffset, particleControllers[index].endOffset, 1f - particleControllers[index].lifePercent);
                    particles[index].position += right * offset.x * scale.x * evaluateResult.size + evaluateResult.normal * offset.y * scale.y * evaluateResult.size;
                }
                particles[index].velocity = evaluateResult.direction;
            }

            particleControllers[index].lifePercent = lifePercent;
        }


        void OnParticleBorn(int index)
        {
            birthIndex++;
            double percent = 0.0;
#if UNITY_5_5_OR_NEWER
            float emissionRate = Mathf.Lerp(_particleSystem.emission.rateOverTime.constantMin, _particleSystem.emission.rateOverTime.constantMax, 0.5f);
#elif UNITY_5_3 || UNITY_5_3_OR_NEWER
            float emissionRate = Mathf.Lerp(_particleSystem.emission.rate.constantMin, _particleSystem.emission.rate.constantMax, 0.5f);
#else
            float emissionRate = _particleSystem.emissionRate;
#endif

#if UNITY_5_5_OR_NEWER
            float expectedParticleCount = emissionRate * _particleSystem.main.startLifetime.constantMax;
#else
            float expectedParticleCount = emissionRate * _particleSystem.startLifetime;
#endif

            if (birthIndex > expectedParticleCount) birthIndex = 0;
            switch (emitPoint)
            {
                case EmitPoint.Beginning: percent = 0f; break;
                case EmitPoint.Ending: percent = 1f; break;
                case EmitPoint.Random: percent = Random.Range(0f, 1f); break;
                case EmitPoint.Ordered: percent = expectedParticleCount > 0 ? (float)birthIndex / expectedParticleCount : 0f;  break;
            }
            Evaluate(evaluateResult, UnclipPercent(percent));
            particleControllers[index].splinePercent = percent;
          
            particleControllers[index].speed = Random.Range(minCycles, maxCycles);
            Vector2 circle = Vector2.zero;
            if (volumetric)
            {
                if (emitFromShell) circle = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector2.right;
                else circle = Random.insideUnitCircle;
            }
            particleControllers[index].startOffset = circle * 0.5f;
            particleControllers[index].endOffset = Random.insideUnitCircle * 0.5f;


            Vector3 right = Vector3.Cross(evaluateResult.direction, evaluateResult.normal);
            particles[index].position = evaluateResult.position + right * particleControllers[index].startOffset.x * evaluateResult.size * scale.x + evaluateResult.normal * particleControllers[index].startOffset.y * evaluateResult.size * scale.y;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
            float forceX = _particleSystem.forceOverLifetime.x.constantMax;
            float forceY = _particleSystem.forceOverLifetime.y.constantMax;
            float forceZ = _particleSystem.forceOverLifetime.z.constantMax;
            if (_particleSystem.forceOverLifetime.randomized)
            {
                forceX = Random.Range(_particleSystem.forceOverLifetime.x.constantMin, _particleSystem.forceOverLifetime.x.constantMax);
                forceY = Random.Range(_particleSystem.forceOverLifetime.y.constantMin, _particleSystem.forceOverLifetime.y.constantMax);
                forceZ = Random.Range(_particleSystem.forceOverLifetime.z.constantMin, _particleSystem.forceOverLifetime.z.constantMax);
            }

#if UNITY_5_5_OR_NEWER
            float time = particles[index].startLifetime - particles[index].remainingLifetime;
#else
            float time = particles[index].startLifetime - particles[index].lifetime;
#endif
            Vector3 forceDistance = new Vector3(forceX, forceY, forceZ) * 0.5f * (time * time);

#if UNITY_5_5_OR_NEWER
            float startSpeed = _particleSystem.main.startSpeed.constantMax;
#else
            float startSpeed = _particleSystem.startSpeed;
#endif

            if (motionType == MotionType.ByNormal)
            {
#if UNITY_5_5_OR_NEWER
                particles[index].position += evaluateResult.normal * startSpeed * (particles[index].startLifetime - particles[index].remainingLifetime);
#else
            particles[index].position += evaluateResult.normal * startSpeed * (particles[index].startLifetime - particles[index].lifetime);
#endif
                particles[index].position += forceDistance;
                particles[index].velocity = evaluateResult.normal * startSpeed + new Vector3(forceX, forceY, forceZ) * time;
            }
            else if (motionType == MotionType.ByNormalRandomized)
            {
                Vector3 normal = Quaternion.AngleAxis(Random.Range(0f, 360f), evaluateResult.direction) * evaluateResult.normal;
#if UNITY_5_5_OR_NEWER
                particles[index].position += normal * startSpeed * (particles[index].startLifetime - particles[index].remainingLifetime);
#else
                particles[index].position += normal * startSpeed * (particles[index].startLifetime - particles[index].lifetime);
#endif
                particles[index].position += forceDistance;
                particles[index].velocity = normal * startSpeed + new Vector3(forceX, forceY, forceZ) * time;
            }
#endif
        }
    }
}
