﻿using System.Collections;
using System.Collections.Generic;
using SoundManagers.VOs;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#CCFFCC")]
    public class Chat : DialogueBaseNode {

        public CharacterInfo character;
        [TextArea] public string text;
        public AudioClip voiceClip;
        public RandomVoiceClips randomClips;
        [HideInInspector] public bool isAlreadyShown;
        [Output(instancePortList = true)] public List<Answer> answers = new List<Answer>();

        [System.Serializable] public class Answer {
            public string text;
            public AudioClip voiceClip;
        }

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Count == 0) {
                port = GetOutputPort("output");
            } else {
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public bool HasAnswerGotOutput(int index)
        {
            if (answers.Count <= index)
            {
                return false;
            } else
            {
                return GetOutputPort("answers " + index).IsConnected;
            }
        }

        public override void Trigger() {
            (graph as DialogueGraph).current = this;
        }
    }
}