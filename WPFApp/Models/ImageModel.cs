using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Resources;
using System.Globalization;
using GetDescriptionImageApp.Resources;

namespace GetDescriptionImageApp.Models
{
    public class ImageModel
    {
        public string Url { get; set; }
        public string Alt { get; set; }
        public string Description { get; set; }
        public string Autor { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public ImageSource ImageSource { get; set; }
        public BitmapImage ImageData { get; set; }

        public override string ToString()
        {
            ResourceManager resourceManager = new ResourceManager("GetDescriptionImageApp.Resources.GeneralResources", typeof(GeneralResources).Assembly);
            string imageResource = resourceManager.GetString("Image", CultureInfo.CurrentCulture);
            string withoutDescriptionResource = resourceManager.GetString("WithoutDescription", CultureInfo.CurrentCulture);

            string customToString = imageResource + (string.IsNullOrEmpty(Alt) ? withoutDescriptionResource : Alt);

            return customToString;
        }
    }
}
