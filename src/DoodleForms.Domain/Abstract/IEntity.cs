namespace DoodleForms.Domain.Abstract;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}