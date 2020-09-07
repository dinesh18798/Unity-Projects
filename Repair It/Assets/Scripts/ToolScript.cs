using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolScript : MonoBehaviour
{
    private BoxCollider2D collider;
    private GameController gameController;
    private Vector3 scaleForTool = new Vector3(0.05f, 0.05f, 0.05f);

    public int toolType = 0;

    public ToolModel toolModel;

    private SpriteRenderer renderer;
    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<BoxCollider2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.InitToolModel();

        //highlight
        renderer = GetComponent<SpriteRenderer>();
        defaultColor = renderer.color;

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {

        renderer.color = Color.yellow;
        Vector3 scaleChange  = scaleForTool; 
        gameObject.transform.localScale += scaleChange;
        this.toolModel.isSelected = true;
    }

    public void ClearSelected()
    {
        if (this.toolModel.isSelected)
        {
            renderer.color = defaultColor;
            this.toolModel.isSelected = false;
            Vector3 scaleChange = scaleForTool;
            gameObject.transform.localScale -= scaleChange;
        }
    }

    public void InitToolModel()
    {
        toolModel = new ToolModel();
        switch (toolType)
        {
            case 0:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_WRENCH;
                break;
            case 1:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.BRUSH;
                break;
            case 2:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_WASHING;
                break;
            case 3:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.TAPE;
                break;
            case 4:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.FLAMETORCH;
                break;
            case 5:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_HORIZONTAL;
                break;
            case 6:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_VERTICAL;
                break;
            case 7:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_TOPLEFT;
                break;
            case 8:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_TOPRIGHT;
                break;
            case 9:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_BOTTOMLEFT;
                break;
            case 10:
                this.toolModel.toolType = ToolModel.TOOL_TYPE.PIPE_BOTTOMRIGHT;
                break;
        }
    }


}
