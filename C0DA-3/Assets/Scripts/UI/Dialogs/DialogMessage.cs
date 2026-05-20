using UnityEngine;

public class DialogMessage
{
    private DialogNPC npc;
    private string messageKey;
    
    public DialogNPC CurrentNPC => npc;
    public string CurrentMessageKey => messageKey;
    
    public DialogMessage(DialogNPC npc, string messageKey)
    {
        this.npc = npc;
        this.messageKey = messageKey;
    }
}
