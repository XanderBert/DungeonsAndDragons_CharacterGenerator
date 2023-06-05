using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonsAndDragonsApp.Model.AI
{
    public class PromptManager : INotifyPropertyChanged
    {
        private string prompt;

        public string Prompt
        {
            get { return prompt; }
            set
            {
                if (prompt != value)
                {
                    prompt = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class AIGenerator_api : ObservableObject
    {
        static AIGenerator_api()
        {
            AuthenticationKey = "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";
            Api = new OpenAI_API.OpenAIAPI(AuthenticationKey);
            Chat = Api.Chat.CreateConversation();
            Character = new Character();
            Prompt  = "Give me a detailed character background/description for a dungeons and dragons character its class is " + Character.Class + " And its Race is " + Character.Race + ", Make sure to structure your output as follows: Name:, \nRace:, \nClass:, \nAppearance:, \nPersonality: and \nBackground:. Make Sure you structure it like that every time and the given words to define the structure are present. the Appearance clarification should not be more then 80 words.";
        }
        static public string AuthenticationKey { get; set; }
        static public OpenAI_API.OpenAIAPI Api { get; set; }
        static public OpenAI_API.Chat.Conversation Chat { get; private set; }
        static public Character Character { get; set; } = new Character();

        private static PromptManager promptManager = new PromptManager();

        public static string Prompt
        {
            get { return promptManager.Prompt; }
            set { promptManager.Prompt = value; }
        }


        static public Character GetGeneratedCharacter() 
        {
            return Character;
        }

        static public async Task Generate(string className, string raceName, string optionalKeywords)
        {
            Chat = Api.Chat.CreateConversation();
            Character.Class = className;
            Character.Race = raceName;
            string basePrompt = Prompt;

            //Start the chat with ChatGPT
            //Add user added keywords if there are given
            if (!string.IsNullOrEmpty(optionalKeywords))
            {
                basePrompt += " Some Additional keywords are: " + optionalKeywords;
            }

            Chat.AppendUserInput(basePrompt);
            string response = await Chat.GetResponseFromChatbotAsync();

            //Calculate the Name response
            string nameResponse = GetKeywordResponse(ref response, "Name: ", "Race: ");
            if (nameResponse.Length <= 0)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Generate(className, raceName, optionalKeywords);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return;
            }
            Character.Name = nameResponse;


            //Calculate the Appearance response
            string appearancePrompt = GetKeywordResponse(ref response, "Appearance: ", "Personality: ");
            if (appearancePrompt.Length <= 0)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Generate(className, raceName, optionalKeywords);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return; 
            }


            int pFrom = response.IndexOf("Background: ") + "Background: ".Length;
            string backResponse = response.Substring(pFrom, response.Length - 1 - pFrom);
            if (backResponse.Length <= 0)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Generate(className, raceName, optionalKeywords);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return;

            }
            Character.Background = backResponse;


            //Generate an image on the appearance response 
            string endAppearancePrompt = "Create a pencil drawing only in black and white a fantasy being with this appearance: " + appearancePrompt + ". Make Sure its not in color";
            var result = await AIGenerator_api.Api.ImageGenerations.CreateImageAsync(endAppearancePrompt);
            Character.ImageUrl = result.Data[0].Url;
        }
        static public async Task GenerateUrl()
        {
            Chat.AppendUserInput("Can you give me a new appearance description?");
            string response = await Chat.GetResponseFromChatbotAsync();
            string endAppearancePrompt = "Create a pencil drawing only in black and white a fantasy being with this appearance: " + response + ". Make Sure its not in color";
            var result = await AIGenerator_api.Api.ImageGenerations.CreateImageAsync(endAppearancePrompt);
            Character.ImageUrl = result.Data[0].Url;
        }
        static public async Task GenerateBackground()
        {
            Chat.AppendUserInput("Can you give me a new Background description? it should not be more then 80 words. I do not need the \"Background: \" prefix.");
            Character.Background = await Chat.GetResponseFromChatbotAsync();
        }
        static public async Task GenerateName() 
        {
            Chat.AppendUserInput("Can you give me a new Name? Ant the only response i want is the name nothing else so i should not need the \"Name: \" prefix");
            Character.Name = await Chat.GetResponseFromChatbotAsync();
        }

        static private string GetKeywordResponse(ref string response, string Keyword, string nextKeyword)
        {
            //Calculate the keyword response
            int pFrom = response.IndexOf(Keyword) + Keyword.Length;
            int pTo = response.LastIndexOf(nextKeyword);
            string keywordResponse = response.Substring(pFrom, pTo - pFrom);

            return keywordResponse;
        }
    }
}
