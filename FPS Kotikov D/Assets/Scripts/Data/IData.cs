using System.Collections.Generic;

namespace FPS_Kotikov_D.Data
{
    public interface IData<T>
    {
        //void Save(T[] data, string path = null);
        void Save(Dictionary<int, T> data, string path = null);
        T Load(string path = null);
    }
}