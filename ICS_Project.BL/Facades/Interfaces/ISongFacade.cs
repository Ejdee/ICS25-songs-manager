using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;

namespace ICS_Project.BL.Facades.Interfaces;

public interface ISongFacade : IFacade<SongEntity, SongListModel, SongDetailModel>
{
}