using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public int Chapter;

    public NPC[] NPCs;

    public void changeChapter(int chapter)
    {
        for(int i = 0; i< NPCs.Length; i++)
        {
            NPCs[i].Chapter = chapter;
        }
    }
}
