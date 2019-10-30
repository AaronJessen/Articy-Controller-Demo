using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OhmsLibraries.Tools.Editor {
    public class GizmosUtils {
        public static void DrawRect( Rect rect ) {
            Vector3 topLeft = new Vector3( rect.xMin, rect.yMax );
            Vector3 topRight = new Vector3( rect.xMax, rect.yMax );
            Vector3 bottomLeft = new Vector3( rect.xMin, rect.yMin );
            Vector3 bottomRight = new Vector3( rect.xMax, rect.yMin );
            Draw4Side( topLeft, topRight, bottomLeft, bottomRight );
        }

        public static void DrawRect( Rect rect, Transform transform ) {
            Vector3 topLeft = new Vector3( rect.xMin, rect.yMax ) + transform.position;
            Vector3 topRight = new Vector3( rect.xMax, rect.yMax ) + transform.position;
            Vector3 bottomLeft = new Vector3( rect.xMin, rect.yMin ) + transform.position;
            Vector3 bottomRight = new Vector3( rect.xMax, rect.yMin ) + transform.position;
            Draw4Side( topLeft, topRight, bottomLeft, bottomRight );
        }

        public static void Draw4Side( Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight ) {
            Gizmos.DrawLine( topLeft, topRight );
            Gizmos.DrawLine( topRight, bottomRight );
            Gizmos.DrawLine( bottomRight, bottomLeft );
            Gizmos.DrawLine( bottomLeft, topLeft );
        }
    }
}