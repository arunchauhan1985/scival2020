﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseGenerator generator = new DatabaseGenerator();
            generator.StartGeneration();
        }
    }
}
