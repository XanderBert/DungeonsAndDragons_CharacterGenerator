using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DungeonsAndDragonsApp.Model.DnD
{
    public class RaceResponse
    {
        [JsonProperty("results")]
        public List<Race> Races { get; set; }
    }
    public class Race
    {
        [JsonProperty("name")]
        public string RaceName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public Image RaceImage { get; private set; }

        public void LoadImage()
        {
            RaceImage = new Image();

            try
            {
                RaceImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Races/" + RaceName + ".png"));
            }
            catch (Exception)
            {

                RaceImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Races/NotFound.png"));

            }
        }
    }
}
