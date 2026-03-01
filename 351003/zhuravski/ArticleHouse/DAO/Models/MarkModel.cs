using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleHouse.DAO.Models;

[Table("tbl_mark")]
public class MarkModel : Model<MarkModel>
{
    public string Name {get; set;} = default!;
    public List<ArticleMark> ArticleMarks { get; set; } = [];
}