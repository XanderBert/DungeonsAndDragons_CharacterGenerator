using System.Collections.Generic;

namespace DungeonsAndDragonsApp.Model.DnD
{
    public static class DungeonsAndDragons_api
    {
        public static string BaseUrl { get; } = "https://www.dnd5eapi.co/api/";
        public static string BaseSite { get; } = "https://www.dnd5eapi.co";
        public static List<Race> Races { get; private set; }
        public static List<Class> Classes { get; private set; }

        static DungeonsAndDragons_api()
        {
            Races = API.Get<RaceResponse>(BaseUrl + "races").Races;

            //Load Image for each Race
            foreach (Race race in Races)
            {
                race.LoadImage();
            }

            Classes = API.Get<ClassResponse>(BaseUrl + "classes").Classes;

            //Load image for each class
            foreach (Class actualClass in Classes)
            {
                actualClass.LoadImage();
                actualClass.LoadClassMembers();
                actualClass.ClassMembers.SavingThrowsAsString = actualClass.GetSavingThrowsAsString();
            }
        }
    }
}
