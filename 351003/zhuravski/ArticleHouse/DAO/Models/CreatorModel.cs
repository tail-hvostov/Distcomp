using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleHouse.DAO.Models;

[Table("tbl_creator")]
public class CreatorModel : Model<CreatorModel>
{
    public CreatorModel() {}
    public string Password {get; set;} = default!;
    public string Login {get; set;} = default!;
    public string FirstName {get; set;} = default!;
    public string LastName {get; set;} = default!;
};