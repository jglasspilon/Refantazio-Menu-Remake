using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private GameInput m_input;

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
    }    

    public void SetToMenuInputs()
    {
        m_input.Player.Disable();
        m_input.Menu.Enable();
    }

    public void SetToPlayerInputs()
    {
        m_input.Menu.Disable();
        m_input.Player.Enable();       
    }

    private void InitializeGameInput()
    {
        if (m_input == null)
        {
            m_input = new GameInput();
        }
    }
}
