using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Models
{
    internal class QuizItem
    {
        public int val { get; set; }
        public string description { get; }
        public string name { get; }


        public QuizItem(string Name, string Desciption)
        {
            val = 1;
            name = Name;
            description = Desciption;
        }
    }
}
