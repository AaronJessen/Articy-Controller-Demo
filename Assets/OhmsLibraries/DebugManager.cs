using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugManager {
    public static void Log( object s ) {
#if DEBUG
        Debug.Log( s );
#endif
    }

    public static void LogFormat(string s, params object[] format ) {
#if DEBUG
        Debug.LogFormat( s, format );
#endif
    }
}
