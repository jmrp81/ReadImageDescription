namespace GetDescriptionImageApp.Models.Azure
{
    public class AzureDescribeResponseModel
    {
        public string RequestId { get; set; }
        public AzureDescriptionModel Description { get; set; }
        public AzureMetadaModel Metadata { get; set; }

        public override string ToString()
        {
            return Description.ToString();
        }
    }
}
