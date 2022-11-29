using Godot;

namespace Infrastructure.Pathfinding
{
    public interface IPathfindingService
    {
        void AddCell(int id, Vector2 position);
        void ConnectCells(int cell1, int cell2);
        int GetCellIdByPosition(Vector2 position);
        Vector2[] GetPath(Vector2 start, Vector2 end);
        void EnableCell(int id);
        void DisableCell(int id);
    }
}