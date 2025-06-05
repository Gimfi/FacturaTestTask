using Cysharp.Threading.Tasks;

namespace Core.Launcher
{
    public interface IGameLauncherPreLaunchProcessor
    {
        UniTask RunProcess();
    }
}