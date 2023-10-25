using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.DAL
{
    internal class Orderitems
    {
        private int id;
        private int OrderId;
        private int partId;
        private string batchNumber;
        private int amount;

        public int Id { get => id; set => id = value; }
        public int OrderId1 { get => OrderId; set => OrderId = value; }
        public int PartId { get => partId; set => partId = value; }
        public string BatchNumber { get => batchNumber; set => batchNumber = value; }
        public int Amount { get => amount; set => amount = value; }
    }
}
