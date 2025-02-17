﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this script needs to be put on the actor, and takes care of the current step to accomplish.
//the step contains a dialogue and maybe an event.

public class StepController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ActorSO _actor = default;
	[SerializeField] private DialogueDataSO _defaultDialogue = default;
	[SerializeField] private QuestManagerSO _questData = default;

	[Header("Listening to channels")]
	[SerializeField] private DialogueActorChannelSO _interactionEvent = default;
	[SerializeField] private VoidEventChannelSO _winDialogueEvent = default;
	[SerializeField] private VoidEventChannelSO _loseDialogueEvent = default;

	[Header("Broadcasting on channels")]
	[SerializeField] private DialogueDataChannelSO _startDialogueEvent = default;

	//check if character is actif. An actif character is the character concerned by the step.

	private DialogueDataSO _currentDialogue;

	private void Start()
	{

		if (_winDialogueEvent != null)
		{ _winDialogueEvent.OnEventRaised += PlayWinDialogue; }
		if (_loseDialogueEvent != null)
		{ _loseDialogueEvent.OnEventRaised += PlayLoseDialogue; }


	}

	void PlayDefaultDialogue()
	{

		if (_defaultDialogue != null)
		{
			_currentDialogue = _defaultDialogue;
			StartDialogue();
		}

	}


	//start a dialogue when interaction
	//some Steps need to be instantanious. And do not need the interact button.
	//when interaction again, restart same dialogue.
	public void InteractWithCharacter()
	{

		DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, false, false);
		Debug.Log("dialogue " + displayDialogue + "actor" + _actor);
		if (displayDialogue != null)
		{
			_currentDialogue = displayDialogue;
			StartDialogue();
		}
		else
		{
			PlayDefaultDialogue();
		}

	}


	void StartDialogue()
	{
		if (_startDialogueEvent != null)
		{
			_startDialogueEvent.RaiseEvent(_currentDialogue);
		}
	}
	void PlayLoseDialogue()
	{
		if (_questData != null)
		{
			DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, true, false);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}

		}



	}
	void PlayWinDialogue()
	{
		if (_questData != null)
		{
			DialogueDataSO displayDialogue = _questData.InteractWithCharacter(_actor, true, true);
			if (displayDialogue != null)
			{
				_currentDialogue = displayDialogue;
				StartDialogue();
			}

		}
	}

}
