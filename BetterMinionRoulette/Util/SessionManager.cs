namespace BetterMinionRoulette.Util;

using Dalamud.Plugin.Services;

using System;

internal sealed class SessionManager
{
    private readonly IClientState _clientState;
    private readonly IFramework _framework;
    private readonly IPlayerState _playerState;

    public SessionManager(IClientState clientState, IFramework framework, IPlayerState playerState)
    {
        _clientState = clientState;
        _framework = framework;
        _playerState = playerState;
    }

    private event Action? LoginInternal;
    public event Action? Login
    {
        add
        {
            LoginInternal += value;
            if (LoginInternal == value)
            {
                _clientState.Login += ClientStateLogin;
            }
        }
        remove
        {
            LoginInternal -= value;
            if (LoginInternal == null)
            {
                _clientState.Login -= ClientStateLogin;
            }
        }
    }

    private void ClientStateLogin()
    {
        _framework.Update += OnFrameworkUpdate;
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        if (!_playerState.IsLoaded)
        {
            return;
        }

        _framework.Update -= OnFrameworkUpdate;
        LoginInternal?.Invoke();
    }
}
