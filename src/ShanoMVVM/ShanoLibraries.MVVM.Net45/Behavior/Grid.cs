using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShanoLibraries.MVVM.Behavior
{
    public static class Grid
    {
        public static void SetRows(System.Windows.Controls.Grid grid, string rows)
        {
            grid.RowDefinitions.Clear();
            foreach (GridLength length in GetLengths(rows))
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = length });
            }
        }

        public static void SetColumns(System.Windows.Controls.Grid grid, string columns)
        {
            grid.ColumnDefinitions.Clear();
            foreach (GridLength length in GetLengths(columns))
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = length });
            }
        }

        static IEnumerable<GridLength> GetLengths(string lengths) =>
            GetLengths(SplitLengths(lengths));

        private static IEnumerable<string> SplitLengths(string lengths) =>
            lengths
            .Split(',')
            .Select(x => x.Trim().ToLower())
            .Where(x => !string.IsNullOrEmpty(x));

        static IEnumerable<GridLength> GetLengths(IEnumerable<string> lengthStrings)
        {
            foreach (string lengthString in lengthStrings)
            {
                yield return
                    lengthString[0] == '*' ? new GridLength(1, GridUnitType.Star) :
                    lengthString.Contains("auto") ? new GridLength(1, GridUnitType.Auto) :
                    new GridLength(double.Parse(lengthString), GridUnitType.Pixel);
            }
        }
    }
}
