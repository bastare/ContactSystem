namespace ContactSystem.Core.Domain;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public interface IModel<TKey> : IModel
{
	new TKey Id { get; set; }
}

public interface IModel
{
	[Key, DatabaseGenerated ( DatabaseGeneratedOption.Identity )]
	object Id { get; set; }
}