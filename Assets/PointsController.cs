using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public PointType pointType;
    public GameObject prefab;
    List<GameObject> states;

    private MinotaurController player;
    private Image img;
    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<MinotaurController>();
            img = GetComponent<Image>();
        }
    }

    public enum PointType
    {
        OD,
        OG
    }
    public void SetPoints(int i)
    {
       /* if(player == null) Start();
        if (pointType == PointType.OD)
        {
            var val = i * 1f / player.MinotaurOD * (states.Count - 1);
            img.sprite = states[(int) Mathf.Round(val)];
        }
        else
        {
            var val = i * 1f / player.HungerOG * (states.Count - 1);
            img.sprite = states[(int) Mathf.Round(val)];
        }*/
    }
}
