using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterMove : MonoBehaviour
{
    public string Identifier = "";
    public Text moveText;
    public MoveData moveData;
    public WordBlock wordBlock = null;

    public void SetMoveData(MoveData inMoveData)
    {
        if (wordBlock != null)
        {
            //This encounter has an existing word block that must be removed.
        }

        moveData = inMoveData;

        if (!moveText)
        {
            Debug.LogError("Text object has not been set for encounter move. Cannot assign word block.");
            return;
        }

        Color moveColour = GameDataManager.Instance.GetStatData(moveData.StatName).Colour;
        Color highlightColour = GameDataManager.Instance.GetStatData(moveData.StatName).Highlight;
        moveText.text = moveData.Name;
        moveText.color = moveColour;

        wordBlock = new WordBlock(moveData.Identifier, moveText, moveData.Name, highlightColour);

        //Need to set the listeners
        wordBlock.onMatch += wordBlock.HighlightProgress;
        wordBlock.onNoMatch += wordBlock.ClearProgress;
        wordBlock.onAction += ChooseMove;
        wordBlock.onRemove += RemoveWordBlock;

        TypeManager.Instance.AddFreeFormWordBlock(wordBlock);
    }

    public void ChooseMove()
    {
        GameFlowManager.Instance.encounterManager.ChooseMove(Identifier);
    }

    private void RemoveWordBlock()
    {
        wordBlock.onMatch -= wordBlock.HighlightProgress;
        wordBlock.onNoMatch -= wordBlock.ClearProgress;

        wordBlock.onRemove -= RemoveWordBlock;
    }
}
