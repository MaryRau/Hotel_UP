using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_UP
{
    internal class MethodRate
    {
        public float OccupancyRate()
        {
            float totalRooms = Entities.GetContext().RoomFond.Count();

            if (totalRooms == 0)
                return 0;

            DateTime earliest = Entities.GetContext().ClientsInHotel.Where(x => x.DepartureDate != null).OrderBy(x => x.EntryDate).Select(x => x.EntryDate).FirstOrDefault();
            DateTime lastest = (DateTime)Entities.GetContext().ClientsInHotel.Where(x => x.DepartureDate != null).OrderByDescending(x => x.DepartureDate).Select(x => x.DepartureDate).FirstOrDefault();

            float occupiedRooms = Entities.GetContext().ClientsInHotel.Where(x => x.EntryDate >= earliest && x.DepartureDate <= lastest).Select(x => x.Number).Distinct().Count();

            return (occupiedRooms / totalRooms) * 100;
        }
    }
}
