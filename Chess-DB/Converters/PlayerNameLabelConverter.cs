using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using ChessDB.Model;

namespace Chess_DB.Converters
{
    public class PlayerNameLabelConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values is null || values.Count < 2)
            {
                return string.Empty;
            }

            var role = parameter as string ?? string.Empty;
            var id = values[0] as Guid? ?? Guid.Empty;
            var players = values[1] as IEnumerable;

            string name = "Unassigned";
            if (id != Guid.Empty && players is not null)
            {
                foreach (var p in players)
                {
                    if (p is Player player && player.Id == id)
                    {
                        name = $"{player.LastName} {player.FirstName}".Trim();
                        break;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                return name;
            }

            return $"{role}: {name}";
        }

        public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            return Array.Empty<object?>();
        }
    }
}
