using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleHouse.DAO.Models;

[Table("tbl_article")]
public class ArticleModel : Model<ArticleModel>
{
    public long CreatorId {get; set;}
    public string Title {get; set;} = default!;
    public string Content {get; set;} = default!;
    //Отметки времени пока решено пропустить.
}