using System;
using System.Collections.Generic;
using System.Text;

namespace GetDescriptionImageApp.Models.Azure
{
   public class AzureTranslateResponse
    {
        public AzureTranslateDetectedLanguage detectedLanguage { get; set; }

        public List<AzureTranslateTranslation> translations { get; set; }
    }
}
