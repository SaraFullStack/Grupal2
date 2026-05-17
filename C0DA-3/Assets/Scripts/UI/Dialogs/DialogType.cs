using UnityEngine;

public enum DialogType
{
    FoxDialog01,
    BearDialog01,
    BearClawUnlock
}

static class DialogTypeMethods
{
    public static Dialog GetDialog(this DialogType type)
    {
        switch (type)
        {
            case DialogType.FoxDialog01:
                
                return new Dialog( new DialogMessage[] {
                    new DialogMessage( DialogNPC.Fox, "dialogA01"),
                    new DialogMessage( DialogNPC.Coda, "dialogA02"),
                    new DialogMessage( DialogNPC.Fox, "dialogA03"),
                    new DialogMessage( DialogNPC.Coda, "dialogA04"),
                    new DialogMessage( DialogNPC.Fox, "dialogA05"),
                    new DialogMessage( DialogNPC.Coda, "dialogA06")
                });
            
            case DialogType.BearDialog01:

                return new Dialog(new DialogMessage[] {
                    new DialogMessage( DialogNPC.Bear, "dialogB01"),
                    new DialogMessage( DialogNPC.Coda, "dialogB02"),
                    new DialogMessage( DialogNPC.Bear, "dialogB03"),
                    new DialogMessage( DialogNPC.Coda, "dialogB04"),

                });
            case DialogType.BearClawUnlock:
                return new Dialog(new DialogMessage[] {
                new DialogMessage(DialogNPC.Diodo, "bear_claw_unlock_01")
            });

            default:
                return new Dialog(new DialogMessage[] { });
        }
    }

}