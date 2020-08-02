namespace GetDescriptionImageApp.Models.Azure
{
    public class AzureCaptionModel
    {
        public string Text { get; set; }
        public decimal Confidence { get; set; }

        public override string ToString()
        {
            string result = $"Image Description: '{Text}' with Confidence: '{Confidence}'";
            return result;
        }
    }
}
