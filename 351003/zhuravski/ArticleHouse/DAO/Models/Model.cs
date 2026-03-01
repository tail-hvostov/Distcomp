using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleHouse.DAO.Models;

public abstract class Model<T> where T : Model<T>
{
    [Column("id")]
    public long Id {get; set;}
    public virtual T Clone()
    {
        return (T)MemberwiseClone();
    }
}