using System;

namespace ReservationsApi.Contracts
{
    public class RegistrationCreate
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
    }
}