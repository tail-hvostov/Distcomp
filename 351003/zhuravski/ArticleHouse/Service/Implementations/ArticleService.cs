using ArticleHouse.DAO;
using ArticleHouse.DAO.Models;
using ArticleHouse.Service.DTOs;
using ArticleHouse.Service.Exceptions;
using ArticleHouse.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticleHouse.Service.Implementations;

public class ArticleService : Service, IArticleService
{
    private readonly ApplicationContext db;
    private readonly IMarkService markService;

    public ArticleService(ApplicationContext db, IMarkService markService)
    {
        this.db = db;
        this.markService = markService;
    }

    public async Task<ArticleResponseDTO[]> GetAllArticlesAsync()
    {
        ArticleModel[] models = await db.Articles.ToArrayAsync();
        return [.. models.Select(MakeResponseFromModel)];
    }

    public async Task<ArticleResponseDTO> CreateArticleAsync(ArticleRequestDTO dto)
    {
        //Пока предположим, что хеш-теги можно добавлять только при создании.
        List<long>? markIds = null;
        if (null != dto.Marks)
        {
            IEnumerable<string> marks = dto.Marks.Distinct();
            List<MarkModel>? markModels = await db.Marks.Where(m => marks.Contains(m.Name)).ToListAsync();
            markIds = [.. markModels.Select(m => m.Id)];
            HashSet<string> foundMarks = [.. markModels.Select(m => m.Name)];
            string[] missingMarks = [.. marks.Where(m => !foundMarks.Contains(m))];
            foreach (string missing in missingMarks)
            {
                MarkRequestDTO request = new()
                {
                    Name = missing
                };
                MarkResponseDTO response = await markService.CreateMarkAsync(request);
                markIds.Add(response.Id);
            }
        }
        ArticleModel model = MakeModelFromRequest(dto);
        await db.Articles.AddAsync(model);
        await InvokeDAOMethod(() => db.SaveChangesAsync());

        if (null != markIds) {
            foreach (int markId in markIds)
            {
                model.ArticleMarks.Add(new ArticleMark
                {
                    ArticleId = model.Id,
                    MarkId = markId
                });
            }
        }

        return MakeResponseFromModel(model);
    }

    public async Task<ArticleResponseDTO> GetArticleByIdAsync(long id)
    {
        ArticleModel? model = await db.Articles.FirstOrDefaultAsync(o => o.Id == id);
        if (null == model)
        {
            throw new ServiceObjectNotFoundException();
        }
        return MakeResponseFromModel(model);
    }

    public async Task DeleteArticleAsync(long id)
    {
        ArticleModel? model = await db.Articles.FirstOrDefaultAsync(o => o.Id == id);
        if (null == model)
        {
            throw new ServiceObjectNotFoundException();
        }
        db.Articles.Remove(model);
        await InvokeDAOMethod(() => db.SaveChangesAsync());
    }

    public async Task<ArticleResponseDTO> UpdateArticleByIdAsync(long id, ArticleRequestDTO dto)
    {
        if (null == dto.Id)
        {
            throw new ServiceException();
        }
        ArticleModel? model = await db.Articles.FirstOrDefaultAsync(o => o.Id == dto.Id);
        if (null == model) {
            throw new ServiceObjectNotFoundException();
        }
        ShapeModelFromRequest(ref model, dto);
        await InvokeDAOMethod(() => db.SaveChangesAsync());
        return MakeResponseFromModel(model);
    }

    private static ArticleModel MakeModelFromRequest(ArticleRequestDTO dto)
    {
        ArticleModel result = new();
        ShapeModelFromRequest(ref result, dto);
        return result;
    }

    private static void ShapeModelFromRequest(ref ArticleModel model, ArticleRequestDTO dto)
    {
        model.Id = dto.Id ?? 0;
        model.CreatorId = dto.CreatorId;
        model.Title = dto.Title;
        model.Content = dto.Content;
    }

    private static ArticleResponseDTO MakeResponseFromModel(ArticleModel model)
    {
        return new ArticleResponseDTO()
        {
            Id = model.Id,
            CreatorId = model.CreatorId,
            Title = model.Title,
            Content = model.Content
        };
    }
}