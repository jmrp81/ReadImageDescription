using GetDescriptionImageApp.Models.Azure;
using System.Threading.Tasks;

namespace GetDescriptionImageApp.BusinessLogic
{
    public interface IAzureBLogic
    {
        AzureDescriptionModel GetAzureDescriptionImage(byte[] imageBytes, string base64ImageString);
    }
}
