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

    public class Class
    {

        [JsonProperty("name")]
        public string ClassName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonIgnore]
        public Image ClassImage { get; private set; }

        public void LoadImage() 
        {
            ClassImage = new Image();

            try 
            {
                ClassImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Classes/" + ClassName + ".png"));
            }
            catch(Exception ex) 
            {

                ClassImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/DungeonsAndDragonsApp;component/Resources/Classes/NotFound.png"));

            }           
        }
    }
}
