using System;

namespace ReservationsApi.Contracts
{
    public class Registration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}