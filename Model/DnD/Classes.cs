using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DungeonsAndDragonsApp.Model.DnD
{
    public class ClassResponse
    {
        [JsonProperty("results")]
        public List<Class> Classes { get; set; }
    }


    public class ClassMembers
    {
        [JsonProperty("hit_die")]
        public int HitDie { get; set; }
        [JsonProperty("proficiencies")]
        public List<Proficiency> Proficiencies { get; set; }
        [JsonProperty("saving_throws")]
        public List<SavingThrow> SavingThrows { get; set; }
        [JsonIgnore]
        public string SavingThrowsAsString { get; set; }
    }

    public class Class
    {

        [JsonProperty("name")]
        public string ClassName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public Image ClassImage { get; private set; }


        public ClassMembers ClassMembers { get; set; }

        public void LoadImage()
        {
            ClassImage = new Image();

            try
            {
                ClassImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Classes/" + ClassName + ".png"));
            }
            catch (Exception ex)
            {
                ClassImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Classes/NotFound.png"));
            }
        }

        public void LoadClassMembers()
        {
            string baseUrl = DungeonsAndDragons_api.BaseSite;
            if (!string.IsNullOrEmpty(Url))
            {
                ClassMembers = API.Get<ClassMembers>(baseUrl + Url);
            }
        }

        public string GetSavingThrowsAsString()
        {
            string savingThrows = "";

            foreach (SavingThrow save in ClassMembers.SavingThrows)
            {
                savingThrows += save.Name + ", ";
            }
            savingThrows = savingThrows.TrimEnd(' ');
            savingThrows = savingThrows.TrimEnd(',');
            return savingThrows;
        }
    }
}
