using System;
using System.ComponentModel.DataAnnotations;

namespace GeoCam.Api.DataAccess
{
	public class Camera : EntityBase<Guid>
	{
		[Required]
		public string Name { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }
	}
}
