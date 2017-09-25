using UnityEngine;
using System.Collections;

namespace Dreamteck.Splines
{
    [AddComponentMenu("Dreamteck/Splines/Spline Positioner")]
    public class SplinePositioner : SplineTracer
    {
        public enum Mode { Percent, Distance }
        
        [System.Obsolete("Deprecated in 1.0.8. Use targetObject instead")]
        public Transform applyTransform
        {
            get
            {
                return targetObject.transform;
            }

            set
            {
                if (value != null) targetObject = value.gameObject;
                else targetObject = null;
            }
        }

        public GameObject targetObject
        {
            get
            {
                if (_targetObject == null)
                {
                    if (_applyTransform != null) //Temporary check to migrate SplinePositioners that use applyTransform
                    {
                        _targetObject = _applyTransform.gameObject;
                        _applyTransform = null;
                        return _targetObject;
                    }
                    return gameObject;
                }
                return _targetObject;
            }

            set
            {
                if (value != _targetObject)
                {
                    _targetObject = value;
                    RefreshTargets();
                    Rebuild(false);
                }
            }
        }

        public double position
        {
            get
            {
                return _position;
            }
            set
            {
                if (value != _position)
                {
                    animPosition = (float)value;
                    _position = value;
                    Rebuild(false);
                }
            }
        }

        public Mode mode
        {
            get { return _mode;  }
            set
            {
                if (value != _mode)
                {
                    _mode = value;
                    Rebuild(false);
                }
            }
        }

        /// <summary>
        /// Returns the evaluation result at the current position
        /// </summary>
        [System.Obsolete("Deprecated in 1.0.8. Use result instead")]
        public SplineResult positionResult
        {
            get { return _result; }
        }

        /// <summary>
        /// Returns the offsetted evaluation result at the current position. 
        /// </summary>
        [System.Obsolete("Deprecated in 1.0.8. Use offsettedResult instead")]
        public SplineResult offsettedPositionResult
        {
            get
            {
                return offsettedResult;
            }
        }

        [SerializeField]
        [HideInInspector]
        private Transform _applyTransform;
        [SerializeField]
        [HideInInspector]
        private GameObject _targetObject;
        [SerializeField]
        [HideInInspector]
        private double _position = 0.0;
        [SerializeField]
        [HideInInspector]
        private float animPosition = 0f;
        [SerializeField]
        [HideInInspector]
        private Mode _mode = Mode.Percent;

        protected override void OnDidApplyAnimationProperties()
        {
            if (animPosition != _position) position = animPosition;
            base.OnDidApplyAnimationProperties();
        }

        protected override Transform GetTransform()
        {
            return targetObject.transform;
        }

        protected override Rigidbody GetRigidbody()
        {
            return targetObject.GetComponent<Rigidbody>();
        }

        protected override Rigidbody2D GetRigidbody2D()
        {
            return targetObject.GetComponent<Rigidbody2D>();
        }

        protected override void PostBuild()
        {
            base.PostBuild();
            double percent = _position;
            double lastPercent = _result.percent;
            if (mode == Mode.Distance) SetDistance((float)_position);
            else  SetPercent(percent);
            CheckTriggersClipped(lastPercent, percent);
            InvokeTriggers();
        }

    }
}
