namespace ICS_Project.DAL.Interfaces;

public interface IEntity
{
    public Guid Id { get; init; }
    public string Name { get; }
    public string Description { get; init; }
}