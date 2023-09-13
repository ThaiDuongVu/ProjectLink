using UnityEngine;

public class PlayerCombat : CharacterCombat
{
    private Player _player;
    private PlayerMovement _playerMovement;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    #endregion
}
