namespace Hera.Core.Data;

public interface IQuery<T> {
    IEnumerable<T> Execute();
}
