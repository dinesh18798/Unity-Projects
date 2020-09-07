using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TubeModel 
{
    // Start is called before the first frame update

    public enum PIPE_STATUS
    {
        NORMAL = 0, BROKEN = 1, LOOSEN = 2, DIRTY = 3, LEAKAGE = 4
    }

    public enum PIPE_TYPE
    {
        HORIZONTAL = 0, VERTICAL = 1 , BOTTOM_LEFT  = 2, BOTTOM_RIGHT = 3 , TOP_LEFT = 4 , TOP_RIGHT = 5
    }

    public enum PIPE_FACE
    {
        TOP = 0 , DOWN = 1 , LEFT = 2, RIGHT = 3
    }

    public PIPE_STATUS pipeStatus { get; set; }
    public PIPE_TYPE pipeType { get; set; }

    //index 0 = top , 1 = down , 2 = left , 3 = right
    public int [] connectableDirection = new int[] { 0 , 0 , 0 , 0};
    public bool isSelected { get; set; }


    public TubeModel() {

    }


    public void Rotate()
    {
        int[] temp = connectableDirection;
        connectableDirection[0] = temp[2];
        connectableDirection[1] = temp[3];
        connectableDirection[2] = temp[1];
        connectableDirection[3] = temp[0];
    }



}
