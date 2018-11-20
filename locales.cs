using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace locales {
    class Program {
        /// <summary>
        /// These are "default" and "custom" locales that don't have BCP-47 equivalent
        /// </summary>
        private static HashSet<int> exclusions = new HashSet<int> { 0x007F, 0x1000 };

        /// <summary>
        /// Look up all the known cultures in the way we want them
        /// </summary>
        private static IEnumerable<CultureInfo> listCultures() {
            return from c in CultureInfo.GetCultures(CultureTypes.AllCultures)
                   where !exclusions.Contains(c.LCID)
                   orderby c.LCID
                   select c;
        }

        /// <summary>
        /// Print all the cultures to the console
        /// </summary>
        private static void printCultures() {
            var cultures = listCultures();
            Console.WriteLine($"\n/* Found {cultures.Count()} .NET cultures */\n");
            foreach (var culture in cultures) {
                Console.WriteLine($"0x{culture.LCID:X4} -> \"{culture.IetfLanguageTag}\"");
            }
        }

        private static bool shouldPrintCultureMap = false;
        private static void printCultureMap() {
            var cultureArrows =
                listCultures().Select(c => $"0x{c.LCID:X4} -> \"{c.IetfLanguageTag}\"");
            Console.WriteLine($"\nMap({String.Join(", ", cultureArrows)})");
        }

        private static bool shouldPrintDuplicates = false;
        /// <summary>
        /// Print information about any duplicate LCIDs if found
        /// </summary>
        private static void printDuplicates() {
            var grouped =
                from c in listCultures()
                group c by c.LCID into cg
                where cg.Count() > 1
                orderby cg.Key
                select cg;
            if (grouped.Count() > 0) {
                Console.WriteLine($"\n/* Found {grouped.Count()} LCIDs corresponding to more than one culture\n");
                foreach (var cg in grouped) {
                    var names = cg.Select(c => $"\"{c.IetfLanguageTag}\"");
                    Console.WriteLine($"{cg.Key} -> List({String.Join(", ", names)})");
                }
            }
        }

        static void Main(string[] args) {
            printCultures();
            if (shouldPrintCultureMap) {
                printCultureMap();
            }
            if (shouldPrintDuplicates) {
                printDuplicates();
            }
        }
    }
}
