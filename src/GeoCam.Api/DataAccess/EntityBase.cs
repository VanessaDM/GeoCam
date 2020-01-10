using System.ComponentModel.DataAnnotations;

namespace GeoCam.Api.DataAccess
{
	public interface IEntityBase
	{
	}

	public interface IEntityBase<T>
	{
		T Id { get; }
	}

	public class EntityBase<T> : IEntityBase, IEntityBase<T> where T : struct
	{
		[Key]
		public T Id { get; set; }
	}
}
