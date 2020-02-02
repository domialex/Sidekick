namespace Sidekick.Business.Languages.UI.Implementations
{
    [UILanguage("fr")]
    public class UILanguageFR : UILanguageEN
    {
        public new string LanguageName => "fr";

        public new string OverlayAccountName => "Nom de compte";
        public new string OverlayCharacter => "Nom du personnage";
        public new string OverlayPrice => "Prix";
        public new string OverlayItemLevel => "iLvl";
        public new string OverlayAge => "Âge";
    }
}
