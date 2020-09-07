using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    internal TubeScript SelectedTube;
    public List<ToolScript> SelectedTools;
    public int Level;

    private AudioSource audioSource;
    private SceneChanger sceneChanger;

    internal bool showResult = false;
    internal bool isLevelCompleted = false;
    private AudioSource soundEffect;

    internal float timeLimit;

    // Start is called before the first frame update
    void Start()
    {
        ApplicationUtil.CurrentLevel = Level;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = ApplicationUtil.GameSoundVolume;

        GameObject sceneChangerObject = GameObject.FindWithTag("SceneChanger");
        if (sceneChangerObject != null)
        {
            sceneChanger = sceneChangerObject.GetComponent<SceneChanger>();
        }

        GameObject soundeffectGameObject = GameObject.FindWithTag("SoundEffect");
        if (soundeffectGameObject != null)
        {
            soundEffect = soundeffectGameObject.GetComponent<AudioSource>();
        }

        showResult = false;
        isLevelCompleted = false;
        soundEffect.volume = ApplicationUtil.GameMusicVolume;
    }


    // Update is called once per frame
    void Update()
    {
        IsTimerSideBarFull();
        if (Input.GetMouseButtonDown(0))
        {
            soundEffect.Play();
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                #region Is pipe selected
                TubeScript pipe = hit.collider.gameObject.GetComponent<TubeScript>();
                if (pipe != null)
                {
                    if (SelectedTube == pipe)
                    {
                        //deselect the selected obj
                        SelectedTube.ClearSelected();
                        SelectedTube = null;
                    }
                    else
                    {
                        //player selects new pipe 
                        if (SelectedTube != null)
                        {
                            //clear previous pipe
                            SelectedTube.ClearSelected();
                            SelectedTube = null;
                        }

                        pipe.OnClick();
                    }
                }
                #endregion
                #region Is tools selected
                ToolScript tool = hit.collider.gameObject.GetComponent<ToolScript>();
                if (tool != null)
                {
                    bool isInSelectedToolsList = false;
                    foreach (var objTool in SelectedTools)
                    {
                        Debug.Log(objTool);
                        if (tool == objTool)
                        {
                            //check is it in list
                            isInSelectedToolsList = true;
                        }
                    }
                    if (isInSelectedToolsList)
                    {
                        //deselect it
                        SelectedTools.Remove(tool);
                        tool.ClearSelected();
                    }
                    else
                    {
                        //select it
                        SelectedTools.Add(tool);
                        tool.OnClick();
                    }

                }

                #endregion
                #region Check Rule
                #endregion

                TryFixedPipe();
            }

        }

    }

    internal void LoadNextLevel()
    {
        sceneChanger.FadeToNextLevel();
    }

    internal void VolumeUpdate()
    {
        if (audioSource != null)
            audioSource.volume = ApplicationUtil.GameSoundVolume;
        if (soundEffect != null)
            soundEffect.volume = ApplicationUtil.GameMusicVolume;
    }

    internal void LoadMainMenu()
    {
        sceneChanger.FadeToMainMenu();
    }

    internal void ReloadScene()
    {
        sceneChanger.ReloadScene();
    }

    public void ClearSelectedTools()
    {
        foreach (var tool in SelectedTools)
        {
            tool.ClearSelected();
        }
        SelectedTools.Clear();
    }

    public void TryFixedPipe()
    {
        if (this.SelectedTube != null)
        {
            bool result = this.SelectedTube.TryFixed(SelectedTools);

        }
    }


    public bool IsAllTubesFixed()
    {
        var tubes = GameObject.FindGameObjectsWithTag("Pipe");
        Debug.Log("Total pipes in a game: " + tubes.Length);

        foreach (var tube in tubes)
        {
            var script = tube.GetComponent<TubeScript>();
            if (script != null)
            {
                if (script.tubeModel.pipeStatus != TubeModel.PIPE_STATUS.NORMAL && script.IsActive)
                {
                    return false;
                }
            }
        }

        return true;
    }


    public void IsTimerSideBarFull()
    {
        if (timeLimit >= 100f) Result();
    }


    public void OnClickedRepairButton()
    {
        Result();
    }

    private void Result()
    {

        if (IsAllTubesFixed())
        {
            Debug.Log("Win");
            isLevelCompleted = true;
        }
        else
        {
            Debug.Log("Lose");
            isLevelCompleted = false;
        }

        showResult = true;
    }

}
