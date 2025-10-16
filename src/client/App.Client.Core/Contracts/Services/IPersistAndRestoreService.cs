namespace App.Client.Core.Contracts.Services;

/// <summary>
/// 保存还原接口
/// </summary>
public interface IPersistAndRestoreService
{
    void RestoreData();

    void PersistData();
}