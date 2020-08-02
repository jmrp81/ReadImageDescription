using GetDescriptionImageApp.Helpers;
using GetDescriptionImageApp.Models.Azure;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace GetDescriptionImageApp.BusinessLogic
{
    public class AzureBLogic : IAzureBLogic
    {
        // info API Azure Cognitive --> https://westus.dev.cognitive.microsoft.com/docs/services/5adf991815e1060e6355ad44/operations/56f91f2e778daf14a499e1fe

        // info API Azure Translate --> https://docs.microsoft.com/es-es/azure/cognitive-services/translator/reference/v3-0-reference

        private readonly Logger Logger;
        private readonly ReadWriteConfiguration readWriteConfiguration;

        private string language = "";
        private string subscriptionKey = "";
        private string urlAzure = "";
        private string urlDescribe = "";
        private string AzureTranslateSubscriptionKey = "";
        private string AzureGeneralTranslateURL = "";
        private string AzureTranslateRegion = "";
        private string urlTranslate = "";
        private int maxCandidate = 1;
        private int maxRequestAzure = 20;
        private int currentRequestAzure = 1;
        private bool useAzureTranslate = false;
        private DateTime? dateTimeInitAzureRequest;

        public AzureBLogic()
        {
            Logger = LogManager.GetCurrentClassLogger();
            readWriteConfiguration = new ReadWriteConfiguration();

            Logger.Info($"AzureBLogic Constructor - GetConfiguration from app.config");
            ReadAzureAppConfiguration();
        }

        public AzureDescriptionModel GetAzureDescriptionImage(byte[] imageBytes, string base64ImageString)
        {
            AzureDescriptionModel imageDescribe = null;

            Logger.Info($"AzureBLogic START - GetAzureDescriptionImage Action from image: '{base64ImageString}'");

            try
            {
                DateTime currentDateTime = DateTime.Now;

                if (!dateTimeInitAzureRequest.HasValue)
                {
                    dateTimeInitAzureRequest = currentDateTime;
                }
                else
                {
                    TimeSpan intervalToEvaluate = currentDateTime - dateTimeInitAzureRequest.Value;

                    if (intervalToEvaluate.Seconds > 60)
                    {
                        dateTimeInitAzureRequest = currentDateTime;
                        currentRequestAzure = 1;
                    }
                }

                if (currentRequestAzure <= maxRequestAzure)
                {
                    currentRequestAzure++;
                    ReadAzureAppConfiguration();

                    imageDescribe = Task.Run(async () => await CallAzureDescribe(imageBytes)).Result;

                    if (useAzureTranslate)
                    {
                        imageDescribe = Task.Run(async () => await CallAzureTranslate(imageDescribe)).Result;
                    }
                }
                else
                {
                    Logger.Error($"AzureBLogic ERROR - GetAzureDescriptionImage Action Maximum requests per minute have been exhausted.");
                    imageDescribe = new AzureDescriptionModel()
                    {
                        ErrorMessage = $"Alcanzado el limite de peticiones por minuto, actualmente son: '{maxRequestAzure}' por minuto."
                    };
                }
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "AzureBLogic ERROR - GetAzureDescriptionImage Action");
            }
            finally
            {
                Logger.Info($"AzureBLogic FINISH - GetAzureDescriptionImage Action from image: '{base64ImageString}' with response: '{imageDescribe}'");
            }

            return imageDescribe;
        }

        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

            return client;
        }

        private void ReadAzureAppConfiguration()
        {
            Logger.Info($"AzureBLogic START - ReadAppConfiguration from app.config");
            if (readWriteConfiguration != null)
            {
                language = readWriteConfiguration.GetAzureLanguage();
                maxCandidate = readWriteConfiguration.GetAzureMaxCandidate();
                subscriptionKey = readWriteConfiguration.GetAzureSubscriptionKey();
                urlAzure = readWriteConfiguration.GetAzureURL();
                maxRequestAzure = readWriteConfiguration.GetMaxRequestAzure();
                useAzureTranslate = readWriteConfiguration.GetUseAzureTranslate();
                AzureTranslateSubscriptionKey = readWriteConfiguration.GetAzureTranslateSubscriptionKey();
                AzureGeneralTranslateURL = readWriteConfiguration.GetAzureGeneralTranslateURL();
                AzureTranslateRegion = readWriteConfiguration.GetAzureTranslateRegion();

                urlDescribe = $"{urlAzure}vision/v2.0/describe?maxCandidates={maxCandidate}&language={language}";
                urlTranslate = $"{AzureGeneralTranslateURL}?api-version=3.0&to=es";

                Logger.Info($"AzureBLogic - ReadAppConfiguration - Get Azure Configuration language: '{language}', maxCandidate: '{maxCandidate}', subscriptionKey: '{subscriptionKey}', urlAzure: '{urlAzure}', maxRequestAzure: '{maxRequestAzure}', translateKey: '{AzureTranslateSubscriptionKey}', translateURl: '{AzureGeneralTranslateURL}', azureRegion: '{AzureTranslateRegion}'");
            }
            else
            {
                Logger.Error($"AzureBLogic ERROR - ReadAppConfiguration object is null");
            }
        }

        private async Task<AzureDescriptionModel> CallAzureDescribe(byte[] imageBytes)
        {
            Logger.Error($"AzureBLogic START - CallAzureDescribe Action");

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            AzureDescribeResponseModel imageDescribeResponse; 
            AzureDescriptionModel imageDescribe = null;

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Add the byte array as an octet stream to the request body.
            using (ByteArrayContent content = new ByteArrayContent(imageBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Asynchronously call the REST API method.
                response = await client.PostAsync(urlDescribe, content);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                imageDescribeResponse = JsonConvert.DeserializeObject<AzureDescribeResponseModel>(contentString);

                if (imageDescribeResponse != null)
                {
                    imageDescribe = imageDescribeResponse.Description;
                }
                else
                {
                    Logger.Error($"AzureBLogic ERROR - CallAzureDescribe Action response OK but NOT mapped object result");
                }
            }
            else
            {
                Logger.Error($"AzureBLogic ERROR - CallAzureDescribe Action response: '{response.RequestMessage}'");
            }

            return imageDescribe;
        }

        private async Task<AzureDescriptionModel> CallAzureTranslate(AzureDescriptionModel imageDescribe)
        {
            Logger.Error($"AzureBLogic START - CallAzureTranslate Action, imageDescribe: '{imageDescribe}'");

            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            List<AzureTranslateResponse> imageTranslateResponse;

            if (imageDescribe != null && imageDescribe.Captions.Count > 0 && !string.IsNullOrEmpty(imageDescribe.Captions[0].Text))
            {
                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AzureTranslateSubscriptionKey);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Region", AzureTranslateRegion);

                List<AzureTranslateRequest> azureTranslateRequestList = new List<AzureTranslateRequest>();

                foreach (AzureCaptionModel azureCaptionModel in imageDescribe.Captions)
                {
                    AzureTranslateRequest azureTranslateRequest = new AzureTranslateRequest()
                    {
                        Text = azureCaptionModel.Text
                    };

                    azureTranslateRequestList.Add(azureTranslateRequest);
                }

                var content = new StringContent(JsonConvert.SerializeObject(azureTranslateRequestList), Encoding.UTF8, "application/json");
       
                // Asynchronously call the REST API method.
                response = await client.PostAsync(urlTranslate, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Asynchronously get the JSON response.
                    string contentString = await response.Content.ReadAsStringAsync();
                    imageTranslateResponse = JsonConvert.DeserializeObject<List<AzureTranslateResponse>>(contentString);

                    if (imageTranslateResponse != null)
                    {
                        int imageToTranslate = 0;
                        foreach (AzureTranslateResponse azureTranslateResponse in imageTranslateResponse)
                        {
                            foreach (AzureTranslateTranslation azureTranslateTranslation in azureTranslateResponse.translations)
                            {
                                if (imageToTranslate < imageDescribe.Captions.Count)
                                {
                                    imageDescribe.Captions[imageToTranslate].Text = azureTranslateTranslation.text;
                                    imageToTranslate++;
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.Error($"AzureBLogic ERROR - CallAzureTranslate Action response OK but NOT mapped object result");
                    }
                }
                else
                {
                    Logger.Error($"AzureBLogic ERROR - CallAzureTranslate Action response: '{response.RequestMessage}'");
                }
            }
            else
            {
                Logger.Error($"AzureBLogic ERROR - CallAzureTranslate Action not received imageDescribe object");
            }
          
            return imageDescribe;
        }
    }
}
