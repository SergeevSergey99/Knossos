using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [HideInInspector]
    public MinotaurController player = null;
    [HideInInspector]
    public List<CHEL> cheliks = new List<CHEL>();
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

    public void CheliksRandMove()
    {
        
        for (int i = 0; i < cheliks.Count; i++)
        {
            if (cheliks[i].OD > 0)
            {
                cheliks[i].OD--;
                cheliks[i].character.MoveToRandomPossibleDir();
                break;
            }
        }

        StartCoroutine(WaitTillStop());
    }

    public void startCheliksTurn()
    {
        _isPlayerTurn = false;
        turnButton.interactable = false;
        player.Sound();
        cheliks = cheliks.OrderByDescending(o=>Mathf.Abs((o.character.transform.position - player.transform.position).magnitude)).ToList();
        if (player.GetOG() > 0)
        {
            player.LoseOG();
            player.ShowOG();
            UpdateCheliksOD();

            CheliksRandMove();
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

        if (SumOD() > 0) CheliksRandMove();
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