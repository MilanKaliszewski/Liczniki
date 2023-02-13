using counters.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;


namespace counters.Controls;


public partial class CounterView : ContentView
{
    int counter = 0;
    public CounterView()
    {
        InitializeComponent();
        this.counter = 0;

        UISettings uiSettings = new UISettings();
        bool isDarkModeEnabled = uiSettings.GetColorValue(UIColorType.Background).R == 0;

        if (isDarkModeEnabled)
        {
            this.rameczkaStyl.BackgroundColor = Colors.Black;
        }
        else
        {
            this.rameczkaStyl.BackgroundColor= Colors.White;
        }


    }
    public CounterView(string id)
    {
        InitializeComponent();
        this.id.Text = id;
        this.counter = 0;

        UISettings uiSettings = new UISettings();
        bool isDarkModeEnabled = uiSettings.GetColorValue(UIColorType.Background).R == 0;

        if (isDarkModeEnabled)
        {
            this.rameczkaStyl.BackgroundColor = Colors.Black;
        }
        else
        {
            this.rameczkaStyl.BackgroundColor = Colors.White;
        }
    }

    public CounterView(string name, string count, string id, string color)
    {
        InitializeComponent();
        this.counterBox.Placeholder = name.Replace("-", " ");
        this.value.Text = count;
        this.rameczkaStyl.BackgroundColor = Color.FromHex(color);
        this.id.Text = id;
        this.counter = int.Parse(count);

        UISettings uiSettings = new UISettings();
        bool isDarkModeEnabled = uiSettings.GetColorValue(UIColorType.Background).R == 0;

        if (isDarkModeEnabled)
        {
            this.rameczkaStyl.BackgroundColor = Colors.Black;
        }
        else
        {
            this.rameczkaStyl.BackgroundColor = Colors.White;
        }

    }

    private void minus_Clicked(object sender, EventArgs e)
    {
        this.counter--;
        this.value.Text = counter.ToString();
    }

    private void plus_Clicked(object sender, EventArgs e)
    {
        this.counter++;
        this.value.Text = counter.ToString();
    }

    private void saveCounter_Clicked(object sender, EventArgs e)
    {
        string targetFileName = "countersSaves.txt";
        string fileName = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);

        if (!File.Exists(fileName))
        {
            File.Create(fileName).Dispose();
        }

        var lines = File.ReadAllLines(fileName);
        var id = this.id.Text;
        var exists = false;

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(id + " "))
            {
                exists = true;
                lines[i] = id + " " + this.counterBox.Text.Replace(" ", "-") + " " + this.value.Text + " " + this.rameczkaStyl.BackgroundColor.ToHex().ToString();
                break;
            }
        }

        if (!exists)
        {
            File.AppendAllText(fileName, id + " " + this.counterBox.Text.Replace(" ", "-") + " " + this.value.Text + " " + this.rameczkaStyl.BackgroundColor.ToHex().ToString() + Environment.NewLine);
        }
        else
        {
            File.WriteAllLines(fileName, lines);
        }
    }

    private async void editCounter_Clicked(object sender, EventArgs e)
    {
        var response = await App.Current.MainPage.DisplayActionSheet("Wybierz kolor", "Anuluj", null, "Czerwony", "Niebieski", "Zielony", "Ró¿owy", "Pomarañczowy", "Czarny", "Bia³y");
        if (response != null)
        {
            switch (response)
            {
                case ("Czerwony"):
                    this.rameczkaStyl.BackgroundColor = Colors.Red;
                    break;
                case ("Niebieski"):
                    this.rameczkaStyl.BackgroundColor = Colors.Blue;
                    break;
                case ("Zielony"):
                    this.rameczkaStyl.BackgroundColor = Colors.Green;
                    break;
                case ("Ró¿owy"):
                    this.rameczkaStyl.BackgroundColor = Colors.Pink;
                    break;
                case ("Pomarañczowy"):
                    this.rameczkaStyl.BackgroundColor = Colors.Orange;
                    break;
                case ("Czarny"):
                    this.rameczkaStyl.BackgroundColor = Colors.Black;
                    break;
                case ("Bia³y"):
                    this.rameczkaStyl.BackgroundColor = Colors.White;
                    break;
            }
        }
    }

    private async void deleteCounter_Clicked(object sender, EventArgs e) {
        bool jedna = await App.Current.MainPage.DisplayAlert("Usuwanie", "Czy chcesz usun¹æ tylko jeden counter, czy wszystkie z ekranu?", "Wszystkie", "Jeden");
        if (jedna)
        {
            if(this.Parent is FlexLayout flexLayout)
            {
                flexLayout.Children.Clear();
            }
        }
        else
        {
            if(this.Parent is FlexLayout flexLayout)
            {
                flexLayout.Children.Remove(this);
            }
        }

    }
}