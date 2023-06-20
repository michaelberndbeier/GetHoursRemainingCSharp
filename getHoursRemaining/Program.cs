using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;

int getDaysRemaining()
{
    

    var cultureInfo = new CultureInfo("de-DE");
    var vacDaysLinesWithoutWeekendPath = "/home/mbb/src/dotnetCoreTest/getHoursRemaining/getHoursRemaining/vacDaysWithoutWeekend.csv";
    var vacDaysWithoutWeekend = File.ReadLines(vacDaysLinesWithoutWeekendPath)
        .ToList().ConvertAll(item => DateTime.Parse(item, cultureInfo));

    var today = DateTime.Today;
    var maxDay = vacDaysWithoutWeekend.MaxBy(day => day.Ticks);

    var daysBetween = (int)maxDay.Subtract(today).TotalDays;
    int numWorkDays = 0;

    for (int i = 0; i <= daysBetween; i++)
    {
        switch (today.Add(new TimeSpan(i, 0, 0, 0)).DayOfWeek)
        {
            case DayOfWeek.Saturday:
            case DayOfWeek.Sunday:
                break;
            default:
                numWorkDays++;
                break;
        }
    }

    var vacDaysToCome = vacDaysWithoutWeekend.FindAll(day => day.Ticks >= today.Ticks);
    return numWorkDays - vacDaysToCome.Count;
}

int estimationOfHoursWorkedToday()
{
    var now = DateTime.Now;
    var beginnOfWorkDay = DateTime.Today.Add(new TimeSpan(6, 0, 0));

    switch (now.DayOfWeek)
    {
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday: 
            return 0;
        default:
            var virtuallyWorkedHours = now.Subtract(beginnOfWorkDay).Hours;
            return (virtuallyWorkedHours > 0) ? virtuallyWorkedHours : 0;
    }
}


int unusedVacDays = 0;
var balance = 0;
try
{
    string arg1 = args[0];
    int.TryParse(arg1, out unusedVacDays);
    string arg2 = args[1];
    int.TryParse(arg2, out balance);
}
catch (Exception e)
{
    e.ToString();
    // Console.WriteLine(e.ToString()); 
}


var hoursByDaysRemaining = getDaysRemaining() * 8;
var estimatedHoursWorkedToday = estimationOfHoursWorkedToday();
var hoursRemaining = hoursByDaysRemaining - unusedVacDays * 8 - balance - estimatedHoursWorkedToday;




Console.WriteLine(hoursRemaining);
