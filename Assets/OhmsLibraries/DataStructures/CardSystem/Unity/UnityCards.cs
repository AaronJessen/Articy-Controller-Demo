using DataStructures.CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace OhmsLibraries.DataStructures.CardSystem {
    [System.Serializable]
    public class Card<T> {
        public T data;
        [DisableInPlayMode]
        public int category;
        public int appearance;

        public bool Corresponds<W> ( Card<W> other ) {
            return other.category == category;
        }

        public bool Corresponds ( int other ) {
            return other == category;
        }
    }

    #region CardData
    [System.Serializable]
    public class StringCardData {
        public string info;
    }
    [System.Serializable]
    public class StringIntCardData {
        public string text;
        public int value;
    }
    [System.Serializable]
    public class IntCardData {
        public int value;
    }
    #endregion
}