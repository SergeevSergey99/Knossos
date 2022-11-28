using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool useAI = false;
    private Vector2 target= Vector2.zero;
    private MinotaurController player;
    public Node GetCurrNode() => _currNode;

    [SerializeField]
    private SpriteRenderer sign;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        maze = FindObjectOfType<MAZE>();
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

    public void SetPlayer(MinotaurController mino)
    {
        player = mino;
        if (player != null)
        {
            sign.gameObject.SetActive(true);
        }
        else
        {
            sign.gameObject.SetActive(false);
        }
    }
    public List<direction> GetPossible(bool countCheliks = true)
    {
        List<direction> directions = new List<direction>();

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
        var dir = GetRandomPossible();
        MoveTo(dir);
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
        if (dir == direction.none) return;
        _currNode.RemoveCharacter();

        switch (dir)
        {
            case direction.left:
                _currNode = maze.Maze[_currNode.x - 1, _currNode.y];
                if (spriteRenderer != null) spriteRenderer.flipX = false;
                if (animator != null) animator.Play("FlipXFalse");
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.right:
                _currNode = maze.Maze[_currNode.x + 1, _currNode.y];
                if (spriteRenderer != null) spriteRenderer.flipX = true;
                if (animator != null) animator.Play("FlipXTrue");
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.up:
                _currNode = maze.Maze[_currNode.x, _currNode.y + 1];
                if (animator != null) animator.SetBool("isWalk", true);
                break;
            case direction.down:
                _currNode = maze.Maze[_currNode.x, _currNode.y - 1];
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