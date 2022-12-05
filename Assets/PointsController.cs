using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public PointType pointType;
    public GameObject prefab;
    List<GameObject> states = new List<GameObject>();

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

    private int count = 0;
    private GameObject last = null;

    public void SetPoints(int i)
    {
        if (player == null) Start();
        if (i < count)
        {
            if (last != null)
            {
                last.GetComponent<Animator>().Play("Disappear");

                last = null;
                count--;
                if (count > 0) last = states[count - 1];
            }
        }
        else
        {
            if (transform.childCount == 0)
            {
                for (int j = 0; j < i; j++)
                {
                    var go = Instantiate(prefab, transform);
                    count++;
                    last = go;
                    states.Add(go);
                }
            }
            else
            {
                for (int j = 0; j < transform.childCount; j++)
                {
                    var anim = states[j].GetComponent<Animator>();
                    if (!anim.enabled)
                    {
                        anim.enabled = true;
                        anim.Play("Appear");
                        count++;
                        last = states[j];
                    }
                }
            }
        }
    }
}