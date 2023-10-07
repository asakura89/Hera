namespace Hera.Core.Data;

public interface ICommand<T> {
    void Execute(T entity);
}
