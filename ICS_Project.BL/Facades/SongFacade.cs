using ICS_Project.BL.Facades.Interfaces;
using ICS_Project.BL.Mappers;
using ICS_Project.BL.Models;
using ICS_Project.DAL.Entities;
using ICS_Project.DAL.Mappers;
using ICS_Project.DAL.UnitOfWork;

namespace ICS_Project.BL.Facades;

public class SongFacade(IUnitOfWorkFactory unitOfWorkFactory, SongModelMapper modelMapper) 
    : 
        FacadeBase<SongEntity, SongListModel, SongDetailModel, SongEntityMapper>(unitOfWorkFactory, modelMapper), ISongFacade;

