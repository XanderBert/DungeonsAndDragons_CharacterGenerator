using System;

namespace DungeonsAndDragonsApp.Model.AI
{
    public class AIGenerator_api
    {
        static AIGenerator_api()
        {
            AuthenticationKey = "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";
            Api = new OpenAI_API.OpenAIAPI(AuthenticationKey);
        }
        static public OpenAI_API.OpenAIAPI Api { get; set; }
        static public String AuthenticationKey { get; set; }
    }
}
