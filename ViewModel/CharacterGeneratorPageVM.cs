using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DungeonsAndDragonsApp.Model;
using DungeonsAndDragonsApp.Model.AI;
using DungeonsAndDragonsApp.Model.DnD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DungeonsAndDragonsApp.ViewModel
{
    public class CharacterGeneratorPageVM : ObservableObject
    {
        public CharacterGeneratorPageVM()
        {
            Races = DungeonsAndDragons_api.Races;
            Classes = DungeonsAndDragons_api.Classes;
            GenerateCommand = new RelayCommand(Generate);
            GeneratedCharacter = new Character();
        }

        public RelayCommand GenerateCommand { get; private set; }

        private List<Race> races;
        public List<Race> Races
        {
            get { return races; }
            set { SetProperty(ref races, value); }
        }

        private List<Class> classes;
        public List<Class> Classes
        {
            get { return classes; }
            set { SetProperty(ref classes, value); }
        }

        private Class selectedClass;
        public Class SelectedClass
        {
            get { return selectedClass; }
            set { SetProperty(ref selectedClass, value); }
        }

        private Race selectedRace;
        public Race SelectedRace
        {
            get { return selectedRace; }
            set { SetProperty(ref selectedRace, value); }
        }

        private Character generatedCharacter;
        public Character GeneratedCharacter
        {
            get { return generatedCharacter; }
            set { SetProperty(ref generatedCharacter, value); }
        }

        private string savingThrows;
        public string SavingThrows
        {
            get { return savingThrows; }
            set { SetProperty(ref savingThrows, value); }
        }

        public async void Generate()
        {
            generatedCharacter.Reset();

            string prompt = "Give me a detailed character background/description for a dungeons and dragons character its class is " + SelectedClass.ClassName + " And its Race is " + SelectedRace.RaceName + ", Make sure to structure your output as follows: Name:, \nRace:, \nClass:, \nAppearance:, \nPersonality: and \nBackground:. Make Sure you structure it like that every time and the given words to define the structure are present. the Appearance clarification should not be more then 100 words";

            ////Add user added keywords if there are given
            //if (keywordsEntry.ToString() != string.Empty)
            //{
            //    prompt += " Some Additional keywords are: " + Keywords;
            //}

            //Start the chat with ChatGPT
            var chat = AIGenerator_api.Api.Chat.CreateConversation();
            chat.AppendUserInput(prompt);

            //Wait For Chat GPT To respond
            string response = await chat.GetResponseFromChatbotAsync();

            //Calculate the Name response
            string nameResponse = GetKeywordResponse(ref response, "Name: ", "Race: ");
            if (nameResponse.Length <= 0)
            {
                Generate();
                return;
            }
            GeneratedCharacter.Name = nameResponse;


            //Calculate the Race response
            string raceResponse = GetKeywordResponse(ref response, "Race: ", "Class: ");
            if (raceResponse.Length <= 0)
            {
                Generate();
                return;
            }
            GeneratedCharacter.Race = raceResponse;


            //Calculate the Class response
            string classResponse = GetKeywordResponse(ref response, "Class: ", "Appearance: ");
            if (classResponse.Length <= 0)
            {
                Generate();
                return;
            }
            GeneratedCharacter.Class = classResponse;

            //Calculate the Appearance response
            string appearancePrompt = GetKeywordResponse(ref response, "Appearance: ", "Personality: ");
            if (appearancePrompt.Length <= 0)
            {
                Generate();
                return;
            }


            int pFrom = response.IndexOf("Background: ") + "Background: ".Length;
            string backResponse = response.Substring(pFrom, response.Length - 1 - pFrom);
            if (backResponse.Length <= 0)
            {
                Generate();
                return;
            }
            GeneratedCharacter.Background = backResponse;

            //Print response to the output window for debugging
            Console.WriteLine(response);



            //Generate an image on the appearance response 
            string endAppearancePrompt = "Create a pencil drawing only in black and white a fantasy being with this appearance: " + appearancePrompt;
            var result = await AIGenerator_api.Api.ImageGenerations.CreateImageAsync(endAppearancePrompt);
            GeneratedCharacter.ImageUrl = result.Data[0].Url;

            Console.WriteLine(GeneratedCharacter.ImageUrl);
        }

        public string GetKeywordResponse(ref string response, string Keyword, string nextKeyword)
        {
            //Calculate the keyword response
            int pFrom = response.IndexOf(Keyword) + Keyword.Length;
            int pTo = response.LastIndexOf(nextKeyword);
            string keywordResponse = response.Substring(pFrom, pTo - pFrom);

            return keywordResponse;
        }
    }
}
