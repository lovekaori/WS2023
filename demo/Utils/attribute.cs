using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.Utils
{
    class attribute
    {
        private string suppliers;

        private string warehouse;

        private string date;

        private string part_Name;

        private string batch_Number;

        private string amount;

        private string source_warehouse;

        private string destination_warehouse;

        public string Suppliers { get => suppliers; set => suppliers = value; }
        public string Warehouse { get => warehouse; set => warehouse = value; }
        public string Date { get => date; set => date = value; }
        public string Part_Name { get => part_Name; set => part_Name = value; }
        public string Batch_Number { get => batch_Number; set => batch_Number = value; }
        public string Amount { get => amount; set => amount = value; }
        public string Source_Warehouse { get => source_warehouse; set => source_warehouse = value; }
        public string Destination_warehouse { get => destination_warehouse; set => destination_warehouse = value; }
    }
}
