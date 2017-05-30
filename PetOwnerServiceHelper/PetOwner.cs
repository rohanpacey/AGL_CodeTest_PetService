using System;
using System.Collections.Generic;
using System.Text;

namespace PetOwnerServiceHelper
{

    public class Pet
    {
        public string name { get; set; }
        public string type { get; set; }
    }

    public class PetOwner
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public List<Pet> Pets { get; set; }
    }
}
