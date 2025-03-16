namespace CinemaApp.Domain.ValueObjects;

public class Seat
{
    public int Row { get; private set; }
    public int Number { get; private set; }
    public bool IsAvailable { get; private set; } = true;
    private Seat() { }
    public Seat(int row, int number)
    {
        Row = row;
        Number = number;
    }

    public void Book()
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Seat is already booked.");

        IsAvailable = false;
    }
}
