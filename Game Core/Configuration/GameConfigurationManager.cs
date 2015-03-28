using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Storage;

namespace ZitaAsteria
{
    public class GameConfigurationManager
    {
        public bool IsGamePadConnected { get; set; }

        private const string ConfigFileName = "game.config";
        private const string ContainerName = "ZitaAsteria";

        private StorageDevice device;
        private Object stateobj;

        public GameConfigurationManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            //CheckStatus();
            //GetDevice();
        }

        public void CheckStatus()
        {
            IsGamePadConnected = GamePad.GetCapabilities(Microsoft.Xna.Framework.PlayerIndex.One).IsConnected;
        }

        private void GetDevice()
        {
            stateobj = (object)"GetDevice for Player One";

            StorageDevice.BeginShowSelector(PlayerIndex.One, new AsyncCallback((result) =>
            {
                device = StorageDevice.EndShowSelector(result);

            }), stateobj);
        }

        public void LoadConfiguration()
        {
            WorldContainer.gameConfiguration = new GameConfiguration();

            //// Open a storage container.
            //IAsyncResult result =
            //    device.BeginOpenContainer(ContainerName, null, null);

            //// Wait for the WaitHandle to become signaled.
            //result.AsyncWaitHandle.WaitOne();

            //using (StorageContainer container = device.EndOpenContainer(result))
            //{
            //    // Close the wait handle.
            //    result.AsyncWaitHandle.Close();

            //    string filename = ConfigFileName;

            //    // Check to see whether the save exists.
            //    if (container.FileExists(filename))
            //    {

            //        XmlSerializer serializer = new XmlSerializer(typeof(GameConfiguration));

            //        using (Stream fileStream = container.OpenFile(ConfigFileName, FileMode.Open))
            //        {
            //            WorldContainer.gameConfiguration = (GameConfiguration)serializer.Deserialize(fileStream);
            //        }
            //    }
            //    else
            //    {
            //        WorldContainer.gameConfiguration = new GameConfiguration();
            //    }
            //}
        }

        public void SaveConfiguration()
        {
            //// Open a storage container.
            //IAsyncResult result =
            //    device.BeginOpenContainer(ContainerName, null, null);

            //// Wait for the WaitHandle to become signaled.
            //result.AsyncWaitHandle.WaitOne();

            //using (StorageContainer container = device.EndOpenContainer(result))
            //{
            //    // Close the wait handle.
            //    result.AsyncWaitHandle.Close();

            //    string filename = ConfigFileName;

            //    // Check to see whether the save exists.
            //    if (container.FileExists(filename))
            //        // Delete it so that we can create one fresh.
            //        container.DeleteFile(filename);

            //    XmlSerializer serializer = new XmlSerializer(typeof(GameConfiguration));

            //    // Create the file.
            //    using (Stream stream = container.CreateFile(filename))
            //    {
            //        serializer.Serialize(stream, WorldContainer.gameConfiguration);
            //    }
            //}
         
        }
    }
}
