using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

// Alias pour éviter les conflits et simplifier le code
using Thickness = Avalonia.Thickness;
using CornerRadius = Avalonia.CornerRadius;

namespace Chess_DB.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // --- GRID PRINCIPALE ---
            var layout = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*"),
                ColumnDefinitions = new ColumnDefinitions("200,*")
            };

            // --- TITRE EN HAUT ---
            var titleBorder = new Border
            {
                Background = new SolidColorBrush(Color.Parse("#2C3E50")),
                Padding = new Thickness(20),

                Child = new TextBlock
                {
                    Text = "Chess contest Manager",
                    Foreground = Brushes.White,
                    FontSize = 32,
                    FontWeight = FontWeight.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            };

            Grid.SetColumnSpan(titleBorder, 2);
            layout.Children.Add(titleBorder);

            // --- BARRE LATERALE ---
            var sidebar = new StackPanel
            {
                Background = new SolidColorBrush(Color.Parse("#34495E")),
                Spacing = 10,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // Ajout des boutons du menu
            sidebar.Children.Add(CreateMenuButton("Compétitions"));
            sidebar.Children.Add(CreateMenuButton("Players"));
            sidebar.Children.Add(CreateMenuButton("Inscriptions"));
            sidebar.Children.Add(CreateMenuButton("Parties"));
            sidebar.Children.Add(CreateMenuButton("Classements ELO"));

            // Permet de remplir l'espace vertical restant
            sidebar.Children.Add(new StackPanel { VerticalAlignment = VerticalAlignment.Stretch });

            Grid.SetRow(sidebar, 1);
            layout.Children.Add(sidebar);

            // --- ZONE CENTRALE ---
            var contentArea = new Border
            {
                Background = new SolidColorBrush(Color.Parse("#ECF0F1")),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(15),

                Child = new TextBlock
                {
                    Text = "Sélectionnez une action dans le menu",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 20,
                    Foreground = new SolidColorBrush(Color.Parse("#7F8C8D"))
                }
            };

            Grid.SetRow(contentArea, 1);
            Grid.SetColumn(contentArea, 1);
            layout.Children.Add(contentArea);

            // --- On définit le layout comme contenu de la vue ---
            Content = layout;
        }

        // Cette méthode ne doit JAMAIS lancer d'exception
        private void InitializeComponent()
        {
            // Aucune interface XAML à charger
        }

        private Button CreateMenuButton(string text)
        {
            var btn = new Button
            {
                Content = text,
                Height = 50,
                HorizontalContentAlignment = HorizontalAlignment.Left
            };
            btn.Click += (_, _) => Navigate(text);

            return btn;
        }
        
        private Border _contentArea;
        
        private void Navigate(string pageName)
        {
            UserControl page = pageName switch
            {
                "Players"         => new PlayersPage(),
                _ => new UserControl
                {
                    Content = new TextBlock
                    {
                        Text = "Page inconnue",
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                    }
                }
            };

            _contentArea.Child = page;
        }
    }
}

