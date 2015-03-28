using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZitaAsteria.ObjectManagement
{
    public class ObjectPoolProfile
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        string updateProfile;

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(WorldContainer.graphicsDevice);
            spriteFont = WorldContent.contentManager.Load<SpriteFont>("SpriteFont1");
        }

        public void Update(GameTime gameTime)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Type key in ObjectManager.ObjectPools.Keys)
            {
                IObjectPool objectPool = ObjectManager.ObjectPools[key];
                sb.Append(key.Name);
                sb.Append(" : Initial Size = ");
                sb.Append(objectPool.InitialSize);
                sb.Append(",");
                sb.Append("Current Size = ");
                sb.Append(objectPool.CurrentSize);
            }
            
            updateProfile = sb.ToString();
        }


        public void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, updateProfile, new Vector2(600, 10), Color.White);

            spriteBatch.End();
        }
    }
}
