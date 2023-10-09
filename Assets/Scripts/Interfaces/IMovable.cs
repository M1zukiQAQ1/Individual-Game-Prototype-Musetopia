using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    // Start is called before the first frame update
    void MoveObject(Vector3 pos);

    float moveSpeed { get; set; }

}
