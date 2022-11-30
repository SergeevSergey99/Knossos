using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class MazeCharacter : MonoBehaviour
{
    //Graphics
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer sign;
    public Animator animator;

    [FormerlySerializedAs("FramesPerMove")] [SerializeField]
    private int framesPerMove = 50;
    // Graphics

    //Nodes
    [HideInInspector] public MAZE maze;
    private Node _currNode;
    public Node GetCurrNode() => _currNode;

    public MazeCharacter.direction lastDirection = MazeCharacter.direction.none;
    //Nodes

    //AI Stuff
    public bool useAI = false;
    [HideInInspector] public PathFinder.POINT target = null;
    public PathFinder.POINT playerPOS = null;

    [HideInInspector] public List<PathFinder.POINT> path = null;
    //AI Stuff

    //Player movement stuff
    private MinotaurController player;
    public bool isPlayerNull() => player == null;

    private bool isPlayer = false;
    //Player movement stuff

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        maze = FindObjectOfType<MAZE>();
        isPlayer = GetComponent<MinotaurController>() != null;
    }

    void Start()
    {
        _currNode = maze.FindCLoosestNodeToPosition(transform.position);
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
        var nodeInDireection = maze.GetNeighbourNodeByDirection(_currNode, dir);
        return !(nodeInDireection.isWall || HaveCharacters(nodeInDireection));
    }

    public bool HasCheliks(direction dir)
    {
        var nodeInDirection = maze.GetNeighbourNodeByDirection(_currNode, dir);
        return HaveCharacters(nodeInDirection);
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

        if (_currNode.x > 0 && !maze.GetNeighbourNodeByDirection(_currNode, direction.left).isWall)
            if (!countCheliks || !HaveCharacters(maze.GetNeighbourNodeByDirection(_currNode, direction.left)))
                directions.Add(direction.left);
        if (_currNode.x < maze.baseTilesList.sizeX - 1 &&
            !maze.GetNeighbourNodeByDirection(_currNode, direction.right).isWall)
            if (!countCheliks || !HaveCharacters(maze.GetNeighbourNodeByDirection(_currNode, direction.right)))
                directions.Add(direction.right);
        if (_currNode.y > 0 && !maze.GetNeighbourNodeByDirection(_currNode, direction.down).isWall)
            if (!countCheliks || !HaveCharacters(maze.GetNeighbourNodeByDirection(_currNode, direction.down)))
                directions.Add(direction.down);
        if (_currNode.y < maze.baseTilesList.sizeY - 1 &&
            !maze.GetNeighbourNodeByDirection(_currNode, direction.up).isWall)
            if (!countCheliks || !HaveCharacters(maze.GetNeighbourNodeByDirection(_currNode, direction.up)))
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
        spriteRenderer.flipX = flip == 1;
        //Uncomment when u have animation
        //animator.Play(flip == 1 ? "FlipXTrue" : "FlipXFalse");
    }

    public void MoveTo(direction dir)
    {
        lastDirection = dir;
        if (dir == direction.none) return;
        _currNode.RemoveCharacter();
        switch (dir)
        {
            case direction.left:
                _currNode = maze.GetNeighbourNodeByDirection(_currNode, direction.left);
                SetFlipX(0);
                break;
            case direction.right:
                _currNode = maze.GetNeighbourNodeByDirection(_currNode, direction.right);
                SetFlipX(1);
                break;
            case direction.up:
                _currNode = maze.GetNeighbourNodeByDirection(_currNode, direction.up);
                break;
            case direction.down:
                _currNode = maze.GetNeighbourNodeByDirection(_currNode, direction.down);
                break;
            default:
                return;
        }

        animator.SetBool("isWalk", true);
        transform.SetParent(_currNode.transform);
        _currNode.SetCharacter(transform);
        StartCoroutine(MoveToZero());
    }

    IEnumerator MoveToZero()
    {
        float t = 0.01f;
        int i = framesPerMove;
        var start = transform.localPosition;
        while (i > 0)
        {
            yield return new WaitForSeconds(t);
            transform.localPosition = start * (i * 1f / framesPerMove);
            i--;
        }

        transform.localPosition = Vector3.zero;

        if (animator != null) animator.SetBool("isWalk", false);
    }

    public direction GetOppositeDirection(direction dir)
    {
        switch (dir)
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
}