using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCon
{
    class Program
    {
        public byte B { get; set; }
        static void Main(string[] args)
        {
            int i = 1327671;

            new Program().C();
            Console.WriteLine(new Program().B);   
           
        }

        public void C()
        {
            var c = this.GetType().ToString();
            Console.WriteLine(this.GetType().ToString());
        }
    }
}
