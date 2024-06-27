using System.Collections;
using System.Globalization;
using Plugin.Maui.Calendar.Controls.Interfaces;
using Plugin.Maui.Calendar.Models;

namespace Plugin.Maui.Calendar.Shared.Controls.SelectionEngines;

public class MultiSelectionEngine : ISelectionEngine
{
    private readonly HashSet<DateTime> _selectedDates;

    public MultiSelectionEngine()
    {
        _selectedDates = [];
    }

    public string GetSelectedDateText(string selectedDateTextFormat, CultureInfo culture)
    {
        string dates = "";
        if (_selectedDates?.Any(item => item > DateTime.MinValue) == true)
        {
            dates = _selectedDates
                .Where(item => item > DateTime.MinValue)
                .Select(item => item.ToString(selectedDateTextFormat, culture))
                .Aggregate((a, b) => $"{a}, {b}");
        }

        return dates ?? string.Empty;
    }

    public bool TryGetSelectedEvents(EventCollection allEvents, out ICollection selectedEvents)
    {
        if(_selectedDates is null)
        {
            selectedEvents = null;
            return false;
        }
        return allEvents.TryGetValues(_selectedDates, out selectedEvents);
    }

    public bool IsDateSelected(DateTime dateToCheck)
    {
        return _selectedDates?.Contains(dateToCheck)??false;
    }

    public List<DateTime> PerformDateSelection(
        DateTime dateToSelect,
        List<DateTime> disabledDates = null
    )
    {
        if (!_selectedDates.Remove(dateToSelect))
        {
            if (disabledDates is null || !disabledDates.Contains(dateToSelect))
            {
                _selectedDates.Add(dateToSelect);
            }
        }

        return [.. _selectedDates];
    }

    public void ClearSelection()
    {
        _selectedDates.Clear();
    }
    public void UpdateDateSelection(List<DateTime>? datesToSelect)
    {
        datesToSelect?.ForEach(date => _selectedDates.Add(date));
    }
}
