using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public ushort Year { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Make: {Make} Model: {Model} Year: {Year} VIN: {VIN} Color: {Color}";
        }


    }
}
