using System;
namespace ZitaAsteria
{
    public interface IObjectPool
    {
        void Initialize();
        bool BatchIncreaseSizeIsPercentage { get; set; }
        int BatchIncreaseSize { get; set; }
        //object GetObjectAsObject();
        T GetObject<T>() where T : class;
        int InitialSize { get; set; }
        void ReleaseObject(object obj);
        int CurrentSize { get; }
    }
}
