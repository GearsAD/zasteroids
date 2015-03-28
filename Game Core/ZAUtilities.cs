using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace ZitaAsteria
{
    public static class ZAUtilities
    {
        public static Array GetValuesFromEnum<T>()
        {
            Type enumType = typeof(T);
            FieldInfo[] fields = enumType.GetFields();

            List<T> enumValues = new List<T>();

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == typeof(T))
                    enumValues.Add((T)Enum.Parse(typeof(T), fields[i].Name, false));
            }

            Array array = enumValues.ToArray();

            return array;
        }

        public static bool HasPlayerJoined(PlayerIndex playerIndex)
        {
            return WorldContainer.PlayersJoined[(int)playerIndex];
        }

        public static bool IsPlayerActive(PlayerIndex playerIndex)
        {
            //if (WorldContainer.playerActive != null)
            //{
            //    if (WorldContainer.playerActive.ContainsKey((int)playerIndex))
            //        return WorldContainer.playerActive[(int)playerIndex];
            //}

            return false;
        }
    }
}
