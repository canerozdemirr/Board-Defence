using System;
using PrimeTween;
using UnityEngine;

namespace Datas
{
    [Serializable]
    public class AnimationData
    {
        [SerializeField] private string _animationKey;
        [SerializeField] private AnimationTransformType _transformType;
        [SerializeField] private Vector3 _startValue;
        [SerializeField] private Vector3 _endValue;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        public string AnimationKey => _animationKey;
        public AnimationTransformType TransformType => _transformType;
        public Vector3 StartValue => _startValue;
        public Vector3 EndValue => _endValue;
        public float Duration => _duration;
        public Ease Ease => _ease;
    }

    public enum AnimationTransformType
    {
        Scale,
        Position,
        LocalPosition,
        Rotation
    }
}
