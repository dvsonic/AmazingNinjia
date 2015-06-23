using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.Security;

public class GameData 
{
    public static int score = 0;
    public static int hp;
    public const int MAX_HP = 3;

    private static SecurityElement _language;
    public static SecurityElement getLanguage()
    {
        if(null == _language)
        {
            string language = "config/language_en";
            if(Application.systemLanguage == SystemLanguage.Chinese)
                language = "config/language_cn";
            SecurityParser SP = new SecurityParser();
            SP.LoadXml(Resources.Load(language).ToString());
            _language = SP.ToXml();
        }
        return _language;
    }
}
