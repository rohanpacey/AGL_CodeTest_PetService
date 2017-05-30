using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetOwnerServiceHelper
{
    public class PetServiceBusinessLogic
    {

        /// <summary>
        /// Process and flatten the service data according to the business rules
        /// </summary>
        /// <param name="petOwners">Deserialized data from the service</param>
        /// <param name="petType">The type of pet to return data for</param>
        /// <returns>List of type PetOwnerOutput</returns>
        public List<PetOwnerOutput> ProcessPetOwnerData(List<PetOwner> petOwners, string petType)
        {
            //check vars
            if (petOwners == null)
            {
                throw new ArgumentException("Argument cannot be null", "petOwners");
            }
            if (petType == null)
            {
                throw new ArgumentException("Argument cannot be null", "petType");
            }

            //prepare output var
            List<PetOwnerOutput> petOwnerOutput = new List<PetOwnerOutput>();

            //get gender groupings and loop
            IEnumerable<IGrouping<string, PetOwner>> genderGrouping = petOwners.OrderByDescending(PetOwner => PetOwner.gender)
                                                                               .GroupBy(PetOwner => PetOwner.gender);
            foreach (IGrouping<string, PetOwner> petGroup in genderGrouping)
            {
                //flatten the data, remove items where there are no pet objects, filter to our pet type and order by the pet name
                IEnumerable<Pet> petsList = petGroup.Where(petOwner => petOwner.Pets != null)
                                                    .SelectMany(petOwner => petOwner.Pets)
                                                    .Where(pet => pet.type.ToLower() == petType.ToLower())
                                                    .OrderBy(pet => pet.name);

                //write values to our output object
                foreach (Pet pet in petsList)
                {
                    petOwnerOutput.Add(new PetOwnerOutput { gender = petGroup.Key, petName = pet.name });
                }
            }
            return petOwnerOutput;
        }

        /// <summary>
        /// Construct a list for output from list of type PetOwnerOutput
        /// </summary>
        /// <param name="petOwnerOutput">List of type petOwnerOutput</param>
        /// <returns>String for screen display</returns>
        public string RenderOutputAsList(List<PetOwnerOutput> petOwnerOutput)
        {
            string output = "";
            var gender = "";
            foreach (PetOwnerOutput item in petOwnerOutput)
            {
                if (gender != item.gender || String.IsNullOrEmpty(gender))
                {
                    output += String.Format("{0}\n", item.gender);
                    gender = item.gender;
                }
                output += String.Format("\t{0}\n", item.petName);
            }
            return output;
        }
    }
}
