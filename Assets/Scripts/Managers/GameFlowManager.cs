using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : Singleton<GameFlowManager>
{
    public enum GameState
    {
        encounter, map_navigation
    }
    private GameState _gameState = GameState.encounter;

    public EncounterManager encounterManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameState = GameState.encounter;
        GameDataManager.Instance.LoadData("GameConfigs\\sample_gameconfig");
        HandleGameStateTransition("encounter_01");
    }

    private void HandleGameStateTransition(string transitionStateName = "")
    {
        switch (_gameState)
        {
            case GameState.encounter:
                encounterManager.SetEncounter(transitionStateName);
                encounterManager.gameObject.SetActive(true);
                break;
        }
    }
}
