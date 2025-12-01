using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Chess_DB.Views;

public class PlayersPage : UserControl
{
    public PlayersPage()
    {
        Content = new TextBlock
        {
            Text = "Page Joueurs",
            FontSize = 28,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brushes.Black
        };
    }
}