using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MinotaurController : MonoBehaviour
{
    private TurnManager TM = null;
    private MazeCharacter MC = null;
    public int MinotaurOD = 3;
    public int HungerOG = 5;
    PointsController MinotaurOD_UI;
    PointsController HungerOG_UI;
    int _MinotaurOD = 0;
    int _HungerOG = 0;

    public MazeCharacter GetMC() => MC;

    private void Awake()
    {
        TM = FindObjectOfType<TurnManager>();
        MC = GetComponent<MazeCharacter>();
        foreach (var PC in FindObjectsOfType<PointsController>())
        {
            if (PC.pointType == PointsController.PointType.OD)
                MinotaurOD_UI = PC;
            else
                HungerOG_UI = PC;
        }
        UpdateOD();
        UpdateOG();
    }

    public void UpdateOD()
    {
        _MinotaurOD = MinotaurOD;
        MinotaurOD_UI.SetPoints(_MinotaurOD);
    }
    public void UpdateOG()
    {
        _HungerOG = HungerOG;
        ShowOG();
    }
    public void ShowOG()
    {
        HungerOG_UI.SetPoints(_HungerOG);
    }

    public void LoseOG()
    {
        _HungerOG--;
    }

    public int GetOG() => _HungerOG;

    private bool _isMoving = false;

    public void AnimStop()
    {
        _isMoving = false;
    }

    public void MoveTo(MazeCharacter.direction dir)
    {   
        if (_MinotaurOD > 0)
        {
            _isMoving = true;
            _MinotaurOD--;
            MinotaurOD_UI.SetPoints(_MinotaurOD);
            if(MC.HanCheliks(dir))
                TM.ZoomToMino();
            MC.MoveTo(dir);
            StartCoroutine(WaitTillStop());
        }

    }


    IEnumerator WaitTillStop()
    {   
        
        while (transform.localPosition != Vector3.zero)
        {
            yield return new WaitForSeconds(0.05f);
        }

        _isMoving = false;
/*
        foreach (var chel in TM.cheliks)
        {
            if (MC.GetCurrNode().x == chel.character.GetCurrNode().x
                && MC.GetCurrNode().y == chel.character.GetCurrNode().y)
            {
                TM.ZoomToMino();
                break;
            }
        }*/
        GetComponent<AudioManager>().Stop();
        Sound();
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

    public void Sound()
    {
        PathFinder.POINT sizes = new PathFinder.POINT(MC.maze.baseTilesList.sizeX, MC.maze.baseTilesList.sizeY);
        int[,] maze = new int[sizes.x, sizes.y];
        bool[,] visited = new bool[sizes.x, sizes.y];

        for (int i = 0; i < MC.maze.baseTilesList.sizeX; i++)
        {
            for (int j = 0; j < MC.maze.baseTilesList.sizeY; j++)
            {
                if (MC.maze.Maze[i, j].isWall) maze[i, j] = -1;
                else maze[i, j] = 0;
            }
        }

        PathFinder.POINT startTarget = new PathFinder.POINT(MC.GetCurrNode().x, MC.GetCurrNode().y);
        
        PathFinder.FindPathBFS(startTarget.x, startTarget.y, ref visited, ref maze, sizes);
        int max = -1;
        for (int i = 0; i < sizes.x; i++)
        {
            for (int j = 0; j < sizes.y; j++)
            {
                if (maze[i, j] > max)
                    max = maze[i, j];
            }
        }

        List<PathFinder.POINT> maxs = new List<PathFinder.POINT>();
        for (int i = 0; i < sizes.x; i++)
        {
            for (int j = 0; j < sizes.y; j++)
            {
                if (maze[i, j] > max * 0.6f)
                    maxs.Add(new PathFinder.POINT(i, j));
            }
        }


        foreach (var chel in TM.cheliks)
        {
            if (Mathf.Abs(chel.character.GetCurrNode().x - MC.GetCurrNode().x) <= 2
                && Mathf.Abs(chel.character.GetCurrNode().y - MC.GetCurrNode().y) <= 2)
            {
                chel.character.SetPlayer(this);
            }
            else
            {
                chel.character.SetPlayer(null);
            }
            
        }
        for (int i = MC.GetCurrNode().x + 1; i < MC.maze.baseTilesList.sizeX; i++)
        {
           if(MC.maze.Maze[i, MC.GetCurrNode().y].isWall) break;
           else if (MC.maze.Maze[i, MC.GetCurrNode().y].character != null)
               MC.maze.Maze[i, MC.GetCurrNode().y].character.GetComponent<MazeCharacter>().SetPlayer(this);
        }
        for (int i = MC.GetCurrNode().x - 1; i >= 0; i--)
        {
           if(MC.maze.Maze[i, MC.GetCurrNode().y].isWall) break;
           else if (MC.maze.Maze[i, MC.GetCurrNode().y].character != null)
               MC.maze.Maze[i, MC.GetCurrNode().y].character.GetComponent<MazeCharacter>().SetPlayer(this);
        }
        for (int i = MC.GetCurrNode().y - 1; i >= 0; i--)
        {
           if(MC.maze.Maze[MC.GetCurrNode().x, i].isWall) break;
           else if (MC.maze.Maze[MC.GetCurrNode().x, i].character != null)
               MC.maze.Maze[MC.GetCurrNode().x, i].character.GetComponent<MazeCharacter>().SetPlayer(this);
        }
        for (int i = MC.GetCurrNode().y + 1; i < MC.maze.baseTilesList.sizeY; i++)
        {
           if(MC.maze.Maze[MC.GetCurrNode().x, i].isWall) break;
           else if (MC.maze.Maze[MC.GetCurrNode().x, i].character != null)
               MC.maze.Maze[MC.GetCurrNode().x, i].character.GetComponent<MazeCharacter>().SetPlayer(this);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!TM.IsPlayerTurn() || TM.GetPaused()) return;
        if (_isMoving) return;

        if (_MinotaurOD > 0)
        {
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
                _MinotaurOD--;
                MinotaurOD_UI.SetPoints(_MinotaurOD);
                MC.animator.Play("ActiveGear90");
            }
            if (Input.GetKeyDown(KeyCode.E) && HasNodeGear())
            {
                _isMoving = true;
                _MinotaurOD--;
                MinotaurOD_UI.SetPoints(_MinotaurOD);
                MC.animator.Play("ActiveGear_90");
            }
        }
    }
}