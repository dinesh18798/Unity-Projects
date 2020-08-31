﻿using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class ConversationManager : MonoBehaviour
    {
        private const float TRANSITION_TIME = 0.2f; // Transition time for fades

        public static ConversationManager Instance { get; private set; }

        public delegate void ConversationStartEvent();
        public delegate void ConversationEndEvent();

        public static ConversationStartEvent OnConversationStarted;
        public static ConversationEndEvent OnConversationEnded;

        private enum eState
        {
            TransitioningDialogueBoxOn,
            ScrollingText,
            TransitioningOptionsOn,
            Idle,
            TransitioningOptionsOff,
            TransitioningDialogueOff,
            Off,
            NONE,
        }

        // User-Facing options
        // Drawn by custom inspector
        public bool ScrollText;
        public float ScrollSpeed = 1;
        public Sprite BackgroundImage;
        public bool BackgroundImageSliced;
        public Sprite OptionImage;
        public bool OptionImageSliced;

        // Non-User facing 
        // Not exposed via custom inspector
        //
        // Base panels
        public RectTransform DialoguePanel;
        public RectTransform OptionsPanel;
        // Dialogue UI
        public Image DialogueBackground;
        public Image NpcIcon;
        public TMPro.TextMeshProUGUI NameText;
        public TMPro.TextMeshProUGUI DialogueText;
        // Components
        public AudioSource AudioPlayer;
        // Prefabs
        public UIConversationButton ButtonPrefab;
        // Default values
        public Sprite BlankSprite;


        // Private
        private float m_elapsedScrollTime;
        private int m_scrollIndex;
        public int m_targetScrollTextCount;
        private eState m_state;
        private float m_stateTime;
        private Conversation m_conversation;
        private List<UIConversationButton> m_uiOptions;

        private SpeechNode m_pendingDialogue;
        private OptionNode m_selectedOption;
        private SpeechNode m_currentSpeech;

        private bool isRoot = false;
        private List<Sprite> playerSprite;


        //--------------------------------------
        // Awake, Start, Destroy
        //--------------------------------------

        private void Awake()
        {
            // Destroy myself if I am not the singleton
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            Instance = this;

            m_uiOptions = new List<UIConversationButton>();
        }

        private void Start()
        {
            NpcIcon.sprite = BlankSprite;
            DialogueText.text = "";
            TurnOffUI();
        }

        private void OnDestroy()
        {
            Instance = null;
        }




        //--------------------------------------
        // Update
        //--------------------------------------

        private void Update()
        {
            switch (m_state)
            {
                case eState.TransitioningDialogueBoxOn:
                    {
                        m_stateTime += Time.deltaTime;
                        float t = m_stateTime / TRANSITION_TIME;

                        if (t > 1)
                        {
                            DoSpeech(m_pendingDialogue);
                            return;
                        }

                        SetColorAlpha(DialogueBackground, t);
                        SetColorAlpha(NpcIcon, t);
                        SetColorAlpha(NameText, t);
                    }
                    break;

                case eState.ScrollingText:
                    UpdateScrollingText();
                    break;

                case eState.TransitioningOptionsOn:
                    {
                        m_stateTime += Time.deltaTime;
                        float t = m_stateTime / TRANSITION_TIME;

                        if (t > 1)
                        {
                            SetState(eState.Idle);
                            return;
                        }

                        for (int i = 0; i < m_uiOptions.Count; i++)
                            m_uiOptions[i].SetAlpha(t);
                    }
                    break;

                case eState.Idle:
                    {
                        m_stateTime += Time.deltaTime;

                        if (m_currentSpeech.AutomaticallyAdvance)
                        {
                            if (m_currentSpeech.Dialogue != null || m_currentSpeech.Options == null || m_currentSpeech.Options.Count == 0)
                            {
                                if (m_stateTime > m_currentSpeech.TimeUntilAdvance)
                                {
                                    SetState(eState.TransitioningOptionsOff);
                                }
                            }
                        }
                    }
                    break;

                case eState.TransitioningOptionsOff:
                    {
                        m_stateTime += Time.deltaTime;
                        float t = m_stateTime / TRANSITION_TIME;

                        if (t > 1)
                        {
                            ClearOptions();

                            if (m_currentSpeech.AutomaticallyAdvance)
                            {
                                if (m_currentSpeech.Dialogue != null)
                                {
                                    DoSpeech(m_currentSpeech.Dialogue);
                                    return;
                                }
                                else if (m_currentSpeech.Options == null || m_currentSpeech.Options.Count == 0)
                                {
                                    EndConversation();
                                    return;
                                }  
                            }

                            if (m_selectedOption == null)
                            {
                                EndConversation();
                                return;
                            }

                            SpeechNode nextAction = m_selectedOption.Dialogue;
                            if (nextAction == null)
                            {
                                EndConversation();
                            }
                            else
                            {
                                DoSpeech(nextAction);
                            }
                            return;
                        }


                        for (int i = 0; i < m_uiOptions.Count; i++)
                            m_uiOptions[i].SetAlpha(1 - t);

                        SetColorAlpha(DialogueText, 1 - t);
                    }
                    break;

                case eState.TransitioningDialogueOff:
                    {
                        m_stateTime += Time.deltaTime;
                        float t = m_stateTime / TRANSITION_TIME;

                        if (t > 1)
                        {
                            TurnOffUI();
                            return;
                        }

                        SetColorAlpha(DialogueBackground, 1 -t);
                        SetColorAlpha(NpcIcon, 1 - t);
                        SetColorAlpha(NameText, 1 - t);
                    }
                    break;
            }
        }

        private void UpdateScrollingText()
        {
            const float charactersPerSecond = 1500;
            float timePerChar = (60.0f / charactersPerSecond);
            timePerChar *= ScrollSpeed;

            m_elapsedScrollTime += Time.deltaTime;

            if (m_elapsedScrollTime > timePerChar)
            {
                m_elapsedScrollTime = 0f;

                DialogueText.maxVisibleCharacters = m_scrollIndex;
                m_scrollIndex++;

                // Finished?
                if (m_scrollIndex >= m_targetScrollTextCount)
                {
                    SetState(eState.TransitioningOptionsOn);
                }
            }
        }




        //--------------------------------------
        // Set state
        //--------------------------------------

        private void SetState(eState newState)
        {
            switch (m_state)
            {
                case eState.TransitioningOptionsOff:
                    m_selectedOption = null;
                    break;
            }

            m_state = newState;
            m_stateTime = 0f;

            switch (m_state)
            {
                case eState.TransitioningDialogueBoxOn:
                    {
                        SetColorAlpha(DialogueBackground, 0);
                        SetColorAlpha(NpcIcon, 0);
                        SetColorAlpha(NameText, 0);

                        DialogueText.text = "";
                        NameText.text = m_pendingDialogue.Name;
                        NpcIcon.sprite = m_pendingDialogue.Icon != null ? m_pendingDialogue.Icon : BlankSprite;
                    }
                    break;

                case eState.ScrollingText:
                    {
                        SetColorAlpha(DialogueText, 1);

                        if (m_targetScrollTextCount == 0)
                        {
                            SetState(eState.TransitioningOptionsOn);
                            return;
                        }
                    }
                    break;

                case eState.TransitioningOptionsOn:
                    {
                        for (int i = 0; i < m_uiOptions.Count; i++)
                        {
                            m_uiOptions[i].gameObject.SetActive(true);
                        }
                    }
                    break;
            }     
        }




        //--------------------------------------
        // Start / End Conversation
        //--------------------------------------
        public void StartConversation(NPCConversation conversation, List<Sprite> playerSprite)
        {
            this.playerSprite = playerSprite;
            StartConversation(conversation);
        }

        public void StartConversation(NPCConversation conversation)
        {
            m_conversation = conversation.Deserialize();
            if (OnConversationStarted != null)
                OnConversationStarted.Invoke();

            TurnOnUI();
            m_pendingDialogue = m_conversation.Root;
            isRoot = true;
            SetState(eState.TransitioningDialogueBoxOn);
        }

        public void EndConversation()
        {
            SetState(eState.TransitioningDialogueOff);

            if (OnConversationEnded != null)
                OnConversationEnded.Invoke();
        }




        //--------------------------------------
        // Do Speech
        //--------------------------------------

        public void DoSpeech(SpeechNode speech)
        {
            if (speech == null)
            {
                EndConversation();
                return;
            }

            m_currentSpeech = speech;

            // Clear current options
            ClearOptions();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (speech.Name == "Player Name")
            {
                if (playerSprite != null && playerSprite.Count > 0)
                {
                    speech.Icon = playerSprite[PlayerInfo.PlayerCharacterID];
                }
                speech.Name = textInfo.ToTitleCase(PlayerInfo.PlayerName.ToLower());
            }

            // Set sprite
            if (speech.Icon == null)
            {
                NpcIcon.sprite = BlankSprite;
            }
            else
            {
                NpcIcon.sprite = speech.Icon;
            }

            // Set font
            if (speech.TMPFont != null)
            {
                DialogueText.font = speech.TMPFont;
            }
            else
            {
                DialogueText.font = null;
            }

            // Set name
            NameText.text = speech.Name;

            // Set text
            if (string.IsNullOrEmpty(speech.Text))
            {
                if (ScrollText)
                {
                    DialogueText.text = "";
                    m_targetScrollTextCount = 0;
                    DialogueText.maxVisibleCharacters = 0;
                    m_elapsedScrollTime = 0f;
                    m_scrollIndex = 0;
                }
                else
                {
                    DialogueText.text = "";
                    DialogueText.maxVisibleCharacters = 1;
                }
            }
            else
            {
                if(isRoot && SceneManager.GetActiveScene().buildIndex == (int)SceneManagerController.Scenes.INTRODUCTION)
                {
                    speech.Text = string.Format("Hi, {0}!", textInfo.ToTitleCase(PlayerInfo.PlayerName.ToLower()));
                    isRoot = false;
                }

                if (ScrollText)
                {
                    DialogueText.text = speech.Text;
                    m_targetScrollTextCount = speech.Text.Length + 1;
                    DialogueText.maxVisibleCharacters = 0;
                    m_elapsedScrollTime = 0f;
                    m_scrollIndex = 0;
                }
                else
                {
                    DialogueText.text = speech.Text;
                    DialogueText.maxVisibleCharacters = speech.Text.Length;
                }
            }



            // Call the event
            if (speech.Event != null)
                speech.Event.Invoke();

            // Play the audio
            if (speech.Audio != null)
            {
                AudioPlayer.clip = speech.Audio;
                AudioPlayer.volume = speech.Volume;
                AudioPlayer.Play();
            }

            // Display new options
            if (speech.Options.Count > 0)
            {
                for (int i = 0; i < speech.Options.Count; i++)
                {
                    UIConversationButton option = GameObject.Instantiate(ButtonPrefab, OptionsPanel);
                    option.InitButton(speech.Options[i]);
                    option.SetOption(speech.Options[i]);
                    m_uiOptions.Add(option);
                }
            }
            else
            {
                // Display "Continue" / "End" if we should.
                bool notAutoAdvance = !speech.AutomaticallyAdvance;
                bool autoWithOption = (speech.AutomaticallyAdvance && speech.AutoAdvanceShouldDisplayOption);
                if (notAutoAdvance || autoWithOption)
                {
                    // Else display "continue" button to go to following dialogue
                    if (speech.Dialogue != null)
                    {
                        UIConversationButton option = GameObject.Instantiate(ButtonPrefab, OptionsPanel);
                        option.SetFollowingAction(speech.Dialogue);
                        m_uiOptions.Add(option);
                    }
                    // Else display "end" button
                    else
                    {
                        UIConversationButton option = GameObject.Instantiate(ButtonPrefab, OptionsPanel);
                        option.SetAsEndConversation();
                        m_uiOptions.Add(option);
                    }
                }
            }

            // Set the button sprite and alpha
            for (int i = 0; i < m_uiOptions.Count; i++)
            {
                m_uiOptions[i].SetImage(OptionImage, OptionImageSliced);
                m_uiOptions[i].SetAlpha(0);
                m_uiOptions[i].gameObject.SetActive(false);
            }

            SetState(eState.ScrollingText);
        }



        //--------------------------------------
        // Option Selected
        //--------------------------------------

        public void OptionSelected(OptionNode option)
        {
            m_selectedOption = option;
            SetState(eState.TransitioningOptionsOff);
        }




        //--------------------------------------
        // Util
        //--------------------------------------

        private void TurnOnUI()
        {
            DialoguePanel.gameObject.SetActive(true);
            OptionsPanel.gameObject.SetActive(true);

            if (BackgroundImage != null)
            {
                DialogueBackground.sprite = BackgroundImage;

                if (BackgroundImageSliced)
                    DialogueBackground.type = Image.Type.Sliced;
                else
                    DialogueBackground.type = Image.Type.Simple;
            }

            NpcIcon.sprite = BlankSprite;
        }

        private void TurnOffUI()
        {
            DialoguePanel.gameObject.SetActive(false);
            OptionsPanel.gameObject.SetActive(false);
            SetState(eState.Off);
#if UNITY_EDITOR
            // Debug.Log("[ConversationManager]: Conversation UI off.");
#endif
        }

        private void ClearOptions()
        {
            while (m_uiOptions.Count != 0)
            {
                GameObject.Destroy(m_uiOptions[0].gameObject);
                m_uiOptions.RemoveAt(0);
            }
        }

        private void SetColorAlpha(MaskableGraphic graphic, float a)
        {
            Color col = graphic.color;
            col.a = a;
            graphic.color = col;
        }
    }
}