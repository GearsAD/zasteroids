using System;
namespace ZitaAsteria.Scene
{
    public interface ICamera
    {
        Microsoft.Xna.Framework.Matrix GetProjectionMatrix(Microsoft.Xna.Framework.Graphics.Viewport viewPort);
        Microsoft.Xna.Framework.Matrix GetOrthographicProjectionMatrix(Microsoft.Xna.Framework.Graphics.Viewport viewPort);
        Microsoft.Xna.Framework.Vector3 GetRawCameraLocation();
        Microsoft.Xna.Framework.Matrix GetViewMatrix();
        Microsoft.Xna.Framework.Matrix GetShadowMapViewMatrix();
        //Microsoft.Xna.Framework.Matrix GetViewMatrixForTarget();
        Microsoft.Xna.Framework.Vector3 targetCameraLocation { get; set; }
        void Update(Microsoft.Xna.Framework.GameTime gameTime);
        void ApplyCameraShake(double shakeAmountPx);
        void InitializeCameraShakerCE();
    }
}
