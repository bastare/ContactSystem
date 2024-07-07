namespace ContactSystem.Core.Common.Interfaces;

public interface IBuilder<out TBuilded>
{
	TBuilded Build ();
}