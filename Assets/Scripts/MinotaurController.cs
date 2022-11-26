using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    private TurnManager TM = null;
    private MazeCharacter MC = null;
    public int MinotaurOD = 3;
    int _MinotaurOD = 0;
    private void Awake()
    {
        UpdateOD();
        TM = FindObjectOfType<TurnManager>();
        MC = GetComponent<MazeCharacter>();
    }

    public void UpdateOD()
    {
        _MinotaurOD = MinotaurOD;
    }

    private bool isMoving = false;

    public void MoveTo(MazeCharacter.direction dir)
    {
        isMoving = true;
        if (_MinotaurOD > 0)
        {
            _MinotaurOD--;
            MC.MoveTo(dir);
        }

        StartCoroutine(WaitTillStop());
    }
    

    IEnumerator WaitTillStop()
    {
        while (transform.localPosition != Vector3.zero)
        {
            yield return new WaitForSeconds(0.05f);
        }

        isMoving = false;

        if (_MinotaurOD <= 0) 
        {
            TM.startCheliksTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!TM.IsPlayerTurn()) return;
        if(isMoving) return;
        
        var dirs = MC.GetPossible(false);
        if(Input.GetKeyDown(KeyCode.UpArrow) && dirs.Contains(MazeCharacter.direction.up)) MoveTo(MazeCharacter.direction.up);
        if(Input.GetKeyDown(KeyCode.DownArrow) && dirs.Contains(MazeCharacter.direction.down)) MoveTo(MazeCharacter.direction.down);
        if(Input.GetKeyDown(KeyCode.LeftArrow) && dirs.Contains(MazeCharacter.direction.left)) MoveTo(MazeCharacter.direction.left);
        if(Input.GetKeyDown(KeyCode.RightArrow) && dirs.Contains(MazeCharacter.direction.right)) MoveTo(MazeCharacter.direction.right);
    }
}
