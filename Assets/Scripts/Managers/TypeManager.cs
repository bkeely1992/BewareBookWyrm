using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeManager : Singleton<TypeManager>
{
    public Text wordOutputText = null;
    public float backspaceUpdateThreshold = 0.25f;

    public delegate void TypeAction();
    public static event TypeAction OnTyped;

    public string WrittenInput => _writtenInput;
    private string _writtenInput = "";
    private float _timeSinceLastBackspace = 0f;

    private void Start()
    {
        if(wordOutputText == null)
        {
            wordOutputText = GetComponent<Text>();
            if(wordOutputText == null)
            {
                gameObject.SetActive(false);
                Debug.LogError("Could not find word output text on object. Disabling object.");
                return;
            }
        }
    }


    private void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.anyKey)
        {
            string keysPressed = Input.inputString;

            if (Input.GetKey(KeyCode.Backspace) && _writtenInput.Length != 0)
            {
                _timeSinceLastBackspace += Time.deltaTime;
                if(_timeSinceLastBackspace > backspaceUpdateThreshold)
                {
                    _writtenInput = _writtenInput.Substring(0, _writtenInput.Length - 1);
                    wordOutputText.text = _writtenInput;
                    if(OnTyped != null)
                    {
                        OnTyped();
                    }
                    
                    _timeSinceLastBackspace = 0f;
                }
                
            }
            else if (keysPressed.Length == 1)
            {
                _timeSinceLastBackspace = 0f;
                _writtenInput += keysPressed;
                wordOutputText.text = _writtenInput;
                if(OnTyped != null)
                {
                    OnTyped();
                }
                
            }
        }
    }

    public void ClearText()
    {
        _writtenInput = "";
        wordOutputText.text = _writtenInput;
    }
}
