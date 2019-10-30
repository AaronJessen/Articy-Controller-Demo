namespace Articy.Unity {
    public static class ArticyExtensions {
        public static void SetVariableByGameState( this BaseGlobalVariables variables, GameState state ) {
            variables.SetVariableByString( state.Id, state.Value );
        }
    }
}