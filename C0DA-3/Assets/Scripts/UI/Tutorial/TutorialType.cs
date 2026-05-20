using UnityEngine;

public enum TutorialType
{
    RomperCajas,
    CurarDano,
    CajasCamino
}

static class TutorialTypeMethods
{
    public static string GetTitle(this TutorialType type)
    {
        switch (type)
        {
            case TutorialType.CajasCamino:
                return "tutorial_cajas_camino_title";

            case TutorialType.RomperCajas:
                return "tutorial_romper_cajas_title";

            case TutorialType.CurarDano:
                return "tutorial_curar_dano_title";

            default:
                return "";
        }
    }

    public static string GetMessage(this TutorialType type)
    {
        switch (type)
        {
            case TutorialType.CajasCamino:
                return "tutorial_cajas_camino_message";

            case TutorialType.RomperCajas:
                return "tutorial_romper_cajas_message";

            case TutorialType.CurarDano:
                return "tutorial_curar_dano_message";

            default:
                return "";
        }
    }
}