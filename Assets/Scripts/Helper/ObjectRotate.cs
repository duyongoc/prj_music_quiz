using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotate : MonoBehaviour
{

    #region FIELDS
    public Vector3 dir;
    public float moveSpeed = 5f;
    #endregion

    #region UNITY
    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(dir.x, dir.y, dir.z);
    }
    #endregion



}
