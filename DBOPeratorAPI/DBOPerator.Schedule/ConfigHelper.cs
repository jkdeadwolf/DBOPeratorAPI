using System;
using System.Collections.Generic;
using System.Text;

namespace DBOPerator.Schedule
{
    class ConfigHelper
    {
        public static string Random = Get();

        public static string Random2 => Get();

        private static string Get()
        {
            return new Random().Next(6000).ToString();
        }
    }
}
