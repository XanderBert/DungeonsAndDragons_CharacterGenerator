using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DungeonsAndDragonsApp.Model;
using DungeonsAndDragonsApp.Model.AI;
using DungeonsAndDragonsApp.Model.DnD;
using DungeonsAndDragonsApp.View;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

//Todo: Image URL Binding is throwing an error


namespace DungeonsAndDragonsApp.ViewModel
{
    public class CharacterGeneratorPageVM : ObservableObject
    {
        public CharacterGeneratorPageVM()
        {
            Races = DungeonsAndDragons_api.Races;
            Classes = DungeonsAndDragons_api.Classes;
            GeneratedCharacter = new Character();

            GenerateCommand = new RelayCommand(Generate);
            ShowFullScreenCommand = new RelayCommand(ShowFullScreen);
            GenerateNameCommand = new RelayCommand(GenerateName);
            GenerateImageCommand = new RelayCommand(GenerateURL);   
            GenerateBackgroundCommand = new RelayCommand(GenerateBackground);
        }

        public RelayCommand GenerateCommand { get; private set; }
        public RelayCommand ShowFullScreenCommand { get; private set; }
        public RelayCommand GenerateNameCommand { get; private set; }
        public RelayCommand GenerateImageCommand { get; private set; }
        public RelayCommand GenerateBackgroundCommand { get; private set; }



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

        public void ShowFullScreen() 
        {
            CharacterSheetWindow characterSheet = new CharacterSheetWindow(GeneratedCharacter);
            characterSheet.Show();
        }
        public async void Generate() 
        {
            GeneratedCharacter.Reset();
            await AIGenerator_api.Generate(SelectedClass.ClassName, SelectedRace.RaceName, "");
            GeneratedCharacter = AIGenerator_api.GetGeneratedCharacter();
        }

        public async void GenerateName ()
        {
            await AIGenerator_api.GenerateName();
            GeneratedCharacter.Name = AIGenerator_api.Character.Name;
        }

        public async void GenerateURL() 
        {
            await AIGenerator_api.GenerateUrl();
            GeneratedCharacter.ImageUrl = AIGenerator_api.Character.ImageUrl;
        }

        public async void GenerateBackground() 
        {
            await AIGenerator_api.GenerateBackground();
            generatedCharacter.Background = AIGenerator_api.Character.Background;
        }
    }
}