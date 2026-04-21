using UnityEngine;

public enum TutorialType
{
    DobleJump,
    Rush
}

static class TutorialTypeMethods
{
    public static string GetTitle(this TutorialType type)
    {
        switch (type)
        {
            case TutorialType.DobleJump:
                return "tutorial1_title";
            case TutorialType.Rush:
                return "tutorial2_title";
            default:
                return "";
        }
    }
    
    public static string GetMessage(this TutorialType type)
    {
        switch (type)
        {
            case TutorialType.DobleJump:
                return "tutorial1_message";
            case TutorialType.Rush:
                return "tutorial2_message";
            default:
                return "";
        }
    }
}