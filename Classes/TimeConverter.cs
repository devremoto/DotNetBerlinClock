using System;
using System.Collections.Generic;
using System.Linq;

namespace BerlinClock
{
	public class TimeConverter : ITimeConverter
	{
		const char YELLOW = 'Y';
		const char RED = 'R';
		const char OFF = 'O';
		enum LINE_TYPE
		{
			MAIN,
			SECONDARY
		}

		public string convertTime(string aTime)
		{
			List<string> rows = new List<string>();

			string top = GenerateTopRow(aTime);
			string hourMain = GenerateHourMain(aTime);
			string HourSecondary = GenerateHourSecondary(aTime);
			string minutesMain = GenerateMinutesMain(aTime);
			string minutesSecondary = GenerateMinutesSecondary(aTime);

			rows.AddRange(new string[] { top, hourMain, HourSecondary, minutesMain, minutesSecondary });
			return string.Join("\r\n", rows);
		}

		private string GenerateTopRow(string aTime)
		{
			var seconds = GetSeconds(aTime);
			return $"{(seconds % 2 == 0 ? YELLOW : OFF)}";
		}

		private string GenerateHourMain(string aTime)
		{
			var hours = GetHours(aTime);
			return GenerateRow(hours, 4, LINE_TYPE.MAIN, RED);
		}

		private string GenerateHourSecondary(string aTime)
		{
			var hours = GetHours(aTime);
			return GenerateRow(hours, 4, LINE_TYPE.SECONDARY, RED);
		}

		private string GenerateMinutesMain(string aTime)
		{
			var minutes = GetMinutes(aTime);
			var result = GenerateRow(minutes, 11, LINE_TYPE.MAIN, YELLOW).ToArray();
			for (var i = 0; i < result.Length; i++)
			{
				if ((i + 1) % 3 == 0 && result[i] == YELLOW)
				{
					result[i]= RED;
				}
			}
			return string.Join("", result);
		}

		private string GenerateMinutesSecondary(string aTime)
		{
			var minutes = GetMinutes(aTime);
			return GenerateRow(minutes, 4, LINE_TYPE.SECONDARY, YELLOW);
		}

		private int[] SplitTime(string aTime)
		{
			return aTime.Split(':').Select(x => Convert.ToInt32(x)).ToArray();
		}

		private int GetHours(string aTime)
		{
			return SplitTime(aTime)[0];
		}

		private int GetMinutes(string aTime)
		{
			return SplitTime(aTime)[1];
		}

		private int GetSeconds(string aTime)
		{
			return SplitTime(aTime)[2];
		}

		private string GenerateRow(int value, int rowLength, LINE_TYPE lineType, char ligth)
		{
			var strAux = "";
			var count = lineType == LINE_TYPE.MAIN ? value / 5 : value % 5;
			var left = strAux.PadLeft(count, ligth);
			var rigth = strAux.PadLeft(rowLength - count, OFF);
			return $"{left}{rigth}";
		}
	}
}