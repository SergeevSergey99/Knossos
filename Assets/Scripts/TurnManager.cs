using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public MinotaurController player;
    public List<MazeCharacter> cheliks;
    public int chelicksOD = 3;
    List<int> _chelicksOD = new List<int>();
    private bool _isPlayerTurn = true;
    public bool IsPlayerTurn() => _isPlayerTurn;
    public Button turnButton;

    private void Awake()
    {
        for (int i = 0; i < cheliks.Count; i++)
        {
            if(_chelicksOD.Count < cheliks.Count) _chelicksOD.Add(chelicksOD);
            else _chelicksOD[i] = chelicksOD;
        }
    }

    public void CheliksRandMove()
    {
        for (int i = 0; i < _chelicksOD.Count; i++)
        {
            if (_chelicksOD[i] > 0)
            {
                _chelicksOD[i]--;
                cheliks[i].MoveToRandomPossibleDir();
                break;
            }
        }

        StartCoroutine(WaitTillStop());
    }

    public void startCheliksTurn()
    {
        _isPlayerTurn = false;
        turnButton.interactable = false;
        player.LoseOG();
        player.ShowOG();
        Awake();

        CheliksRandMove();
    }

    bool IsCheliksStoped()
    {
        foreach (var chel in cheliks)
        {
            if (chel.transform.localPosition != (Vector3.zero) )
                return false;
        }

        return true;
    }

    int SumOD()
    {
        int sum = 0;
        foreach (var VARIABLE in _chelicksOD)
        {
            sum += VARIABLE;
        }

        return sum;
    }

    IEnumerator WaitTillStop()
    {
        while (!IsCheliksStoped())
        {
            yield return new WaitForSeconds(0.05f);
        }

        if (SumOD() > 0) CheliksRandMove();
        else
        {
            _isPlayerTurn = true;
            turnButton.interactable = true;
            player.UpdateOD();
        }
    }
}