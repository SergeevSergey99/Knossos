using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MazeCharacter : MonoBehaviour
{
    private MAZE _maze;

    private Node _currNode;
    
    [SerializeField]private SpriteRenderer spriteRenderer;
    
    public Node GetCurrNode() => _currNode;
    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _maze = FindObjectOfType<MAZE>();
        int closestX = 0, closestY = 0;
        float min = float.MaxValue;
        for (int i = 0; i < _maze.baseTilesList.sizeX; i++)
        {
            for (int j = 0; j < _maze.baseTilesList.sizeY; j++)
            {
                if ((_maze.Maze[i, j].transform.position - transform.position).magnitude < min)
                {
                    min = (_maze.Maze[i, j].transform.position - transform.position).magnitude;
                    closestX = i;
                    closestY = j;
                }
            }
        }

        _currNode = _maze.Maze[closestX, closestY];
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

    public List<direction> GetPossible(bool countCheliks = true)
    {
        List<direction> directions = new List<direction>();
        if (_currNode.x > 0 && !_maze.Maze[_currNode.x - 1, _currNode.y].isWall
                           && (!countCheliks || !HaveCharacters(_maze.Maze[_currNode.x - 1, _currNode.y]))) 
            directions.Add(direction.left);
        
        if (_currNode.x < _maze.baseTilesList.sizeX - 1 && !_maze.Maze[_currNode.x + 1, _currNode.y].isWall
                                                      && (!countCheliks || !HaveCharacters(_maze.Maze[_currNode.x + 1, _currNode.y])))
            directions.Add(direction.right);
        
        if (_currNode.y > 0 && !_maze.Maze[_currNode.x, _currNode.y - 1].isWall
                           && (!countCheliks || !HaveCharacters(_maze.Maze[_currNode.x, _currNode.y - 1]))) 
            directions.Add(direction.down);
        
        if (_currNode.y < _maze.baseTilesList.sizeY - 1 && !_maze.Maze[_currNode.x, _currNode.y + 1].isWall
                                                      && (!countCheliks || !HaveCharacters(_maze.Maze[_currNode.x, _currNode.y + 1])))
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
    public void MoveTo(direction dir)
    {
        switch (dir)
        {
            case direction.left:
                _currNode = _maze.Maze[_currNode.x - 1, _currNode.y];
                spriteRenderer.flipX = false;
                break;
            case direction.right:
                _currNode = _maze.Maze[_currNode.x + 1, _currNode.y];
                spriteRenderer.flipX = true;
                break;
            case direction.up:
                _currNode = _maze.Maze[_currNode.x, _currNode.y + 1];
                break;
            case direction.down:
                _currNode = _maze.Maze[_currNode.x, _currNode.y - 1];
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
        int cnt = 50;
        int i = cnt;
        var start = transform.localPosition;
        while (i > 0)
        {
            yield return new WaitForSeconds(t);
            transform.localPosition = start *(i*1f/cnt);
            i--;
        }

        transform.localPosition = Vector3.zero;
    }
}