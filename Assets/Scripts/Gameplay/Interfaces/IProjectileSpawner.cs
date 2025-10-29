using Cysharp.Threading.Tasks;
using Gameplay.Objects.Entities;
using UnityEngine;

namespace Gameplay.Interfaces
{
    public interface IProjectileSpawner
    {
        UniTask<ProjectileEntity> ProvideProjectileEntity(string projectileName, Vector3 spawnPosition);
        void ReturnProjectileToPool(ProjectileEntity projectile);
    }
}
