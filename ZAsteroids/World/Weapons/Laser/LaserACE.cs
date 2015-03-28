using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZitaAsteria.World.Effects;
using ZitaAsteria;
using Microsoft.Xna.Framework;
using ZitaAsteria.World.Effects.DPSF;
using ZitaAsteria.MenuSystem;

namespace ZAsteroids.World.Weapons.Laser
{
    public class LaserACE : ACEParticleShaderListEffect
    {
        #region Public Properties
        /// <summary>
        /// The 3D location.
        /// </summary>
        public Vector3 Location3D { get; set; }
        /// <summary>
        /// The pointing vector/quaternion
        /// </summary>
        public Quaternion PointingVector { get; set; }

        #endregion

        #region Private Fields
        /// <summary>
        /// The particle systems.
        /// </summary>
        LaserZAPS portalUpwardPS = null;

        LaserPeripheryZAPS[] _peripheryPS = null;

        int _numPeripheryRays = 0;

        #endregion


        public LaserACE(Quaternion pointingVector, int numPeripheryRays)
        {
            this.IsUsingLifeSpan = false;
            this.LifeSpanMS = 5000;
            PointingVector = pointingVector;
            _numPeripheryRays = numPeripheryRays;
        }

        protected override List<AbstractBillboardZAPS> BuildParticleList()
        {
            if (portalUpwardPS == null)
                portalUpwardPS = new LaserZAPS(Color.White, 1.5f, 2.5f, 35, 35, 300, 1500, false, Vector3.Zero/*Vector3.UnitX * 10.0f*/);
            if (_peripheryPS == null)
            {
                _peripheryPS = new LaserPeripheryZAPS[_numPeripheryRays];
                for (int i = 0; i < _peripheryPS.Length; i++)
                {
                    _peripheryPS[i] = new LaserPeripheryZAPS(Color.PaleGoldenrod * 0.6f, 2, 3, 3, 5f, MenuContainer.Satellite.ObjectRotation, 2.0f * (float)Math.PI / _peripheryPS.Length * i);
                    _peripheryPS[i].IsBoundToParentACELocation = false;
                } 
            }
            portalUpwardPS.IsBoundToParentACELocation = false;
            List<AbstractBillboardZAPS> effects = new List<AbstractBillboardZAPS>();
            effects.Add(portalUpwardPS);
            for(int i = 0; i < _peripheryPS.Length; i++)
                effects.Add(_peripheryPS[i]);
            return effects;
        }

        protected override List<ZitaAsteria.World.ShaderEffects.ShaderEffect> BuildShaderList()
        {
            return new List<ZitaAsteria.World.ShaderEffects.ShaderEffect>();
        }

        /// <summary>
        /// Update the location 3D.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            (_particleSystems[0] as LaserZAPS).PointingVector = PointingVector;
            _particleSystems[0].Location = Location3D;
            _particleSystems[0].Emitter.PositionData.Position = Location3D;
            for (int i = 1; i < _particleSystems.Count; i++)
            {
                _particleSystems[i].Location = Location3D;
                _particleSystems[i].Emitter.PositionData.Position = Location3D;
                (_particleSystems[i] as LaserPeripheryZAPS).PointingVector = PointingVector;
            }
        }

    }
}
