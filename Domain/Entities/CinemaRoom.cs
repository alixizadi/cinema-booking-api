using System.ComponentModel.DataAnnotations.Schema;
using CinemaApp.Application.Queries.GetRoomScheduleSeats;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.ValueObjects;

namespace CinemaApp.Domain.Entities;

public class CinemaRoom : BaseEntity
{
    public string Name { get; private set; }
    public int Rows { get; private init; }
    public int Columns { get; private init; }
    private List<Seat> _seats = new();
    private readonly List<MovieSchedule> _schedules = new();
    
    public IReadOnlyCollection<MovieSchedule> Schedules => _schedules.AsReadOnly();

    private CinemaRoom()
    {
        
    }

    public CinemaRoom(string name, int rows = 10, int columns = 8)
    {
        Name = name;
        Rows = rows;
        Columns = columns;
        InitializeSeats();
    }

    private void InitializeSeats()
    {
        _seats = new List<Seat>();
        for (int row = 1; row <= Rows; row++)
        {
            for (int number = 1; number <= Columns; number++)
            {
                _seats.Add(new Seat(row, number));
            }
        }
    }

    public void AddSchedule(MovieSchedule schedule)
    {
        _schedules.Add(schedule);
    }

    public void UpdateDetails(string newName)
    {
        if (!string.IsNullOrWhiteSpace(newName))
        {
            Name = newName;
        }
    }

    public IEnumerable<Seat> Seats()
    {
        InitializeSeats();
        return _seats.AsEnumerable();

    }
}
