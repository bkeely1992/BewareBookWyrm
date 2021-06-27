using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterInteraction : MonoBehaviour
{
    public string Identifier = "";
    public Text moveText;
    public InteractionData interactionData;
    public WordBlock wordBlock = null;

    public void SetInteractionData(InteractionData inInteractionData)
    {
        if (wordBlock != null)
        {
            //This encounter has an existing word block that must be removed.
        }

        interactionData = inInteractionData;

        if (!moveText)
        {
            Debug.LogError("Text object has not been set for encounter interaction. Cannot assign word block.");
            return;
        }

        Color moveColour = GameDataManager.Instance.GetStatData(interactionData.StatName).Colour;
        Color highlightColour = GameDataManager.Instance.GetStatData(interactionData.StatName).Highlight;
        moveText.text = interactionData.Name;
        moveText.color = moveColour;

        wordBlock = new WordBlock(interactionData.Identifier, moveText, interactionData.Name, highlightColour);

        //Need to set the listeners
        wordBlock.onMatch += wordBlock.HighlightProgress;
        wordBlock.onNoMatch += wordBlock.ClearProgress;
        wordBlock.onAction += ChooseMove;
        wordBlock.onRemove += RemoveWordBlock;

        TypeManager.Instance.AddFreeFormWordBlock(wordBlock);
    }

    public void ChooseMove()
    {
        GameFlowManager.Instance.encounterManager.ChooseInteraction(Identifier);
    }

    private void RemoveWordBlock()
    {
        wordBlock.onMatch -= wordBlock.HighlightProgress;
        wordBlock.onNoMatch -= wordBlock.ClearProgress;

        wordBlock.onRemove -= RemoveWordBlock;
    }
}
