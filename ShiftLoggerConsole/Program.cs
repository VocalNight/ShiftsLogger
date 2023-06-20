using Newtonsoft.Json;
using RestSharp;
using ShiftLoggerConsole;
using ShiftLoggerConsole.Models;



viewMenu();

async void viewMenu()
{
    while (true)
    {

        Console.WriteLine("Welcome to the Shift Logger");
        Console.WriteLine("Select an option with the numpad");
        Console.WriteLine(@"
1 - Add new shift
2 - Show shifts
3 - Delete shifts
4 - Exit");

        ConsoleKey choice = Console.ReadKey().Key;

        switch (choice)
        {
            case ConsoleKey.NumPad1:
                await AddShift();
                break;

            case ConsoleKey.NumPad2:
                await ShowShifts();
                break;

            case ConsoleKey.NumPad3:
                await DeleteShift();
                break;

            case ConsoleKey.NumPad4:
                break;
        }
    }

}

async Task DeleteShift()
{

}

async Task ShowShifts()
{
    var jsonClient = new RestClient("https://localhost:7221/api/");
    var request = new RestRequest("ShiftItems");
    var response = jsonClient.ExecuteAsync(request);
    HttpClient _client = new HttpClient();


    List<ShiftModel> shiftsList;

    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
    {
        string rawResponse = response.Result.Content;
        List<ShiftModel> shifts = JsonConvert.DeserializeObject<List<ShiftModel>>(rawResponse);

        foreach (ShiftModel a in shifts)
        {
            Console.WriteLine(a.Id);
        }
        CreateTableEngine.ShowTable(shifts, "Categories");
        shiftsList = shifts.ToList();
    }
}

async Task AddShift()
{
    Console.WriteLine("Insert start date (dd-mm-yyyy)");
    string startDate = Console.ReadLine();

    Console.WriteLine("Insert start time (hh:mm)");
    string startTime = Console.ReadLine();

    DateTime startingTimeDate = DateTime.ParseExact($"{startDate} {startTime}", "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);



    Console.WriteLine("Insert end date (dd-mm-yyyy");
    string endDate = Console.ReadLine();

    Console.WriteLine("Insert end time (hh:mm");
    string endTime = Console.ReadLine();

    DateTime endingTimeDate = DateTime.ParseExact($"{endDate} {endTime}", "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

    TimeSpan duration = endingTimeDate - startingTimeDate;

    ShiftModelDto shift = new()
    {
        day = startingTimeDate.Day,
        startTime = startingTimeDate,
        endTime = endingTimeDate,
        duration = duration.ToString()
    };

    CreateNewUser(shift);

    Console.ReadLine();
}

async void CreateNewUser( ShiftModelDto model )
{

    var json = JsonConvert.SerializeObject(model);

    var jsonClient = new RestClient("https://localhost:7221/api/");
    var request = new RestRequest("ShiftItems", Method.Post).AddJsonBody(json);
    var response = await jsonClient.PostAsync(request);

    if (response.IsSuccessStatusCode)
    {
        await Console.Out.WriteLineAsync("Sucess!");
    }
    else
    {
        await Console.Out.WriteLineAsync(response.ErrorException.ToString());
    }

}
