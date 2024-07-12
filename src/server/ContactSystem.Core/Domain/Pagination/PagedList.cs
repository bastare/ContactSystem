namespace ContactSystem.Core.Domain.Pagination;

using Interfaces;

public sealed record PagedList<T> : IPagedList<T>
{
	private readonly IReadOnlyList<T> _immutablePagedList;

	public T? this[ int index ] => _immutablePagedList[ index ];

	public int CurrentOffset { get; private set; }

	public int TotalPages { get; private set; }

	public int Limit { get; private set; }

	public int TotalCount { get; private set; }

	public IEnumerator<T> GetEnumerator ()
		=> _immutablePagedList.GetEnumerator ();

	IEnumerator IEnumerable.GetEnumerator ()
		=> GetEnumerator ();

	private PagedList ( IEnumerable<T> items )
		=> _immutablePagedList = [ .. items ];

	public static PagedList<T> Create ( IEnumerable<T> items , int count , int offset , int limit )
	{
		NotNull ( items );
		ParametersAreValid ( limit , offset );

		return new ( items )
		{
			CurrentOffset = offset ,
			TotalPages = ( int ) CalculateTotalPages ( count , limit ) ,
			Limit = limit ,
			TotalCount = count
		};

		static void ParametersAreValid ( int limit , int offset )
		{
			if ( limit <= 0 )
				throw new ArgumentException ( $"{nameof ( limit )}: has a zero or negative value." );

			if ( offset <= 0 )
				throw new ArgumentException ( $"{nameof ( offset )}: has a zero or negative value." );
		}

		static double CalculateTotalPages ( int count , int limit )
			=> Math.Ceiling ( count / ( double ) limit );
	}
}