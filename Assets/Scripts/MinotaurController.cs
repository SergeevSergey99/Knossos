using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MinotaurController : MonoBehaviour
{
    private TurnManager TM = null;
    private MazeCharacter MC = null;
    public int MinotaurOD = 3;
    int _MinotaurOD = 0;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        UpdateOD();
        TM = FindObjectOfType<TurnManager>();
        MC = GetComponent<MazeCharacter>();
    }

    public void UpdateOD()
    {
        _MinotaurOD = MinotaurOD;
    }

    private bool _isMoving = false;

    public void AnimStop()
    {
        _isMoving = false;
    }

    public void MoveTo(MazeCharacter.direction dir)
    {   
        _isMoving = true;
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

        _isMoving = false;

        if (_MinotaurOD <= 0)
        {
            TM.startCheliksTurn();
        }
    }

    public void ActiveGear90()
    {
        transform.parent.parent.GetComponent<RotatableTiles>().RotateAllOfSameType90();
    }

    public void ActiveGear_90()
    {
        transform.parent.parent.GetComponent<RotatableTiles>().RotateAllOfSameType_90();
    }

    bool HasNodeGear()
    {
        if (MC.GetCurrNode().hasCenter && MC.GetCurrNode().center.isGear)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TM.IsPlayerTurn()) return;
        if (_isMoving) return;

        var dirs = MC.GetPossible(false);
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&
            dirs.Contains(MazeCharacter.direction.up)) MoveTo(MazeCharacter.direction.up);
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&
            dirs.Contains(MazeCharacter.direction.down)) MoveTo(MazeCharacter.direction.down);
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) &&
            dirs.Contains(MazeCharacter.direction.left)) MoveTo(MazeCharacter.direction.left);
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) &&
            dirs.Contains(MazeCharacter.direction.right)) MoveTo(MazeCharacter.direction.right);
        if (Input.GetKeyDown(KeyCode.Q) && HasNodeGear())
        {
            _isMoving = true;
            animator.Play("ActiveGear90");
        }

        if (Input.GetKeyDown(KeyCode.E) && HasNodeGear())
        {
            _isMoving = true;
            animator.Play("ActiveGear_90");
        }
    }
}