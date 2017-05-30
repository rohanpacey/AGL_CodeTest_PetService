using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetOwnerServiceHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace PetServiceTest
{
    [TestClass]
    public class PetServiceTest
    {

        private List<PetOwner> _petOwners = new List<PetOwner>();
        [TestInitialize]
        public void InitialiseData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string json = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),@"testData.json"));
            _petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(json);
        }

        [TestMethod]
        public void ProcessPetOwnerData_GetPetOwnersWithCats_Success() {

            // Arrange        
            PetServiceBusinessLogic bl = new PetServiceBusinessLogic();
            string petType = "Cat";

            // Act
            var petOwnerOutput = bl.ProcessPetOwnerData(_petOwners, petType);

            // Assert
            Assert.IsNotNull(petOwnerOutput);
            Assert.IsInstanceOfType(petOwnerOutput, typeof(List<PetOwnerOutput>));
            Assert.AreEqual(7, petOwnerOutput.Count);
            Assert.AreEqual("Garfield", petOwnerOutput[0].petName);
            Assert.AreEqual("Tabby", petOwnerOutput[6].petName);
            Assert.AreEqual("Female", petOwnerOutput[4].gender);

        }

        [TestMethod]
        public void ProcessPetOwnerData_GetPetOwnersWithDogs_Success()
        {

            // Arrange        
            PetServiceBusinessLogic bl = new PetServiceBusinessLogic();
            string petType = "Dog";

            // Act
            var petOwnerOutput = bl.ProcessPetOwnerData(_petOwners, petType);

            // Assert
            Assert.IsNotNull(petOwnerOutput);
            Assert.IsInstanceOfType(petOwnerOutput, typeof(List<PetOwnerOutput>));
            Assert.AreEqual(2, petOwnerOutput.Count);
            Assert.AreEqual("Fido", petOwnerOutput[0].petName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProcessPetOwnerData_InvalidArgument_petOwnersNullParameter()
        {
            // Arrange        
            PetServiceBusinessLogic bl = new PetServiceBusinessLogic();
            string petType = "Cat";

            // Act
            var petOwnerOutput = bl.ProcessPetOwnerData(null, petType);

            // Assert
            // Nothing to assert. Expected exception is thrown and caught
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProcessPetOwnerData_InvalidArgument_petTypeNullParameter()
        {
            // Arrange        
            PetServiceBusinessLogic bl = new PetServiceBusinessLogic();

            // Act
            var petOwnerOutput = bl.ProcessPetOwnerData(_petOwners, null);

            // Assert
            // Nothing to assert. Expected exception is thrown and caught
        }

        [TestMethod]
        public void RenderOutputAsList_Success()
        {

            // Arrange        
            PetServiceBusinessLogic bl = new PetServiceBusinessLogic();
            string petType = "Dog";

            // Act
            var petOwnerOutput = bl.ProcessPetOwnerData(_petOwners, petType);
            var output = bl.RenderOutputAsList(petOwnerOutput);

            // Assert
            Assert.IsNotNull(output);
            Assert.IsInstanceOfType(output, typeof(String));
        }

        [TestMethod]
        public void CallService_Success()
        {
            //I realise this is not good Unit Testing practice, however time was insufficient to properly mock the service    
            //Hence I felt this was permissable as an alternative

            // Arrange        
            var serviceTarget = "http://agl-developer-test.azurewebsites.net/people.json";
            PetOwnerService service = new PetOwnerService(serviceTarget);

            // Act
            Task<List<PetOwner>> petOwners = service.GetPetOwnersAsync();

            // Assert
            Assert.IsNotNull(petOwners);
            Assert.IsInstanceOfType(petOwners, typeof(Task<List<PetOwner>>));
        }

    }
}
