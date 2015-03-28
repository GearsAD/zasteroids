using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ZitaAsteria.World
{
    /// <summary>
    /// The set of different collision types.
    /// </summary>
    public enum PhysicsCollisionType
    {
        /// <summary>
        /// You collided with a player
        /// </summary>
        PlayaCollision,
        /// <summary>
        /// You collided with a building.
        /// </summary>
        BuildingCollision,
        /// <summary>
        /// You collided with the level.
        /// </summary>
        LevelCollision,
        /// <summary>
        /// You collided with an NPC.
        /// </summary>
        NPCCollision,
        /// <summary>
        /// You collided with a projectile (ouch!).
        /// </summary>
        ProjectileCollision,
        /// <summary>
        /// The object existed for too long, used to delete bullets...
        /// </summary>
        LifespanOverCollision,
        /// <summary>
        /// The object collided with an item.
        /// </summary>
        ItemCollision,
        /// <summary>
        /// This is used for turretWeapons that cause splash damage, will cause damage based upon distance.
        /// </summary>
        ExplosionShockwaveCollision

    }
}
