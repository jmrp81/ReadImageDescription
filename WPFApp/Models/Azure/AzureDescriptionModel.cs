using System.Collections.Generic;

namespace GetDescriptionImageApp.Models.Azure
{
    public class AzureDescriptionModel
    {
        public List<string> Tags { get; set; }
        public List<AzureCaptionModel> Captions { get; set; }
        public string ErrorMessage { get; set; }
        public override string ToString()
        {
            string result = $"Tags: '{Tags}' with Captions: '{Captions}'";
            return result;
        }
    }
}
