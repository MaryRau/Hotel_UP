using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_UP
{
    internal class UpdateStatus
    {
        public static void UpdateRoomStatus()
        {
            var rooms = Entities.GetContext().RoomFond.ToList();
            foreach (var room in rooms)
            {
                var reservations = Entities.GetContext().ClientsInHotel.Where(x => x.Number == room.ID && x.DepartureDate != null && x.DepartureDate == DateTime.Today).ToList();

                if (reservations.Count > 0)
                    ChangeStatus(room.ID, "Грязный");
            }
        }

        private static void ChangeStatus(int roomId, string status)
        {
            var report = Entities.GetContext().ReportOfRoomsStatus.FirstOrDefault(r => r.Number == roomId);

            if (report != null)
            {
                report.Status = Entities.GetContext().RoomStatus.Where(x => x.Status == status).Select(x => x.ID).FirstOrDefault();
            }
        }
    }
}
