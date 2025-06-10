using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytesMaestros.Application.Utility
{
	public  class SlotsGenerator
	{
		public static async Task<IEnumerable<DateTime>> GenerateTimeSlots(int orderTypeId)
		{
			var now = DateTimeOffset.Now;
			var timeSlots = new List<DateTime>();
			int startRange = 8;
			int endRange = 22;
			int startDay;

			switch (orderTypeId)
			{
				case 1:
					startDay = now.Hour < 18 ? 0 : 1;
					break;

				case 2:
					startDay = now.Hour < 12 ? 0 : 1;
					break;

				case 3:
					startDay = 3;
					break;

				default:
					startDay = 0;
					break;
			}


			for (int day = startDay; day < 14; day++)
			{
				var currentDay = now.AddDays(day);
				if (DayOfWeek.Saturday == currentDay.DayOfWeek || DayOfWeek.Sunday == currentDay.DayOfWeek)
				{
					continue;
				}

				for (int hour = startRange + 1; hour < endRange; hour++)
				{

					var slot = new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, hour, 0, 0);
					if (slot > DateTime.Now)
					{
						timeSlots.Add(slot);
					}
				}
			}
			var res = timeSlots.OrderBy(x => x.Date).ThenBy(x => !(x.Hour <= 12 || x.Hour >= 19)).ThenBy(x => x.Hour).ToList();
			return await Task.FromResult(res);
		}

	}
}
