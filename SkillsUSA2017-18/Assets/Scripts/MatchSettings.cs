﻿using UnityEngine.Networking;

[System.Serializable]
public class MatchSettings {
    public float respawnTime;
    //public GameMode gameMode;
    public int serverSize;
    public int mapWidth; // set minimums
    public int mapHeight;
    public bool randomMapSeed;
    public string mapSeed;
}