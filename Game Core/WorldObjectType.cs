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
    /// The different types of objects, these are used to split up the CD calculations without having to do a IsSubclassOf()...
    /// </summary>
    public enum WorldObjectType
    {
        Undefined,
        Building,
        ProjectileNormal,
        ProjectileSmart,
        Playa,
        NPCSwarm,
        NPC_AN,
        Item,
        CompoundEffect,
        Trigger
    }
}
