using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Defines the current state
    public GameState State;

    public static event Action<GameState> OnGameStateChanged;
    
    void Awake()
    {
       Instance = this; 
    }

    void start() {
        UpdateGameState(GameState.BeforeSimulationStart);
    }

    public void UpdateGameState(GameState newGameState) {
        State = newGameState;

        switch (newGameState) {
            case GameState.BeforeSimulationStart:
                break;
            case GameState.SimulationRunning:
                break;
            case GameState.SimulationEnded:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }

        OnGameStateChanged?.Invoke(newGameState);
    }

    public enum GameState {
        BeforeSimulationStart,
        SimulationRunning,
        SimulationEnded
    }
}
