using Cysharp.Threading.Tasks;

namespace Commands.Interfaces
{
    public interface ICommand
    {
        void Initialize();
        UniTask Execute();
    }
}