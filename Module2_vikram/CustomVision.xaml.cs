using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Module2_vikram.DataModel;
using Module2_vikram.Model;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Module2_vikram
{
    public partial class CustomVision : ContentPage
    {
        public CustomVision()
        {
            InitializeComponent();
        }

        private async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {

                return file.GetStream();
            });
            await postLocationAsync();

			async Task postLocationAsync()
			{

				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;

				var position = await locator.GetPositionAsync();

				NotHeroModel model = new NotHeroModel()
				{
					Longitude = (float)position.Longitude,
					Latitude = (float)position.Latitude

				};

				await AzureManager.AzureManagerInstance.PostHeroInformation(model);
			}
            await MakePredictionRequest(file);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "9edb926f637d4217a56323b6538e14a4");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/29e9309a-cc47-4123-b301-edaa3047ff89/image?iterationId=c8d11c4e-4192-4cef-8291-127ad9a74fcc";

            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability);

                    TagLabel.Text = (max >= 0.2) ? "SuperHero" : "Not SuperHero";

                }

                //Get rid of file once we have finished using it
                file.Dispose();
            }
        }
    }
}
