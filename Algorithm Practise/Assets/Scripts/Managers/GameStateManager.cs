public enum GameState
{
    None,
    NodeBuilder,
    NodePicker,
    Djikstra
}

public class GameStateManager 
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;
        }
    }

    public GameState CurrentGameState { get; private set; }
    public delegate void GameStateChangeHandler(GameState gameState);
    public event GameStateChangeHandler OnGameStateChange;

    private GameStateManager(){
        CurrentGameState = GameState.NodeBuilder;
    }

    public void SetGameState(GameState newGameState){
        if (newGameState == CurrentGameState) return;

        CurrentGameState = newGameState;
        OnGameStateChange?.Invoke(CurrentGameState);
    }
}
