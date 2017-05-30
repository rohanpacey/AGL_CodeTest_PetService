using PetOwnerServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApp1
{
    class Program
    {
        private const string petType = "Cat";

        static void Main()
        {
            Console.Write("Calling service...\n\n");
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {

            try
            {

                //get the service target Url
                var serviceTarget = ConfigurationManager.AppSettings["TargetServiceUrl"];
                //create service class and call the service
                PetOwnerService service = new PetOwnerService(serviceTarget);
                List<PetOwner> petOwners = await service.GetPetOwnersAsync();
                
                if (petOwners != null)
                {
                    //call the BL class with the service data for manipulation according to business rules
                    PetServiceBusinessLogic bl = new PetServiceBusinessLogic();
                    List<PetOwnerOutput> petOwnerOutput = bl.ProcessPetOwnerData(petOwners, petType);
                    //send the results to be converted to a screen-friendly list and write to the console 
                    Console.Write(bl.RenderOutputAsList(petOwnerOutput));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Exception : {0}", e.Message));
            }

            Console.ReadKey();

        }
    }
}
