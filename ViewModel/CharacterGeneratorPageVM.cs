using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DungeonsAndDragonsApp.Model;
using DungeonsAndDragonsApp.Model.AI;
using DungeonsAndDragonsApp.Model.DnD;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<Race> Races { get; set; }
        public List<Class> Classes { get; set; }

        private Class selectedClass;
        public Class SelectedClass
        {
            get { return selectedClass; }
            set
            {
                selectedClass = value;
                OnPropertyChanged();
            }
        }

        private Race selectedRace;
        public Race SelectedRace
        {
            get { return selectedRace; }
            set
            {
                selectedRace = value;
                OnPropertyChanged();
            }
        }

        public Character GeneratedCharacter { get; set; } 

        public async void Generate()
        {
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

            //Calculate the Appearance response
            string appearancePrompt = GetKeywordResponse(ref response, "Appearance: ", "Personality: ");
            if (appearancePrompt.Length <= 0)
            {
                Generate();
                return;
            }
            
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
