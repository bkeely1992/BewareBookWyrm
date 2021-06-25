using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TypeManager : Singleton<TypeManager>
{
    public Text playerEntryText = null;
    public float backspaceUpdateThreshold = 0.25f;

    private List<WordBlock> _activeFreeFormWordBlocks = new List<WordBlock>();
    private WordBlock _currentFocusedWordBlock = null;
    private string _currentPlayerEntry = "";

    public enum TypeState
    {
        focused, freeform, none
    }
    private TypeState _typeState = TypeState.freeform;
    private float _timeSinceLastBackspace = 0f;

    private void Start()
    {
        if(playerEntryText == null)
        {
            playerEntryText = GetComponent<Text>();
            if(playerEntryText == null)
            {
                gameObject.SetActive(false);
                Debug.LogError("Could not find word output text on object. Disabling object.");
                return;
            }
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            //May need handling to adjust the speed for deleting characters {might be too fast?}
            _timeSinceLastBackspace += Time.deltaTime;

            if(_timeSinceLastBackspace > backspaceUpdateThreshold)
            {
                _currentPlayerEntry = _currentPlayerEntry.Length > 0 ? _currentPlayerEntry.Substring(0, _currentPlayerEntry.Length - 1) : "";
                playerEntryText.text = _currentPlayerEntry;
                HandleTextChange(_currentPlayerEntry);
                _timeSinceLastBackspace = 0f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckForCompletedBlock();
            _timeSinceLastBackspace = 0f;
        }
        else
        {
            string input = Input.inputString;
            if (input != "")
            {
                _currentPlayerEntry = _currentPlayerEntry + input[0];
                playerEntryText.text = _currentPlayerEntry;
                HandleTextChange(_currentPlayerEntry);
            }
            _timeSinceLastBackspace = 0f;
        }
    }

    private void HandleTextChange(string text)
    {
        switch (_typeState)
        {
            case TypeState.freeform:
                foreach(WordBlock wordBlock in _activeFreeFormWordBlocks)
                {
                    wordBlock.HandleProgress(text);
                }
                break;
            case TypeState.focused:
                if(_currentFocusedWordBlock != null)
                {
                    _currentFocusedWordBlock.HandleProgress(text);
                }
                break;
        }
    }

    private void CheckForCompletedBlock()
    {
        switch (_typeState)
        {
            case TypeState.freeform:
                foreach (WordBlock wordBlock in _activeFreeFormWordBlocks)
                {
                    if (wordBlock.Complete)
                    {
                        wordBlock?.onAction.Invoke();
                        return;
                    }
                }
                break;
            case TypeState.focused:
                //Do not need to keep track of enter on focused typing mode.
                break;
        }
    }

    public void ClearFreeFormWordBlocks()
    {
        foreach(WordBlock wordBlock in _activeFreeFormWordBlocks)
        {
            wordBlock.onRemove?.Invoke();
        }
        _activeFreeFormWordBlocks.Clear();
    }

    public void AddFreeFormWordBlock(WordBlock inBlock)
    {
        if (_activeFreeFormWordBlocks.Contains(inBlock))
        {
            Debug.LogError("Word block[" + inBlock.Identifier + "] has already been added to active free form bank. Skipping add.");
            return;
        }
        _activeFreeFormWordBlocks.Add(inBlock);
    }
}
