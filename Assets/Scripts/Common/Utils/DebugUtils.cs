using System;
using System.Collections.Generic;
using UnityEngine;

namespace Charly.Common.Utils
{
    public class DebugUtils
    {
        public static Stack<Color> _colorsHistory = new Stack<Color>(3);
        public static void GizmosPushColor(Color color)
        {
            _colorsHistory.Push(color);
            Gizmos.color = color;
        }

        public static bool GizmosPopColor()
        {
            bool couldPop = _colorsHistory.TryPop(out var color);
            Gizmos.color = couldPop ? 
                color : 
                Color.magenta;

            return couldPop;
        }

        public static ColorFrame CreateGizmoColorFrame(Color color)
        {
            return new ColorFrame(color);
        }
        
        public struct ColorFrame : IDisposable
        {
            private Color _colorCache;

            public ColorFrame(Color colorCache)
            {
                _colorCache = Gizmos.color;
                Gizmos.color = colorCache;
            }
            public void Dispose()
            {
                Gizmos.color = _colorCache;
            }
        }
    }
}