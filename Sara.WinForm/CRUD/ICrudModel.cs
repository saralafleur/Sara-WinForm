using System;

namespace Sara.WinForm.CRUD
{
    public interface ICrudModel<TModel, TValue>
    {
        InputMode Mode { get; set; }
        Action<TModel> SaveEvent { get; set; }
        TValue Item { get; set; }
    }
}
