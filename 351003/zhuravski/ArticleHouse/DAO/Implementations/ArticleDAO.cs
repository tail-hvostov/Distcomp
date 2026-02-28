using ArticleHouse.DAO.Interfaces;
using ArticleHouse.DAO.Models;

namespace ArticleHouse.DAO.Implementations;

class ArticleDAO : IArticleDAO
{
    private readonly IDAO<ArticleModel> internalDAO;

    public ArticleDAO(IDAO<ArticleModel> dao)
    {
        internalDAO = dao;
    }

    public Task<ArticleModel[]> GetAllAsync() => internalDAO.GetAllAsync();
    public Task<ArticleModel> AddNewAsync(ArticleModel model) => internalDAO.AddNewAsync(model);
    public Task DeleteAsync(long id) => internalDAO.DeleteAsync(id);
    public Task<ArticleModel> GetByIdAsync(long id) => internalDAO.GetByIdAsync(id);
    public Task<ArticleModel> UpdateAsync(ArticleModel model) => internalDAO.UpdateAsync(model);
}