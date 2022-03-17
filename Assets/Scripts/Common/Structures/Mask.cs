using System;

namespace Charly.Data
{
    [Flags]
    public enum Mask : int
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