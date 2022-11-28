using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public PointType pointType;
    public GameObject prefab;
    List<GameObject> states;

    private MinotaurController player;
    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<MinotaurController>();

            foreach (Transform VAR in transform)
            {
                Destroy(VAR.gameObject);
            }
            if (pointType == PointType.OD) SetPoints(player.MinotaurOD);
            else SetPoints(player.HungerOG);
            
        }
    }

    public enum PointType
    {
        OD,
        OG
    }

    private GameObject last = null;
    public void SetPoints(int i)
    {
       if(player == null) Start();
       if (i < transform.childCount)
       {
           last.GetComponent<Animator>().Play("Disappear");

           last = null;
           if(transform.childCount > 1) last = transform.GetChild(transform.childCount - 2).gameObject;
       }
       else
       {
           foreach (Transform VAR in transform)
           {
               Destroy(VAR.gameObject);
           }

           for (int j = 0; j < i; j++)
           {
               var go = Instantiate(prefab, transform);
               last = go;
           }
       }
       /*
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
