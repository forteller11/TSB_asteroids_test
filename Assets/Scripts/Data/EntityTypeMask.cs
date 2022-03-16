using UnityEngine;

namespace Charly.Data
{
    //Note: I don't think using enums or flags for branches are code smells in data-oriented coding like they are in OOP, they're honest and simple.
    public struct EntityTypeMask
    {
        public GameType Type;
    }

    public enum GameType
    {
        None     = 0b_0000_0000,
        
        Enemy    = 0b_0000_0001,
        Friend   = 0b_0000_0010,

        Asteroid = 0b_0000_0100,
        UFO      = 0b_0000_1000,
        Ship     = 0b_0001_0000,
        Bullet   = 0b_0010_0000,
    }
}