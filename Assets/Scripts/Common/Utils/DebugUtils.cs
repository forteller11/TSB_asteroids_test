using System;
using UnityEngine;

namespace Charly.Common.Utils
{
    public class DebugUtils
    {
        public struct ColorFrame : IDisposable
        {
            private Color _colorCache;

            public ColorFrame(Color color)
            {
                _colorCache = Gizmos.color;
                Gizmos.color = color;
            }
            public void Dispose()
            {
                Gizmos.color = _colorCache;
            }
        }
    }
}