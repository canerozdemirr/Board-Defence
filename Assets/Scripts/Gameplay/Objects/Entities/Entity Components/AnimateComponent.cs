using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Datas;
using Gameplay.Interfaces;
using PrimeTween;
using UnityEngine;

namespace Gameplay.Objects.Entities.Entity_Components
{
    public class AnimateComponent : BaseEntityComponent, IAnimateComponent
    {
        [SerializeField] private AnimationData[] _animationList;
        
        private Dictionary<string, AnimationData> _animationMap;

        public override void Initialize(IEntity entity)
        {
            base.Initialize(entity);

            _animationMap = new Dictionary<string, AnimationData>();
            foreach (AnimationData animationData in _animationList)
            {
                _animationMap[animationData.AnimationKey] = animationData;
            }
        }

        public void PlayAnimation(string animationKey)
        {
            if (!_animationMap.TryGetValue(animationKey, out AnimationData animationData))
                return;

            switch (animationData.TransformType)
            {
                case AnimationTransformType.Scale:
                    transform.localScale = animationData.StartValue;
                    Tween.Scale(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.Position:
                    transform.position = animationData.StartValue;
                    Tween.Position(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.LocalPosition:
                    transform.localPosition = animationData.StartValue;
                    Tween.LocalPosition(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.Rotation:
                    transform.rotation = Quaternion.Euler(animationData.StartValue);
                    Tween.Rotation(transform, Quaternion.Euler(animationData.EndValue), animationData.Duration, animationData.Ease);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async UniTask PlayAnimationAsync(string animationKey)
        {
            if (!_animationMap.TryGetValue(animationKey, out AnimationData animationData))
                return;

            Tween tween;
            switch (animationData.TransformType)
            {
                case AnimationTransformType.Scale:
                    transform.localScale = animationData.StartValue;
                    tween = Tween.Scale(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.Position:
                    transform.position = animationData.StartValue;
                    tween = Tween.Position(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.LocalPosition:
                    transform.localPosition = animationData.StartValue;
                    tween = Tween.LocalPosition(transform, animationData.EndValue, animationData.Duration, animationData.Ease);
                    break;
                case AnimationTransformType.Rotation:
                    transform.rotation = Quaternion.Euler(animationData.StartValue);
                    tween = Tween.Rotation(transform, Quaternion.Euler(animationData.EndValue), animationData.Duration, animationData.Ease);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            await tween.ToUniTask();
        }
    }
}
