using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_UP
{
    internal class MethodARM
    {
        public float ARD(DateTime startDate,  DateTime endDate)
        {
            var reservations = Entities.GetContext().ClientsInHotel.Where(x => x.EntryDate >= startDate && x.DepartureDate <= endDate && x.DepartureDate != null).ToList();

            if (reservations.Count == 0)
                return 0;

            float totalSum = 0;
            int totalDays = 0;

            foreach (var res in reservations)
            {
                var servicesCost = Entities.GetContext().ClientsServices.Where(x => x.ClientInHotel == res.ID).Sum(x => x.AdditionalServices.Price);
                var reservationCost = res.RoomFond.RoomCategories.RoomPrices.Select(x => x.Price).FirstOrDefault();

                totalSum += servicesCost != null ? (float)servicesCost : 0;
                totalSum += reservationCost;

                int days = Math.Abs((res.EntryDate - res.DepartureDate).Value.Days);
                totalDays += days;
            }

            return totalSum / totalDays;
        }
    }
}
