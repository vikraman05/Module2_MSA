using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Module2_vikram.DataModel;
using Xamarin.Forms;

namespace Module2_vikram
{
    public partial class AzureTable : ContentPage
    {
        MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public AzureTable()
        {
            InitializeComponent();

        }

		async void Handle_ClickedAsync(object sender, System.EventArgs e)
		{
			List<NotHeroModel> notHeroInformation = await AzureManager.AzureManagerInstance.GetHeroInformation();

			HeroList.ItemsSource = notHeroInformation;
		}
    }
}
