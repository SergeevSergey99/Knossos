using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [HideInInspector] public MinotaurController player = null;
    [HideInInspector] public List<CHEL> cheliks = new List<CHEL>();

    public int chelicksOD = 3;

    //List<int> _chelicksOD = new List<int>();
    private bool _isPlayerTurn = true;
    public bool IsPlayerTurn() => _isPlayerTurn;
    public Button turnButton;
    public Animator LoseGame;
    public Animator WinGame;
    public Animator EscapeMenu;
    public Animator KillPanel;
    private bool isPaused = false;
    public bool GetPaused() => isPaused;

    private Vector2 cameraPos;
    private float camSize;

    public class CHEL
    {
        public MazeCharacter character = null;
        public int OD = 0;
    }

    private void Awake()
    {
        if (camSize == 0)
        {
            cameraPos = Camera.main.transform.position;
            camSize = Camera.main.orthographicSize;
            player = FindObjectOfType<MinotaurController>();
            cheliks = new List<CHEL>();
            var chs = FindObjectsOfType<MazeCharacter>();
            foreach (var mc in chs)
            {
                if (mc.GetComponent<MinotaurController>() == null)
                {
                    CHEL ch = new CHEL();
                    ch.character = mc;
                    ch.OD = chelicksOD;
                    cheliks.Add(ch);
                }
            }
        }
    }

    void UpdateCheliksOD()
    {
        foreach (var chel in cheliks)
        {
            chel.OD = chelicksOD;
        }
    }

    public void CheliksRandMove(MazeCharacter mc)
    {
        mc.MoveToRandomPossibleDir();
    }

    void CheckAndMove(MazeCharacter mc, MazeCharacter.direction dir)
    {
        if (mc.CanMove(dir)) mc.MoveTo(dir);
        else
        {
            mc.path.Clear();
            mc.path = null;
            mc.SetPlayer(null);
            CheliksRandMove(mc);
        }
    }

    public void CheliksAIMove(MazeCharacter mc)
    {
        if (mc.path != null)
        {
            mc.path.RemoveAt(0);
            if (mc.path.Count == 0)
            {
                mc.SetPlayer(null);
                CheliksRandMove(mc);
            }
            else
            {
                if (mc.path[0].x > mc.GetCurrNode().x) CheckAndMove(mc, MazeCharacter.direction.right);
                else if (mc.path[0].x < mc.GetCurrNode().x) CheckAndMove(mc, MazeCharacter.direction.left);
                else if (mc.path[0].y > mc.GetCurrNode().y) CheckAndMove(mc, MazeCharacter.direction.up);
                else if (mc.path[0].y < mc.GetCurrNode().y) CheckAndMove(mc, MazeCharacter.direction.down);
            }
        }
        else
        {
            mc.SetPlayer(null);
            CheliksRandMove(mc);
        }
    }

    public void CheliksMove()
    {
        for (int i = 0; i < cheliks.Count; i++)
        {
            if (cheliks[i].OD > 0)
            {
                if (cheliks[i].OD == chelicksOD) UpdateAI(cheliks[i].character);
                cheliks[i].OD--;
                if (cheliks[i].character.useAI) CheliksAIMove(cheliks[i].character);
                else CheliksRandMove(cheliks[i].character);
                break;
            }
        }

        StartCoroutine(WaitTillStop());
    }

    public void UpdateAI(MazeCharacter mc)
    {
        if (mc.useAI && (mc.path == null || mc.path.Count == 0))
        {
            PathFinder.POINT sizes = new PathFinder.POINT(mc.maze.baseTilesList.sizeX, mc.maze.baseTilesList.sizeY);
            int[,] arr = new int[sizes.x, sizes.y];
  //          int[,] arr2 = new int[sizes.x, sizes.y];
            bool[,] visited = new bool[sizes.x, sizes.y];
    //        bool[,] visited2 = new bool[sizes.x, sizes.y];

            for (int i = 0; i < mc.maze.baseTilesList.sizeX; i++)
            {
                for (int j = 0; j < mc.maze.baseTilesList.sizeY; j++)
                {
                    if (mc.maze.Maze[i, j].isWall) arr[i, j] = -1;
                    else arr[i, j] = 0;
                }
            }

            PathFinder.POINT startTarget =
                new PathFinder.POINT(
                    player.GetMC().GetCurrNode().x,
                    player.GetMC().GetCurrNode().y);

            PathFinder.FindPathBFS(startTarget.x, startTarget.y, ref visited, ref arr, sizes);
//            PathFinder.FindPathBFS(mc.GetCurrNode().x, mc.GetCurrNode().y, ref visited2, ref arr2, sizes, true);
            
            List<PathFinder.POINT> maxPoints = new List<PathFinder.POINT>();
            for (int i = 0; i < sizes.x; i++)
            {
                for (int j = 0; j < sizes.y; j++)
                {
                    if (arr[i, j] > arr[mc.GetCurrNode().x, mc.GetCurrNode().y])
                        maxPoints.Add(new PathFinder.POINT(i, j));
                }
            }

            //PathFinder.Shuffle(ref maxPoints);
            if (maxPoints.Count == 0) return;
            maxPoints.OrderBy(o => arr[o.x, o.y]);
            for (int i = (int) Mathf.Floor(maxPoints.Count / 2f); i < maxPoints.Count; i++)
            {
                
                mc.target = new PathFinder.POINT(maxPoints[i].x, maxPoints[i].y);
                mc.path = PathFinder.FindPathBFS(new PathFinder.POINT(mc.GetCurrNode().x, mc.GetCurrNode().y),
                    mc.target, ref arr,
                    sizes);
                if (arr[mc.GetCurrNode().x, mc.GetCurrNode().y] < arr[maxPoints[i].x, maxPoints[i].y]
                    && arr[maxPoints[i].x, maxPoints[i].y] - arr[mc.GetCurrNode().x, mc.GetCurrNode().y]
                    < arr[mc.target.x, mc.target.y] - arr[mc.GetCurrNode().x, mc.GetCurrNode().y])
                {
                    return;
                }
            }
            for (int i = (int) Mathf.Floor(maxPoints.Count / 2f); i > 0; i--)
            {
                
                mc.target = new PathFinder.POINT(maxPoints[i].x, maxPoints[i].y);
                mc.path = PathFinder.FindPathBFS(new PathFinder.POINT(mc.GetCurrNode().x, mc.GetCurrNode().y),
                    mc.target, ref arr,
                    sizes);
                if (arr[mc.GetCurrNode().x, mc.GetCurrNode().y] < arr[maxPoints[i].x, maxPoints[i].y]
                    && arr[maxPoints[i].x, maxPoints[i].y] - arr[mc.GetCurrNode().x, mc.GetCurrNode().y]
                    < arr[mc.target.x, mc.target.y] - arr[mc.GetCurrNode().x, mc.GetCurrNode().y])
                {
                    return;
                }
            }
        }
    }


    public void startCheliksTurn()
    {
        _isPlayerTurn = false;
        turnButton.interactable = false;
        cheliks = cheliks
            .OrderByDescending(o => Mathf.Abs((o.character.transform.position - player.transform.position).magnitude))
            .ToList();

        if (player.GetOG() > 0)
        {
            player.LoseOG();
            player.ShowOG();
            UpdateCheliksOD();

            CheliksMove();
        }
        else
        {
            LoseGame.gameObject.SetActive(true);
            LoseGame.Play("AppearHalf");
        }
    }

    bool IsCheliksStoped()
    {
        foreach (var chel in cheliks)
        {
            if (chel.character.transform.localPosition != (Vector3.zero))
                return false;
        }

        return true;
    }

    int SumOD()
    {
        int sum = 0;
        foreach (var ch in cheliks)
        {
            sum += ch.OD;
        }

        return sum;
    }

    public void ZoomToMino()
    {
        isPaused = true;
        StartCoroutine(ZoomingTo());
    }

    public void ZoomFromMino()
    {
        isPaused = true;
        StartCoroutine(ZoomingFrom());
    }

    IEnumerator ZoomingTo()
    {
        var cam = Camera.main;
        for (int i = 100; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y,
                cam.transform.position.z);
            cam.transform.position = playerPos +
                                     (new Vector3(cameraPos.x, cameraPos.y, cam.transform.position.z) - playerPos) *
                                     (i / 100f);
            cam.orthographicSize = 1 + (camSize - 1) * (i / 100f);
        }

        KillPanel.gameObject.SetActive(true);
        KillPanel.Play("Killing");
    }

    public void Eating()
    {
        player.UpdateOG();
        for (int i = 0; i < cheliks.Count; i++)
        {
            if (cheliks[i].character.GetCurrNode().x == player.GetMC().GetCurrNode().x
                && cheliks[i].character.GetCurrNode().y == player.GetMC().GetCurrNode().y)
            {
                Destroy(cheliks[i].character.gameObject);
                cheliks.RemoveAt(i);
                i--;
            }
        }

        if (cheliks.Count == 0)
        {
            _isPlayerTurn = false;
            WinGame.gameObject.SetActive(true);
            WinGame.Play("AppearHalf");
        }
    }

    IEnumerator ZoomingFrom()
    {
        KillPanel.gameObject.SetActive(false);
        var cam = Camera.main;
        for (int i = 0; i <= 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y,
                cam.transform.position.z);
            cam.transform.position = playerPos +
                                     (new Vector3(cameraPos.x, cameraPos.y, cam.transform.position.z) - playerPos) *
                                     (i / 100f);
            cam.orthographicSize = 1 + (camSize - 1) * (i / 100f);
        }

        isPaused = false;
    }

    IEnumerator WaitTillStop()
    {
        while (!IsCheliksStoped())
        {
            yield return new WaitForSeconds(0.05f);
        }

        if (SumOD() > 0) CheliksMove();
        else
        {
            _isPlayerTurn = true;
            turnButton.interactable = true;
            player.UpdateOD();
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            EscapeMenu.gameObject.SetActive(true);
            EscapeMenu.Play("AppearHalf");
        }
        else EscapeMenu.Play("DisappearHalf");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Pause();
    }
}