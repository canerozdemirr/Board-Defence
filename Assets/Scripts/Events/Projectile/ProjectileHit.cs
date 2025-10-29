using Events.Interfaces;
using Gameplay.Objects.Entities;

namespace Events.Projectile
{
    public struct ProjectileHit : IEvent
    {
        public readonly ProjectileEntity Projectile;

        public ProjectileHit(ProjectileEntity projectile)
        {
            Projectile = projectile;
        }
    }
}
