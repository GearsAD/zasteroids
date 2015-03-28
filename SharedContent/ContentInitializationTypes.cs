using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContent
{
    /// <summary>
    /// What types of content to initialize - used by the different tools in the game engine.
    /// </summary>
    public enum ContentInitializationTypes
    {
        LevelContent = 1,
        MenuContent = 2,
        SfxContent = 4,
        MusicContent = 8,
        CharacterContent = 16,
        BuildingContent = 32,
        ItemContent = 64,
        EffectsContent = 128,
        WeaponContent = 256,
        All = 511
    }
}
