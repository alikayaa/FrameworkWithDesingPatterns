using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAI.Configuration
{
    internal interface IConfigReader
    {
        string ReadKey(string name);
        string ReadConncetionString(string name);
    }
}
