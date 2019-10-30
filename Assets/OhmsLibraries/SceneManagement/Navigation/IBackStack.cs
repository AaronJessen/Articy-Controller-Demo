namespace OhmsLibraries.SceneManagement.Navigation {
    public interface IBackStack {
        void Register( System.Action action, System.Action inverse );
    }
}