using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Module2_vikram.DataModel;

namespace Module2_vikram
{
    public class AzureManager
    {
        private IMobileServiceTable<NotHeroModel> notHeroTable;
        private static AzureManager instance;
        private MobileServiceClient client;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://module2vikram.azurewebsites.net/");
            this.notHeroTable = this.client.GetTable<NotHeroModel>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }


            public async Task<List<NotHeroModel>> GetHeroInformation()
        {
            return await this.notHeroTable.ToListAsync();
        }

		public async Task PostHeroInformation(NotHeroModel notHeroModel)
		{
			await this.notHeroTable.InsertAsync(notHeroModel);
		}
		public async Task UpdateHeroInformation(NotHeroModel notHeroModel)
		{
			await this.notHeroTable.UpdateAsync(notHeroModel);
		}
		public async Task DeleteHeroInformation(NotHeroModel notHeroModel)
		{
			await this.notHeroTable.DeleteAsync(notHeroModel);
		}
    }
}


