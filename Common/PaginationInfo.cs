using Microsoft.AspNetCore.Mvc;

namespace SWP391_M2_BL5_G4_SP25.Common
{
	public class PaginationInfo
	{
		public int PageNumber { get; set; } = 1;

		public int PageSize { get; set; } = 2;

		public int TotalPages { get; set; }

		public int TotalRecords { get; set; }

		public void CalculatePagination(int totalRecords)
		{
			TotalRecords = totalRecords;
			TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
			PageNumber = Math.Max(1, Math.Min(PageNumber, TotalPages > 0 ? TotalPages : 1));
		}
	}
}
