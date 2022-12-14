using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MazeCharacter : MonoBehaviour
{
    [HideInInspector] public MAZE maze;

    private Node _currNode;

    [SerializeField] private SpriteRenderer spriteRenderer;
    public Animator animator;
    [SerializeField] private int FramesPerMove = 50;

    public bool useAI = false;
    [HideInInspector]
    public PathFinder.POINT target = null;
    public PathFinder.POINT playerPOS = null;
    [HideInInspector] public List<PathFinder.POINT> path = null;
    private MinotaurController player;
    public bool isPlayerNull() => player == null;
    public Node GetCurrNode() => _currNode;

    [SerializeField] private SpriteRenderer sign;

    private bool isPlayer = false;
    public direction lastDirection = direction.none;
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        maze = FindObjectOfType<MAZE>();
        isPlayer = GetComponent<MinotaurController>() != null;
    }

    void Start()
    {
        int closestX = 0, closestY = 0;
        float min = float.MaxValue;
        for (int i = 0; i < maze.baseTilesList.sizeX; i++)
        {
            for (int j = 0; j < maze.baseTilesList.sizeY; j++)
            {
                if ((maze.Maze[i, j].transform.position - transform.position).magnitude < min)
                {
                    min = (maze.Maze[i, j].transform.position - transform.position).magnitude;
                    closestX = i;
                    closestY = j;
                }
            }
        }

        _currNode = maze.Maze[closestX, closestY];
        transform.SetParent(_currNode.transform);
        _currNode.SetCharacter(transform);
        transform.localPosition = Vector3.zero;
    }

    public enum direction
    {
        left,
        right,
        up,
        down,
        none
    }

    public bool HaveCharacters(Node node)
    {
        if (node.hasCharacter)
        {
            return true;
        }

        return false;
    }

    public bool CanMove(direction dir)
    {
        switch (dir)
        {
            case direction.left:
                if (!isPlayer && maze.Maze[_currNode.x - 1, _currNode.y].character != null
                    || maze.Maze[_currNode.x - 1, _currNode.y].isWall) return false;
                break;
            case direction.right:
                if (!isPlayer && maze.Maze[_currNode.x + 1, _currNode.y].character != null
                    || maze.Maze[_currNode.x + 1, _currNode.y].isWall) return false;
                break;
            case direction.up:
                if (!isPlayer && maze.Maze[_currNode.x, _currNode.y + 1].character != null
                    || maze.Maze[_currNode.x, _currNode.y + 1].isWall) return false;
                break;
            case direction.down:
                if (!isPlayer && maze.Maze[_currNode.x, _currNode.y - 1].character != null
                    || maze.Maze[_currNode.x, _currNode.y - 1].isWall) return false;
                break;
        }

        return true;
    }

    public bool HanCheliks(direction dir)
    {
        switch (dir)
        {
            case direction.left:
                if (maze.Maze[_currNode.x - 1, _currNode.y].character != null) return true;
                break;
            case direction.right:
                if (maze.Maze[_currNode.x + 1, _currNode.y].character != null) return true;
                break;
            case direction.up:
                if (maze.Maze[_currNode.x, _currNode.y + 1].character != null) return true;
                break;
            case direction.down:
                if (maze.Maze[_currNode.x, _currNode.y - 1].character != null) return true;
                break;
        }

        return false;
    }

    public void SetPlayer(MinotaurController mino)
    {
        player = mino;
        if (player != null)
        {
            useAI = true;
            sign.gameObject.SetActive(true);
            playerPOS = new PathFinder.POINT(player.GetMC().GetCurrNode().x, player.GetMC().GetCurrNode().y);

            path = null;
        }
        else
        {
            if (path == null) useAI = false;
            target = null;
            path = null;
            sign.gameObject.SetActive(false);
        }
    }

    public List<direction> GetPossible(bool countCheliks = true)
    {
        List<direction> directions = new List<direction>();
        directions.Add(direction.none);

        if (_currNode.x > 0 && !maze.Maze[_currNode.x - 1, _currNode.y].isWall
                            && (!countCheliks || !HaveCharacters(maze.Maze[_currNode.x - 1, _currNode.y])))
            directions.Add(direction.left);

        if (_currNode.x < maze.baseTilesList.sizeX - 1 && !maze.Maze[_currNode.x + 1, _currNode.y].isWall
                                                       && (!countCheliks ||
                                                           !HaveCharacters(maze.Maze[_currNode.x + 1, _currNode.y])))
            directions.Add(direction.right);

        if (_currNode.y > 0 && !maze.Maze[_currNode.x, _currNode.y - 1].isWall
                            && (!countCheliks || !HaveCharacters(maze.Maze[_currNode.x, _currNode.y - 1])))
            directions.Add(direction.down);

        if (_currNode.y < maze.baseTilesList.sizeY - 1 && !maze.Maze[_currNode.x, _currNode.y + 1].isWall
                                                       && (!countCheliks ||
                                                           !HaveCharacters(maze.Maze[_currNode.x, _currNode.y + 1])))
            directions.Add(direction.up);
        
        if (directions.Count > 1 && directions.Contains(direction.none)) directions.Remove(direction.none);
        return directions;
    }

    public direction GetRandomPossible()
    {
        var directions = GetPossible();
        if (directions.Count == 0) return direction.none;
        return directions[Random.Range(0, directions.Count)];
    }

    public void MoveToRandomPossibleDir()
    {
        var directions = GetPossible();
        if (directions.Contains(lastDirection))
        {
            MoveTo(lastDirection);
            
        }
        else
        {
            if (directions.Count > 1)
            {
                if (directions.Contains(getOppositeDirection(lastDirection)))
                {
                    directions.Remove(getOppositeDirection(lastDirection));
                }
                
                MoveTo(directions[Random.Range(0, directions.Count)]);
                
            }
            else
            {
                MoveTo(directions[0]);
                
            }
        }
    }

    public direction getOppositeDirection(direction direction)
    {
            switch (direction)
        {
            case direction.left:
                return direction.right;
            case direction.right:
                return direction.left;
            case direction.up:
                return direction.down;
            case direction.down:
                return direction.up;
            default:
                return direction.none;
        }

    }

    public void SetFlipX(int flip)
    {
        if (spriteRenderer != null) spriteRenderer.flipX = flip == 1;
        if (animator != null)
        {
            if (flip == 1) animator.Play("FlipXTrue");
            else animator.Play("FlipXFalse");
        }
    }

    public void MoveTo(direction dir)
    {
        lastDirection = dir;
        if (dir == direction.none) return;

        _currNode.RemoveCharacter();
        switch (dir)
        {
            case direction.left:
                _currNode = maze.Maze[_currNode.x - 1, _currNode.y];
                lastDirection = direction.left;
                if (spriteRenderer != null) spriteRenderer.flipX = false;
                if (animator != null) animator.Play("FlipXFalse");
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.right:
                _currNode = maze.Maze[_currNode.x + 1, _currNode.y];
                lastDirection = direction.right;
                if (spriteRenderer != null) spriteRenderer.flipX = true;
                if (animator != null) animator.Play("FlipXTrue");
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.up:
                _currNode = maze.Maze[_currNode.x, _currNode.y + 1];
                lastDirection = direction.up;
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.down:
                _currNode = maze.Maze[_currNode.x, _currNode.y - 1];
                lastDirection = direction.down;
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            default:
                return;
        }

        transform.SetParent(_currNode.transform);
        _currNode.SetCharacter(transform);
        StartCoroutine(MoveToZero());
    }

    IEnumerator MoveToZero()
    {
        float t = 0.01f;
        int i = FramesPerMove;
        var start = transform.localPosition;
        while (i > 0)
        {
            yield return new WaitForSeconds(t);
            transform.localPosition = start * (i * 1f / FramesPerMove);
            i--;
        }

        transform.localPosition = Vector3.zero;

        if (animator != null) animator.SetBool("isWalk", false);
    }
}