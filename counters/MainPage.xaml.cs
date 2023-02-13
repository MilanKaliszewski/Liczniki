using counters.Controls;
using Microsoft.Maui.Controls.Internals;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace counters;

public partial class MainPage : ContentPage
{
	int count = 0;
	public MainPage()
	{
		InitializeComponent();
    }


    //Dodanie nowego elementu layoutFlex
    private async void Add_Clicked(object sender, EventArgs e)
    {
        if (count < 20)
        {
            this.layoutFlex.Add(new Controls.CounterView(GetNewId()));
            count++;
        }
        else
        {
            await DisplayAlert("Przekroczono limit!", "Przekroczyłeś limit dostępnych w aplikacji liczników. Rozważ usunięcie któregoś!", "OK");
        }

    }

    //Usunięcie wszystkich elementów layoutFlex
    private async void RemoveAll_Clicked(object sender, EventArgs e)
    {
        bool del = await DisplayAlert("Usuń wszystko", "Czy chcesz usunąć wszystkie liczniki? Nie będzie można ich odzyskać. Zostaną usuniętę również z dysku (z zapisu lokalnego)", "Usuń", "Anuluj");
        if (del)
        {
            this.layoutFlex.Children.Clear();
            this.count = 0;
            string targetFileName = "countersSaves.txt";
            string fileName = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                streamWriter.Write("");
            }
        }
    }

    private async void RemoveOnlyDisplayed_Clicked(object sender, EventArgs e)
    {
        bool del = await DisplayAlert("Usuń wyświetlane", "Czy chcesz usunąć wszystkie liczniki z ekranu? Zapis lokalny pozostanie bez zmian. Będzie możliwe wczytanie liczników wcześniej zapisanych. Rozważ zapisanie ważnych liczników na dysku!", "Usuń", "Anuluj");
        if(del)
        {
            this.layoutFlex.Children.Clear();
        }
    }

    private async void LoadAll_Clicked(object sender, EventArgs e)
    {

        string targetFileName = "countersSaves.txt";
        string fileName = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, targetFileName);

        if (!File.Exists(fileName))
        {
            await DisplayAlert("Błąd!", "Nie posiadasz żadnych zapisanych liczników", "Ok");
        }
        else
        {
            string[] lines = File.ReadAllLines(fileName);

            if(lines.Length == 0 || lines==null)
            {
                await DisplayAlert("Uwaga!", "Plik zapisu jest pusty! Nie można wczytać danych!", "OK");
            }
            else
            {
                foreach (string line in lines)
                {
                    string[] data = line.Split(' ');
                    string id = data[0];
                    string name = data[1];
                    string value = data[2];
                    string color = data[3];

                    if (count < 20)
                    {
                        this.layoutFlex.Add(new Controls.CounterView(name,value,id,color));
                        count++;
                    }
                    else
                    {
                        await DisplayAlert("Przekroczono limit!", "Przekroczyłeś limit dostępnych w aplikacji liczników. Rozważ usunięcie któregoś!", "OK");
                    }
                }
            }
        }
    }

    static public string GetNewId()
    {
        return Guid.NewGuid().ToString();
    }
}


