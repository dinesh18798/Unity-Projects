using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolModel 
{
    public enum TOOL_TYPE
    {
        PIPE_WRENCH = 0,
        BRUSH = 1,
        PIPE_WASHING = 2,
        TAPE = 3,
        FLAMETORCH = 4,
        PIPE_HORIZONTAL = 5,
        PIPE_VERTICAL= 6,
        PIPE_TOPLEFT = 7,
        PIPE_TOPRIGHT = 8,
        PIPE_BOTTOMLEFT = 9,
        PIPE_BOTTOMRIGHT = 10

    }

    public TOOL_TYPE toolType { get; set; }

    public bool isSelected { get; set; }

    public ToolModel()
    {
    }



}
