using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo.DAL
{
    internal class Part
    {
        private string name;
        private string batch;
        private int amounds;
        private int id;

        public string Name { get => name; set => name = value; }
        public string Batch { get => batch; set => batch = value; }
        public int Amounds { get => amounds; set => amounds = value; }
        public int Id { get => id; set => id = value; }
    }
}
