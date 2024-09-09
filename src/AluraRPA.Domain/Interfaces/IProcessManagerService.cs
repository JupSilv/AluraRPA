namespace AluraRPA.Domain.Interfaces;
public interface IProcessManagerService
{
    void TaskKillCmd(string imageName);
    Task WaitProcessCpuIdle(string processName, int waitForProcessDelay = 1000);
}