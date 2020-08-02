using GetDescriptionImageApp.BusinessLogic;
using GetDescriptionImageApp.Models;
using GetDescriptionImageApp.Models.Azure;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Image = System.Drawing.Image;

namespace GetDescriptionImageApp.Presentation.Controls
{
    /// <summary>
    /// Lógica de interacción para ImagesControl.xaml
    /// </summary>
    public partial class ImagesControl : UserControl
    {
        readonly IAzureBLogic azureBLogic;
        private readonly Logger Logger;

        private const string pngExtension = "png";
        private const string jpgExtension = "jpg";
        private List<ImageModel> ImagesList = new List<ImageModel>();

        public ImagesControl()
        {
            Logger = LogManager.GetCurrentClassLogger();
            azureBLogic = new AzureBLogic();
            InitializeComponent();
        }

        #region Images Control
        private void UploadImagesClick(object sender, RoutedEventArgs e)
        {
            Spinner.Visibility = Visibility.Visible;

            Logger.Info("MainWindow START - UploadImagesClick Action");

            try
            {
                OpenFileDialog openImageDialog = new OpenFileDialog()
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png",
                    Multiselect = true,
                };

                if (openImageDialog.ShowDialog() == true)
                {
                    ImagesList = new List<ImageModel>();

                    foreach (string currenFileName in openImageDialog?.FileNames)
                    {
                        ImageModel currentImage = new ImageModel()
                        {
                            Name = currenFileName,
                            ImageData = new BitmapImage(new Uri(currenFileName)),
                            IsSelected = false
                        };

                        using Stream fileStream = new FileStream(currenFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                        BitmapDecoder decoder = new JpegBitmapDecoder(fileStream, BitmapCreateOptions.None, BitmapCacheOption.None);
                        var frame = decoder?.Frames[0];
                        BitmapMetadata metadata = (BitmapMetadata)frame.Metadata;

                        currentImage.Alt = string.IsNullOrEmpty(metadata?.Comment) ? "La imagen no tiene descripción." : metadata.Comment;

                        ImagesList.Add(currentImage);
                    }

                    ImageListBox.ItemsSource = ImagesList;

                    if (ImagesList.Count > 0)
                    {
                        LabelImageSelection.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "MainWindow ERROR - UploadImagesClick Action");
            }
            finally
            {
                Logger.Info("MainWindow FINISH - UploadImagesClick Action");
                Spinner.Visibility = Visibility.Hidden;
            }
        }

        private void ModifiedImage(object sender, RoutedEventArgs e)
        {
            Spinner.Visibility = Visibility.Visible;

            Logger.Info("MainWindow START - ModifiedImage Action");

            bool resultGetDescriptionOperation = false;
            string oldPNGPathName = null;
            string newPathName = null;
            List<ImageModel> selectedImages = ImagesList.Where(x => x.IsSelected).ToList();
            List<ImageModel> noSelectedImages = ImagesList.Where(x => !x.IsSelected).ToList();

            try
            {
                foreach (ImageModel currentImage in selectedImages)
                {
                    bool isPNG = currentImage.Name.EndsWith(pngExtension, StringComparison.InvariantCultureIgnoreCase);

                    if (isPNG)
                    {
                        oldPNGPathName = currentImage.Name;
                        newPathName = ReplacePNGImageToJPG(oldPNGPathName);
                        currentImage.Name = newPathName;
                        resultGetDescriptionOperation = GetAndSetDescriptionToImage(currentImage.Name, currentImage);
                        currentImage.Name = oldPNGPathName;
                        File.Delete(newPathName);
                    }
                    else
                    {
                        resultGetDescriptionOperation = GetAndSetDescriptionToImage(currentImage.Name, currentImage);
                    }
                }

                ImagesList = selectedImages.Concat(noSelectedImages).ToList();
                ImageListBox.ItemsSource = ImagesList;

                if (resultGetDescriptionOperation)
                {
                    MessageBox.Show("Obtenidas las descripciones correctamente.", "Operación Correcta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "MainWindow ERROR - ModifiedImage Action");
                MessageBox.Show(exc.Message, "Error Obteniendo las descripciones.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Logger.Info("MainWindow FINISH - ModifiedImage Action");
                Spinner.Visibility = Visibility.Hidden;
            }
        }

        private bool GetAndSetDescriptionToImage(string currentImagePath, ImageModel currentImage)
        {
            Logger.Info($"MainWindow START - GetAndSetDescriptionToImage Action to path: '{currentImagePath}'");

            BitmapDecoder decoder;
            BitmapEncoder encoder;
            bool resultGetOperation = false;

            try
            {
                using Stream fileStream = new FileStream(currentImagePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                decoder = new JpegBitmapDecoder(fileStream, BitmapCreateOptions.None, BitmapCacheOption.None);
                var frame = decoder.Frames[0];

                frame = BitmapFrame.Create(frame);
                BitmapMetadata metadata = (BitmapMetadata)frame.Metadata;

                byte[] imageBytes = File.ReadAllBytes(currentImagePath);
                string base64ImageString = Convert.ToBase64String(imageBytes);

                Logger.Info($"MainWindow START - GetAndSetDescriptionToImage Action Get Azure Image Info from bytes[] of stringbase64: '{base64ImageString}'");

                AzureDescriptionModel azureDescribeResponseModel = azureBLogic.GetAzureDescriptionImage(imageBytes, base64ImageString);

                if (azureDescribeResponseModel != null
                    && string.IsNullOrEmpty(azureDescribeResponseModel.ErrorMessage)
                    && azureDescribeResponseModel.Captions != null
                    && azureDescribeResponseModel.Captions.Count > 0)
                {
                    Logger.Info($"MainWindow START - GetAndSetDescriptionToImage Action get '{azureDescribeResponseModel.Captions.Count}' Description from Azure");

                    foreach (var caption in azureDescribeResponseModel.Captions)
                    {
                        Logger.Info($"MainWindow START - GetAndSetDescriptionToImage Action Image Caption {caption.Text} with Confidence {caption.Confidence}");
                    }

                    List<string> authors = new List<string>()
                    {
                        "Juan Manuel"
                    };

                    ReadOnlyCollection<string> authorsCollection = new ReadOnlyCollection<string>(authors);

                    metadata.Author = authorsCollection;
                    metadata.Title = azureDescribeResponseModel.Captions[0].Text;
                    metadata.ApplicationName = "GetDescriptionImageApp";
                    metadata.Comment = azureDescribeResponseModel.Captions[0].Text;

                    currentImage.Alt = metadata.Comment;
                    currentImage.Autor = metadata.Author.ToString();
                    currentImage.Description = metadata.Comment;

                    encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(frame);

                    string newImageUri = ReplaceUri(currentImagePath);

                    using var targetStream = new FileStream(newImageUri, FileMode.Create);
                    encoder.Save(targetStream);

                    resultGetOperation = true;
                }
                else if (azureDescribeResponseModel != null && !string.IsNullOrEmpty(azureDescribeResponseModel.ErrorMessage))
                {
                    resultGetOperation = false;
                    MessageBox.Show(azureDescribeResponseModel.ErrorMessage, "Error procesando imagenes", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (azureDescribeResponseModel == null)
                {
                    resultGetOperation = false;
                    MessageBox.Show($"No se ha podido obtener información de Azure de la imaten '{currentImagePath}'", "Error procesando imagenes", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    resultGetOperation = false;
                    Logger.Info($"MainWindow START - GetAndSetDescriptionToImage Action NOT get Description from Azure");
                }
            }
            catch (Exception exc)
            {
                resultGetOperation = false;
                Logger.Error(exc, "MainWindow ERROR - GetAndSetDescriptionToImage Action");
                MessageBox.Show("Error Generico al intentar obtener la descripción de la imagen/es.", "Error procesando imagenes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Logger.Info("MainWindow FINISH - GetAndSetDescriptionToImage Action");
            }

            return resultGetOperation;
        }

        private string ReplacePNGImageToJPG(string pngPath)
        {
            Logger.Info($"MainWindow START - ReplacePNGImageToJPG Action to path: '{pngPath}'");

            Image imagePNGToConvert;
            string newPathName = null;

            try
            {
                newPathName = pngPath.Replace(pngExtension, jpgExtension);
                imagePNGToConvert = Image.FromFile(pngPath);

                imagePNGToConvert.Save(newPathName, ImageFormat.Jpeg);
                imagePNGToConvert.Dispose();
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "MainWindow ERROR - ReplacePNGImageToJPG Action");
            }
            finally
            {
                Logger.Info($"MainWindow FINISH - ReplacePNGImageToJPG from path: '{pngPath}' to newPathName: '{newPathName}'");
            }

            return newPathName;
        }

        private string ReplaceUri(string originalUri)
        {
            Logger.Info($"MainWindow START - ReplaceUri Action from originalUri: '{originalUri}'");

            string newFileName = "";
            DateTime dateTimeNow = DateTime.Now;
            string dateNowString = dateTimeNow.ToString("ddMMyyyy_HHMMss");

            try
            {
                if (originalUri.Contains(".jpg"))
                {
                    newFileName = originalUri.Replace(".jpg", "_modified_" + dateNowString + ".jpg");
                }
                else if (originalUri.Contains(".JPG"))
                {
                    newFileName = originalUri.Replace(".JPG", "_modified_" + dateNowString + ".jpg");
                }
                else if (originalUri.Contains(".jpeg"))
                {
                    newFileName = originalUri.Replace(".jpeg", "_modified_" + dateNowString + ".jpg");
                }
                else if (originalUri.Contains(".JPEG"))
                {
                    newFileName = originalUri.Replace(".JPEG", "_modified_" + dateNowString + ".jpg");
                }
                else if (originalUri.Contains(".png"))
                {
                    newFileName = originalUri.Replace(".png", "_modified_" + dateNowString + ".png");
                }
                else if (originalUri.Contains(".PNG"))
                {
                    newFileName = originalUri.Replace(".PNG", "_modified_" + dateNowString + ".png");
                }
            }
            catch (Exception exc)
            {
                Logger.Error(exc, "MainWindow ERROR - ReplaceUri Action");
            }
            finally
            {
                Logger.Info($"MainWindow FINISH - ReplaceUri from originalUri: '{originalUri}' to newFileName: '{newFileName}'");
            }

            return newFileName;
        }
        #endregion Images Control
    }
}
