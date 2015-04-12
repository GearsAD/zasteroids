using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using ZitaAsteria.MenuSystem;
using Microsoft.Xna.Framework;
//using ZitaAsteria;

namespace ZitaAsteria
{
    /// <summary>
    /// A revised form of the sound effects manager [SDC].
    /// </summary>
    public class SoundEffectsMgr
    {
        
        public SoundEffectsMgr()
        {
        }


        /// <summary>
        /// Play the sound effect.
        /// </summary>
        /// <param name="soundEffect"></param>
        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            //Update the sound effect volume.
            SoundEffect.MasterVolume = WorldContainer.gameConfiguration.SoundVolumeNormalised;
            if (WorldContainer.gameConfiguration.SoundsEnabled)
            {
                if (soundEffect != null)
                {
                    SoundEffectInstance effectInstance = soundEffect.CreateInstance();
                    effectInstance.Play();
                }
            }
        }


        /// <summary>
        /// Play the sound effect with the distance calculation.
        /// </summary>
        /// <param name="soundEffect"></param>
        public void PlaySoundEffectInZAsteroids(SoundEffect soundEffect, Vector3 soundLocation)
        {
            if (WorldContainer.gameConfiguration.SoundsEnabled)
            {
                SoundEffectInstance effectInstance = soundEffect.CreateInstance();
                //Set the sound effect volume based upon the distance from the camera and using the size of the shields as the normalizing factor.
                float volume = WorldContainer.gameConfiguration.SoundVolumeNormalised * (1.0f - 0.9f * (MenuContainer.Camera.cameraLocation - soundLocation).Length() / (2.0f*MenuContainer.Shield.Radius));
                if (volume < 0) return;
                effectInstance.Volume = volume;
                if (effectInstance.Volume > 1.0f) effectInstance.Volume = 1.0f;
                effectInstance.Play();
            }
        }
    }
}

