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
4 - Register an Employee
5 - Delete an Employee
6 - Show Employees
7 - Exit");

        ConsoleKey choice = Console.ReadKey(intercept: true).Key;

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
                await ShowEmployees();
                break;

            case ConsoleKey.NumPad5:
                await ShowEmployees();
                break;

            case ConsoleKey.NumPad6:
                await ShowEmployees();
                break;

            case ConsoleKey.NumPad7:
                Console.Clear();
                Console.Write("Bye!");
                return;
        }
    }

}

async Task DeleteShift()
{
    Console.WriteLine("Inform a id to delete");
    string id = Console.ReadLine();


    var jsonClient = new RestClient("https://localhost:7221/api/");

    var request = new RestRequest($"ShiftItems/{id}", Method.Delete);
    request.AddHeader("Content-Type", "application/json");

    var response = jsonClient.Delete(request);

    if (response.IsSuccessStatusCode)
    {
        await Console.Out.WriteLineAsync("Sucess!");
    }
    else
    {
        await Console.Out.WriteLineAsync(response.ErrorException.ToString());
    }
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

        CreateTableEngine.ShowTable(shifts, "Shifts");
        shiftsList = shifts.ToList();
    }
}

async Task ShowEmployees()
{

    var jsonClient = new RestClient("https://localhost:7221/api/");
    var request = new RestRequest("Employees");
    var response = jsonClient.ExecuteAsync(request);
    HttpClient _client = new HttpClient();

    List<EmployeeModel> EmployeeList;

    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
    {
        List<EmployeeModelDto> employeesDto = new();
        string rawResponse = response.Result.Content;
        List<EmployeeModel> employees = JsonConvert.DeserializeObject<List<EmployeeModel>>(rawResponse);

        foreach (var employee in employees)
        {
            EmployeeModelDto employeeDto = new()
            {
                Id = employee.EmployeeId,
                Name = employee.EmployeeName,
            };

            employeesDto.Add(employeeDto);
        }

        CreateTableEngine.ShowTable(employeesDto, "Employees");
    }
}

async Task AddShift()
{
    Console.Clear();
    Console.WriteLine("Insert start date (dd-mm-yyyy)");
    string startDate = Console.ReadLine();

    Console.Clear();
    Console.WriteLine("Insert start time (hh:mm)");
    string startTime = Console.ReadLine();

    DateTime startingTimeDate = DateTime.ParseExact($"{startDate} {startTime}", "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

    Console.Clear();
    Console.WriteLine("Insert end date (dd-mm-yyyy)");
    string endDate = Console.ReadLine();

    Console.Clear();
    Console.WriteLine("Insert end time (hh:mm)");
    string endTime = Console.ReadLine();

    DateTime endingTimeDate = DateTime.ParseExact($"{endDate} {endTime}", "dd-MM-yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

    TimeSpan duration = endingTimeDate - startingTimeDate;

    Console.Clear();
    ShowEmployees();
    Console.WriteLine("Insert the Id of the employee");
    int employeeId = int.Parse(Console.ReadLine());


    ShiftModelDto shift = new()
    {
        day = startingTimeDate.Day,
        startTime = startingTimeDate,
        endTime = endingTimeDate,
        duration = duration.ToString(),
        EmployeeId = employeeId
    };

    CreateNewShift(shift);

    Console.WriteLine("Press any button to continue");
    Console.ReadLine();
    Console.Clear();
}

async void CreateNewShift( ShiftModelDto model )
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
