using UnityEngine;
using System.Collections;
namespace OhmsLibraries.Tools.PolarCoordinates.Sphere {
    public abstract class PolarCoordinate {

        public abstract Vector3 ToVector3();
    }

    public class SphereCoordinate : PolarCoordinate {
        public float longitude;
        public float latitude;
        public float magnitude;

        public SphereCoordinate( Vector3 vector ) {
            magnitude = vector.magnitude;
            if ( vector.x == 0 ) {
                if ( vector.z == 0f ) {
                    longitude = 0f;
                }
                else if ( vector.z > 0f ) {
                    longitude = 90f;
                }
                else {
                    longitude = -90f;
                }
            }
            else {
                longitude = Mathf.Atan( vector.z / vector.x ) * Mathf.Rad2Deg;
            }
            latitude = Mathf.Asin( vector.y / magnitude ) * Mathf.Rad2Deg;
            if ( vector.x < 0f && vector.z < 0f ) {
                longitude += 270f;
            }
            else if ( vector.x < 0f ) {
                longitude += 180f;
            }

        }

        public SphereCoordinate() {
            latitude = 0;
            longitude = 0;
            magnitude = 0;
        }

        public void UpdateVar() {
            if ( longitude > 360f ) {
                longitude -= 360f;
            }
            else if ( longitude < 0f ) {
                longitude += 360f;
            }
        }

        public void UpdateCoord( Vector3 vector ) {
            magnitude = vector.magnitude;
            if ( vector.x == 0 ) {
                if ( vector.z == 0f ) {
                    longitude = 0f;
                }
                else if ( vector.z > 0f ) {
                    longitude = 90f;
                }
                else {
                    longitude = -90f;
                }
            }
            else {
                longitude = Mathf.Atan( vector.z / vector.x ) * Mathf.Rad2Deg;
            }
            latitude = Mathf.Asin( vector.y / magnitude ) * Mathf.Rad2Deg;
            if ( vector.x < 0f && vector.z < 0f ) {
                longitude += 270f;
            }
            else if ( vector.x < 0f ) {
                longitude += 180f;
            }

        }

        public override Vector3 ToVector3() {
            float ro = magnitude * Mathf.Cos( latitude * Mathf.Deg2Rad );
            float x = ro * Mathf.Cos( longitude * Mathf.Deg2Rad );
            float y = magnitude * Mathf.Sin( latitude * Mathf.Deg2Rad );
            float z = ro * Mathf.Sin( longitude * Mathf.Deg2Rad );
            return new Vector3( x, y, z );
        }

        public override string ToString() {
            return string.Format("Lat: {0} Lon;{1}, Mag:{2}", latitude, longitude, magnitude);
        }
    }

    public static class TrasnformPolarExtensions {
        /// <summary>
        /// Gets the position as sphere coordinate. 
        /// Better to use this method only once a manipulate the returned object, unless the center is changed.
        /// </summary>
        /// <returns>The position as a SphereCoordinate</returns>
        /// <param name="center">The referenceCenter.</param>
        public static SphereCoordinate GetSpherePosition( this Transform transform, Vector3 center ) {
            var sphere = new SphereCoordinate( transform.position - center );
            //Debug.Log( sphere.ToString() );
            return sphere;
        }

        public static SphereCoordinate GetSpherePosition( this Transform transform ) {
            return GetSpherePosition( transform, new Vector3( 0, 0, 0 ) );
        }

        public static void SetSpherePosition( this Transform transform, SphereCoordinate coordinate, Vector3 center){
            //Debug.Log( coordinate.ToString() );
            transform.position = coordinate.ToVector3() + center;
        }

        public static void SetSpherePosition( this Transform transform, SphereCoordinate coordinate ) {
            SetSpherePosition( transform, coordinate, new Vector3( 0, 0, 0 ) );
        }

        

    }

}

public static class TransformExtensions {
    public static Vector2 Rotate( this Vector2 v, float degrees ) {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin( radians );
        float cos = Mathf.Cos( radians );

        float tx = v.x;
        float ty = v.y;

        return new Vector2( cos * tx - sin * ty, sin * tx + cos * ty );
    }
}