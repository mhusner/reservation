using System;
using ReservationsApi.Data;

namespace ReservationsApi.Tests;

public static class UowData
{
    public static AppDbContext AddTestDataForCourses(this AppDbContext uow)
    {
        uow.Reservations.Add(new Reservation()
        {
            Id = 1, Apid = "s8skjaus6", Date =
                new DateTime(2022,01,09), Name = "Miroslav Holec"
        });
        uow.SaveChanges();

        return uow;
    }
}