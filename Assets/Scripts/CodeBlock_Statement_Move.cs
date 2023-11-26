using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CodeBlock_Statement_Move : CodeBlock_Statement
{
    public Vector2 Direction { get; set; }

    private void Start()
    {
        Direction = Vector2.up;
    }

    public void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                Direction = Vector2.up;
                break;
            case 1:
                Direction = Vector2.down;
                break;
            case 2:
                Direction = Vector2.left;
                break;
            case 3:
                Direction = Vector2.right;
                break;
        }
    }
}
