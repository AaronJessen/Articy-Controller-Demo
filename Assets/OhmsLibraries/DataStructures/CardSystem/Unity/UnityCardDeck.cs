using DataStructures.CardSystem;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR 
using Sirenix.OdinInspector;
#endif

namespace OhmsLibraries.DataStructures.CardSystem {
    public class CardDeck<T> : ScriptableObject {
#if ODIN_INSPECTOR
        [TableList]
#endif
        public Card<T>[] stack = new Card<T>[1];
        private Range range;
        private int total;
        private int[] percentages;
        [SerializeField]
        private bool infinite = true;
        public int Lenght {
            get {
                if ( infinite ) {
                    return -1;
                }
                return total;
            }
        }

        public void Build () {
            percentages = new int[stack.Length];
            total = 0;
            for ( int i = 0; i < percentages.Length; i++ ) {
                percentages[i] = stack[i].appearance;
                total += percentages[i];
            }
            range = new Range( percentages );
        }

        public Card<T> DrawCard () {
            var next = UnityEngine.Random.Range( 0, total );
            int i = range.GetRange( next );
            if ( !infinite ) {
                range.ModifyRange( i, -1 );
                total--;
            }
            return stack[i];
        }

        public Card<T>[] DrawCard ( int t ) {
            Card<T>[] cards = new Card<T>[t];
            for ( int i = 0; i < t; i++ ) {
                cards[i] = DrawCard();
            }
            return cards;
        }
    }

    //Talvez conceptualmente sería mejor separar la aparición de la clase Card.
    //public class DeckedCard<T> {
    //    public Card<T> card;
    //    public int percentage;
    //}
}