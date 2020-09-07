using System;
using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;
using static ToolModel;

public class TubeScript : MonoBehaviour
{
    public TubeModel tubeModel;
    private BoxCollider2D collider;
    private GameController gameController;
    private SpriteRenderer m_SpriteRenderer;
    public Sprite FixedPipe;

    //params used for initial state only cannot tell accessible after rotate model;
    public bool IsTopAccessible = false;
    public bool IsDownAccessible = false;
    public bool IsLeftAccessible = false;
    public bool IsRightAccessible = false;

    //0 = normal 1 = broken 2 = loosen 3 = dirty 4 = leakage
    public int PipeStatus = 0;
    public int PipeType = 0;


    public bool IsActive { get; set; } = true;

    private SpriteRenderer renderer;
    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        
        collider = this.GetComponent<BoxCollider2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
        InitPipeModel();

        //highlight
        renderer = GetComponent<SpriteRenderer>();
        defaultColor = renderer.color;

    }

    // Update is called once per frame
    void Update()
    {
       OnRotate();
    }

    public void InitPipeModel()
    {
        tubeModel = new TubeModel();
        switch (PipeStatus)
        {
            case 0:
         
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.NORMAL;
                break;
            case 1:
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.BROKEN;
                break;
            case 2:
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.LOOSEN;
                break;
            case 3:
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.DIRTY;
                break;
            case 4:
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.LEAKAGE;
                break;
            default:
                tubeModel.pipeStatus = TubeModel.PIPE_STATUS.NORMAL;
                break;
        }

        switch (PipeType)
        {
            case 0:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.HORIZONTAL;
                break;
            case 1:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.VERTICAL;
                break;
            case 2:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.BOTTOM_LEFT;
                break;
            case 3:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.BOTTOM_RIGHT;
                break;
            case 4:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.TOP_LEFT; 
                break;
            case 5:
                tubeModel.pipeType = TubeModel.PIPE_TYPE.TOP_RIGHT;
                break;

        }

        int[] faces = new int[4];
        if (IsTopAccessible)
        {
            faces[0] = 1;
        }
        if (IsDownAccessible)
        {
            faces[1] = 1;
        }
        if (IsLeftAccessible)
        {
            faces[2] = 1;
        }
        if (IsRightAccessible)
        {
            faces[3] = 1;
        }
    }

    public void OnRotate()
    {
        if (this.tubeModel.isSelected)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameObject.transform.Rotate(0,0,90);
                this.tubeModel.Rotate();
            }
        }
    }

    public void ClearSelected()
    {
        if (this.tubeModel.isSelected)
        {
            renderer.color = defaultColor;
            this.tubeModel.isSelected = false;
            Vector3 scaleChange = new Vector3(0.25f, 0.25f, 0.25f);
            gameObject.transform.localScale -= scaleChange;
        }
    }

    public void OnClick()
    {
        renderer.color = Color.yellow;

        Vector3 scaleChange = new Vector3(0.25f, 0.25f, 0.25f);
            gameObject.transform.localScale += scaleChange;
            this.tubeModel.isSelected = true;
            gameController.SelectedTube = this;
    }

    public bool TryFixed(List<ToolScript> tools)
    {
        bool isValidate = false;
        if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            //do nothing
        }
        #region BROKEN PIPE
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.HORIZONTAL)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_HORIZONTAL)
                {
                    params2 = true;
                }
                else
                {
                    params2 = false;
                    params1 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.VERTICAL)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_VERTICAL)
                {
                    params2 = true;
                }
                else
                {
                    params1 = false;
                    params2 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.TOP_LEFT)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_TOPLEFT)
                {
                    params2 = true;
                }
                else
                {
                    params1 = false;
                    params2 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.TOP_RIGHT)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_TOPRIGHT)
                {
                    params2 = true;
                }
                else
                {
                    params1 = false;
                    params2 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.BOTTOM_RIGHT)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_BOTTOMRIGHT)
                {
                    params2 = true;
                }
                else
                {
                    params1 = false;
                    params2 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.BROKEN && this.tubeModel.pipeType == TubeModel.PIPE_TYPE.BOTTOM_LEFT)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.PIPE_BOTTOMLEFT)
                {
                    params2 = true;
                }
                else
                {

                    params1 = false;
                    params2 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        #endregion
        #region LOOSEN PIPE
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.LOOSEN)
        {
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WRENCH)
                {
                    isValidate = true;
                }
                else
                {
                    isValidate = false;
                    break;
                }
            }

        }
        #endregion
        #region DIRTY PIPE
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.DIRTY)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.PIPE_WASHING)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.BRUSH)
                {
                    params2 = true;
                }
                else
                {
                    params2 = false;
                    params1 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        #endregion
        #region LEAKAGE PIPE
        else if (this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.LEAKAGE)
        {
            bool params1 = false;
            bool params2 = false;
            //check all tool validated
            foreach (var tool in tools)
            {
                if (tool.toolModel.toolType == TOOL_TYPE.FLAMETORCH)
                {
                    params1 = true;
                }
                else if (tool.toolModel.toolType == TOOL_TYPE.TAPE)
                {
                    params2 = true;
                }
                else
                {
                    params2 = false;
                    params1 = false;
                    break;
                }
            }

            if (params2 && params1)
            {
                isValidate = true;
            }
        }
        #endregion
        //fix it
        if (isValidate)
        {

            this.tubeModel.pipeStatus = TubeModel.PIPE_STATUS.NORMAL;
            m_SpriteRenderer.sprite = GetSprite();
            this.ClearSelected();
            //clear selected tool
            gameController.ClearSelectedTools();
            return true;


        }

        return false;
    }

    public Sprite GetSprite()
    {
        //Debug.Log(this.tubeModel.pipeType + " " + this.tubeModel.pipeStatus);
        SpriteManager mgr = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManager>();
        #region Default Pipe
        if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.HORIZONTAL &&
            this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultHorizontalSprite;
        }
        else if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.VERTICAL &&
                 this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultVerticalSprite;
        }
        else if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.BOTTOM_LEFT &&
                 this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultBottomLeftSprite;
        }
        else if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.BOTTOM_RIGHT &&
                 this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultBottomRightSprite;
        }
        else if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.TOP_LEFT &&
                 this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultTopLeftSprite;
        }
        else if (this.tubeModel.pipeType == TubeModel.PIPE_TYPE.TOP_RIGHT &&
                 this.tubeModel.pipeStatus == TubeModel.PIPE_STATUS.NORMAL)
        {
            return mgr.DefaultTopRightSprite;
        }
        #endregion
        return null;
    }





}







