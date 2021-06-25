using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WordBlock
{
    public delegate void TypeEvent();
    public TypeEvent onMatch, onComplete, onNoMatch, onRemove, onAction;

    public Text DisplayText = null;

    public string Identifier => _identifier;
    private string _identifier = "";

    public bool Complete => _complete;
    private bool _complete = false;

    private string _colourCode = "ffffffff";

    private string _block = "";

    public WordBlock(string inIdentifier, Text inDisplayText, string inBlock, Color highlight)
    {
        DisplayText = inDisplayText;
        _identifier = inIdentifier;
        _block = inBlock;
        _colourCode = ColorUtility.ToHtmlStringRGBA(highlight);
        _complete = false;
    }

    public void HandleProgress(string playerInput)
    {
        int progressLength = playerInput.Length;
        _complete = false;

        //Can't match if the input is longer than the block
        if(progressLength > _block.Length)
        {
            return;
        }

        if (_block.Substring(0, progressLength) == playerInput)
        {
            //If there are any match events, run them
            onMatch?.Invoke();

            if (progressLength >= _block.Length)
            {
                //If there are any completion events, run them
                _complete = true;
                onComplete?.Invoke();
            }

            return;
        }

        //If there are any no match events, run them
        onNoMatch?.Invoke();
        
        return;
    }

    public void HighlightProgress()
    {
        string progress = TypeManager.Instance.playerEntryText.text;
        string highlightVariant = "<color=#"+_colourCode+">"+progress + "</color>";

        //Create highlighted variant of the text and apply it to the text
        if(DisplayText != null)
        {
            DisplayText.text = ReplaceFirst(_block, progress, highlightVariant);
        }
    }

    private string ReplaceFirst(string inText, string inInitialValue, string inReplacementValue)
    {
        int firstPosition = inText.IndexOf(inInitialValue);

        if (firstPosition < 0)
        {
            return inText;
        }
        return inText.Substring(0, firstPosition) + inReplacementValue + inText.Substring(firstPosition + inInitialValue.Length);
    }

    public void ClearProgress()
    {
        DisplayText.text = _block;
    }
}

