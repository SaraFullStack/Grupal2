using UnityEngine;

public enum DialogNPC
{
    Coda,
    Fox,
    Bear,
    Diodo
}

static class DialogNPCMethods
{
    public static string GetNameKey(this DialogNPC npc)
    {
        switch (npc)
        {
            case DialogNPC.Coda:
                return "coda_name";
            case DialogNPC.Fox:
                return "npc_fox_name";
            case DialogNPC.Bear:
                return "npc_bear_name";
            case DialogNPC.Diodo:
                return "npc_diodo";
            default:
                return "";
        }
    }
    
    public static string GetAvatarName(this DialogNPC npc)
    {
        switch (npc)
        {
            case DialogNPC.Coda:
                return "coda";
            case DialogNPC.Fox:
                return "fox";
            case DialogNPC.Bear:
                return "bear";
            case DialogNPC.Diodo:
                return "diodo";
            default:
                return "";
        }
    }
}
