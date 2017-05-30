using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PetOwnerServiceHelper
{
    public class PetOwnerService
    {

        private string _serviceTarget;
        public PetOwnerService(string serviceTarget)
        {
            _serviceTarget = serviceTarget;
        }

        public async Task<List<PetOwner>> GetPetOwnersAsync()
        {
            using (var client = new HttpClient())
            {
                List<PetOwner> petOwners = null;
                HttpResponseMessage response = await client.GetAsync(_serviceTarget);
                if (response.IsSuccessStatusCode)
                {
                    petOwners = await response.Content.ReadAsAsync<List<PetOwner>>();
                }
                return petOwners;
            }
        }
    }
}
