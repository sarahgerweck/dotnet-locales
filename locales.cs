using System;
using System.Globalization;
using System.Linq;

namespace locales {
    class Program {
        static void Main(string[] args) {
            var filtered =
                from c in CultureInfo.GetCultures(CultureTypes.AllCultures)
                where c.LCID != 0x1000 && c.LCID != 0x007F
                orderby c.LCID
                select c;

            Console.WriteLine($"/* Found {filtered.Count()} .NET cultures */\n");
            foreach (var culture in filtered) {
                var cid = culture.LCID;
                var ct = culture.IetfLanguageTag;
                Console.WriteLine($"0x{cid:X4} -> \"{ct}\"");
            }
        }
    }
}
