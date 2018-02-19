using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {
    // todo: add camera behavior (low priority)
    // todo: player behavior
    public static GameManager instance;

    public PlayerSettings playerSettings;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    public float t;

	void Awake () {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        t += Time.deltaTime;
    }

    //private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    //public static void RegisterPlayer(string _netID, Player _player)
    //{
    //    string _playerID = PLAYER_ID_PREFIX + _netID;
    //    players.Add(_playerID, _player);
    //    _player.transform.name = _playerID;
    //}

    //public static void UnRegisterPlayer(string _playerID)
    //{
    //    players.Remove(_playerID);
    //}

    //public static Player GetPlayer(string _playerID)
    //{
    //    return players[_playerID];
    //}

    //public static Player[] GetAllPlayers()
    //{
    //    return players.Values.ToArray();
    //}
}
