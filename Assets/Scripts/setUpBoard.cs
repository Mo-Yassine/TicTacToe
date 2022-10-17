using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setUpBoard : MonoBehaviour
{
    int k = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject go = transform.GetChild(k).gameObject;
                go.GetComponent<FieldManager>().setRowColumn(i, j);
                if (k < transform.childCount)
                {
                    k++;
                }
            }
        }
    }
}
