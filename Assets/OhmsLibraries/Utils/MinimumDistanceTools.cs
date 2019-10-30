using UnityEngine;
using System.Collections.Generic;
namespace OhmsLibraries.Tools.Distance {
    public static class MinimumDistanceTools_Extensions {
        /// <summary>
        /// Valor para determinar si un punto está mas cerca respecto a otro usando una operación lineal derivada de 
        /// la ecuación de distancia, considerando que un punto es constante (origin).
        /// </summary>
        /// <remarks>La ecuación es: d(x) = x'z - 0.5(z'z); donde x = origin y z = other 
        /// y ambos son vectores columna y x' y z' son sus transpuestos (vector renglón).</remarks>
        /// <param name="origin">Lo posición origen con la que se harán las comparaciones.</param>
        /// <param name="other">La posición a ordenar.</param>
        /// <returns>Resultado de la operación, entre mayor el valor más cerca el punto other a origin.</returns>
        private static float MinimumDistanceCondition( Vector3 origin, Vector3 other ) {
            return ( Vector3.Dot( origin, other ) - 0.5f * Vector3.Dot( other, other ) );
        }

        /// <summary>
        /// Obtiene el Transform más próxima a origin.
        /// Si dos Transforms están a la misma distancia se retorna el primer encontrado.
        /// </summary>
        /// <param name="origin">El Transform de referencia</param>
        /// <param name="points">La lista de Transform que se buscará el más cercano.</param>
        /// <returns>El indice del transform más cercano en la lista.</returns>
        public static int GetClosest( this Transform origin, params Transform[] points ) {
            int index = 0;
            var comp = MinimumDistanceCondition( origin.position, points[0].position );
            for ( int i = 1; i < points.Length; i++ ) {
                var cond = MinimumDistanceCondition( origin.position, points[i].position );
                if ( cond > comp ) {
                    index = i;
                    comp = cond;
                }
            }
            return index;
        }

        /// <summary>
        /// Obtiene el Transform más próxima a origin.
        /// Si dos Transforms están a la misma distancia se retorna el primer encontrado.
        /// </summary>
        /// <param name="origin">El Transform de referencia</param>
        /// <param name="points">La lista de Transform que se buscará el más cercano.</param>
        /// <returns>El indice del transform más cercano en la lista.</returns>
        public static int GetClosest( this Transform origin, List<Transform> points ) {
            int index = 0;
            var comp = MinimumDistanceCondition( origin.position, points[0].position );
            for ( int i = 1; i < points.Count; i++ ) {
                var cond = MinimumDistanceCondition( origin.position, points[i].position );
                if ( cond > comp ) {
                    index = i;
                    comp = cond;
                }
            }
            return index;
        }

        /// <summary>
        /// Obtiene el Transform más lejano a origin.
        /// Si dos Transforms están a la misma distancia se retorna el primer encontrado.
        /// </summary>
        /// <param name="origin">El Transform de referencia</param>
        /// <param name="points">La lista de Transform que se buscará el más lejano.</param>
        /// <returns>El indice del transform más lejano en la lista.</returns>
        public static int GetFurthest( this Transform origin, Transform[] points ) {
            int index = 0;
            var comp = MinimumDistanceCondition( origin.position, points[0].position );
            for ( int i = 1; i < points.Length; i++ ) {
                var cond = MinimumDistanceCondition( origin.position, points[i].position );
                if ( cond < comp ) {
                    index = i;
                    comp = cond;
                }
            }
            return index;
        }

        /// <summary>
        /// Obtiene el Transform más lejano a origin.
        /// Si dos Transforms están a la misma distancia se retorna el primer encontrado.
        /// </summary>
        /// <param name="origin">El Transform de referencia</param>
        /// <param name="points">La lista de Transform que se buscará el más lejano.</param>
        /// <returns>El indice del transform más lejano en la lista.</returns>
        public static int GetFurthest( this Transform origin, List<Transform> points ) {
            int index = 0;
            var comp = MinimumDistanceCondition( origin.position, points[0].position );
            for ( int i = 1; i < points.Count; i++ ) {
                var cond = MinimumDistanceCondition( origin.position, points[i].position );
                if ( cond < comp ) {
                    index = i;
                    comp = cond;
                }
            }
            return index;
        }

        /// <summary>
        /// Get closests Transform from origin
        /// </summary>
        /// <param name="origin">The reference trnasform</param>
        /// <param name="points">The other transform to find the nearest.</param>
        /// <returns>The nearest index for the transform from origin.</returns>
        public static T GetClosest<T>( this Transform origin, Dictionary<T, Transform> points ) {
            T index = default;
            float? comp = null;
            var keys = points.Keys;
            List<float> indices = new List<float>();
            foreach ( var k in keys ) {
                var cond = MinimumDistanceCondition( origin.position, points[k].position );
                indices.Add( cond );
                if ( comp == null ) {
                    index = k;
                    comp = cond;
                }
                else {
                    if ( cond > comp ) {
                        index = k;
                        comp = cond;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// Get closests Transform from origin
        /// </summary>
        /// <param name="origin">The reference trnasform</param>
        /// <param name="points">The other transform to find the nearest.</param>
        /// <returns>The nearest index for the transform from origin.</returns>
        public static T GetClosest<T, W>( this Transform origin, Dictionary<T, W> points ) where W : MonoBehaviour {
            T index = default;
            float? comp = null;
            var keys = points.Keys;
            List<float> indices = new List<float>();
            foreach ( var k in keys ) {
                var cond = MinimumDistanceCondition( origin.position, points[k].transform.position );
                indices.Add( cond );
                if ( comp == null ) {
                    index = k;
                    comp = cond;
                }
                else {
                    if ( cond > comp ) {
                        index = k;
                        comp = cond;
                    }
                }
            }
            return index;
        }
    }
}