using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.GemTech.Model.Poco
{
    public class GemTechEncryptData
    {
        public string Iv { get; set; }
        public string Value { get; set; }
        public string Mac { get; set; }
    }
}
