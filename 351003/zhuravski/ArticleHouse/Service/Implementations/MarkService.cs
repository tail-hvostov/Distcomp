using ArticleHouse.DAO;
using ArticleHouse.DAO.Models;
using ArticleHouse.Service.DTOs;
using ArticleHouse.Service.Exceptions;
using ArticleHouse.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticleHouse.Service.Implementations;

public class MarkService : Service, IMarkService
{
    private readonly ApplicationContext db;
    public MarkService(ApplicationContext db)
    {
        this.db = db;
    }
    public async Task<MarkResponseDTO> CreateMarkAsync(MarkRequestDTO dto)
    {
        MarkModel model = MakeModelFromRequest(dto);
        await db.Marks.AddAsync(model);
        await InvokeDAOMethod(() => db.SaveChangesAsync());
        return MakeResponseFromModel(model);
    }

    public async Task DeleteMarkAsync(long id)
    {
        MarkModel? model = await db.Marks.FirstOrDefaultAsync(o => o.Id == id);
        if (null == model)
        {
            throw new ServiceObjectNotFoundException();
        }
        db.Marks.Remove(model);
        await InvokeDAOMethod(() => db.SaveChangesAsync());
    }

    public async Task<MarkResponseDTO[]> GetAllMarksAsync()
    {
        MarkModel[] models = await db.Marks.ToArrayAsync();
        return [.. models.Select(MakeResponseFromModel)];
    }

    public async Task<MarkResponseDTO> GetMarkByIdAsync(long id)
    {
        MarkModel? model = await db.Marks.FirstOrDefaultAsync(o => o.Id == id);
        if (null == model)
        {
            throw new ServiceObjectNotFoundException();
        }
        return MakeResponseFromModel(model);
    }

    public async Task<MarkResponseDTO> UpdateMarkByIdAsync(long id, MarkRequestDTO dto)
    {
        if (null == dto.Id)
        {
            throw new ServiceException();
        }
        MarkModel? model = await db.Marks.FirstOrDefaultAsync(o => o.Id == dto.Id);
        if (null == model) {
            throw new ServiceObjectNotFoundException();
        }
        ShapeModelFromRequest(ref model, dto);
        await InvokeDAOMethod(() => db.SaveChangesAsync());
        return MakeResponseFromModel(model);
    }

    private static MarkModel MakeModelFromRequest(MarkRequestDTO dto)
    {
        MarkModel result = new();
        ShapeModelFromRequest(ref result, dto);
        return result;
    }

    private static void ShapeModelFromRequest(ref MarkModel model, MarkRequestDTO dto)
    {
        model.Id = dto.Id ?? 0;
        model.Name = dto.Name;
    }

    private static MarkResponseDTO MakeResponseFromModel(MarkModel model)
    {
        return new MarkResponseDTO()
        {
            Id = model.Id,
            Name = model.Name
        };
    }
}