using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace DungeonsAndDragonsApp.Model
{
    public class Character : ObservableObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string characterClass;
        public string Class
        {
            get { return characterClass; }
            set { SetProperty(ref characterClass, value); }
        }

        private string race;
        public string Race
        {
            get { return race; }
            set { SetProperty(ref race, value); }
        }

        private string personality;
        public string Personality
        {
            get { return personality; }
            set { SetProperty(ref personality, value); }
        }

        private string background;
        public string Background
        {
            get { return background; }
            set { SetProperty(ref background, value); }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value); }
        }

        public void Reset()
        {
            Name = "";
            Class = "";
            Race = "";
            Personality = "";
            Background = "";
            ImageUrl = "";
        }

    }
}
