public class InputManager : Singleton<InputManager>
{
    private GameInput m_input;
    private GameStateManager m_gameState;

    public GameInput InputActions
    {
        get
        {
            Initialize();
            return m_input;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
        SetInputMap(m_gameState.CurrentState);
    }

    private void OnEnable()
    {
        m_gameState.OnGameStateChanged += SetInputMap;
    }

    private void OnDisable()
    {
        m_gameState.OnGameStateChanged -= SetInputMap;
    }

    public void SetInputMap(EGameState state)
    {
        m_input.Field.Disable();
        m_input.Menu.Disable();

        switch(state)
        {          
            case EGameState.Field:
                m_input.Field.Enable();
                break;
            case EGameState.Menu:
                m_input.Menu.Enable();
                break;
        }
    }

    private void Initialize()
    {
        if (m_input == null)
        {
            m_input = new GameInput();
        }

        if(m_gameState == null)
        {
            m_gameState = GameStateManager.Instance;
        }
    }
}

public enum EInputMap
{
    Field,
    Menu
}
