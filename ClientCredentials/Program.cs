using System;

namespace ClientCredentials
{
    class Program
    {
        static void Main(string[] args)
       => IDSHelper.Admin().GetAwaiter().GetResult();
    }
}
