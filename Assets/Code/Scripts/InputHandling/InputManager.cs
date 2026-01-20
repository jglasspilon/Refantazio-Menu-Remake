using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private EInputMap m_startingInputMap;

    private GameInput m_input;
    private EInputMap m_currentInputMap;

    public GameInput InputActions
    {
        get
        {
            InitializeGameInput();
            return m_input;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeGameInput();
        SetInputMap(m_startingInputMap);
    }    

    public void SetInputMap(EInputMap inputMap)
    {
        m_currentInputMap = inputMap;
        m_input.Player.Disable();
        m_input.Menu.Disable();

        switch(inputMap)
        {          
            case EInputMap.Player:
                m_input.Player.Enable();
                break;
            case EInputMap.Menu:
                m_input.Menu.Enable();
                break;
        }
    }

    private void InitializeGameInput()
    {
        if (m_input == null)
        {
            m_input = new GameInput();
        }
    }
}

public enum EInputMap
{
    Player,
    Menu
}
