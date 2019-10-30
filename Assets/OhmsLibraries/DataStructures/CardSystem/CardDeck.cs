
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using OhmsLibraries.DataStructures;

namespace OhmsLibraries.GenericDataStructures.CardSystem {
    public class CardDeck<T> {
        public List<Card<T>> stack;
        private Range range;
        private int total;
        private int[] percentages;
        private bool infinite = true;
        public int Lenght {
            get {
                if ( infinite ) {
                    return -1;
                }
                return total;
            }
        }

        public CardDeck ( List<Card<T>> cards ) {

            this.stack = cards;
            percentages = new int[stack.Count];
            total = 0;
            for ( int i = 0; i < percentages.Length; i++ ) {
                percentages[i] = stack[i].appearance;
                total += percentages[i];
            }
            range = new Range( percentages );
        }

        public CardDeck ( bool infinite, List<Card<T>> cards ) : this( cards ) {
            this.infinite = infinite;
        }

        public CardDeck ( bool infinite, int overrideAppearance, List<Card<T>> cards ) {
            this.stack = cards;
            percentages = new int[stack.Count];
            total = 0;
            for ( int i = 0; i < percentages.Length; i++ ) {
                percentages[i] = overrideAppearance;
                total += percentages[i];
            }
            range = new Range( percentages );
            this.infinite = infinite;
        }

        public void Randomize() {
            var seed = DateTime.Now.Hour * 1000 + DateTime.Now.Minute * 100 + DateTime.Now.Second * 10 + DateTime.Now.Millisecond;
            UnityEngine.Random.InitState( seed );
            Debug.Log( $"Seed {seed}" );
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

        public void Reset () {
            range = new Range( percentages );
            total = 0;
            for(int i = 0; i < percentages.Length; i++ ) {
                total += percentages[i];
            }
        }
    }

    public partial class Card<T> {
        internal int appearance;
    }
}
